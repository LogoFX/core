using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Threading;

namespace LogoFX.Client.Theming
{
    public sealed class ThemesManager : IThemesManager
    {
        #region Fields

        private readonly Dispatcher _dispatcher;

        private ITheme _currentTheme;
        private readonly SortedList<int, ITheme> _themes = new SortedList<int, ITheme>();
        private ResourceDictionary[] _resources;
        private int _currentIndex = -1;
        private bool _isBusy;

        #endregion

        #region Constructors

        public ThemesManager()
        {
            _dispatcher = Application.Current.Dispatcher;
        }

        #endregion

        #region Public Methods

        public void AddDirectAssembly(Assembly assembly)
        {
            var types = assembly.ExportedTypes.Where(x => x.IsSubclassOf(typeof(ResourceDictionary)));

            var list = new List<Tuple<ThemeResourceDictionaryAttribute, Type>>();

            foreach (var t in types)
            {
                var at = t.GetCustomAttribute<ThemeResourceDictionaryAttribute>();
                if (at != null)
                {
                    list.Add(new Tuple<ThemeResourceDictionaryAttribute, Type>(at, t));
                }
            }

            var dic = new Dictionary<string, ITheme>();
            foreach (var tuple in list)
            {
                var theme = GetOrCreateTheme(tuple.Item1, tuple.Item2, list, dic);
                if (string.IsNullOrEmpty(tuple.Item1.ParentThemeName))
                {
                    _themes.Add(theme.Order, theme);
                }
            }
        }

        #endregion

        #region Private Members

        private ITheme GetOrCreateTheme(
            ThemeResourceDictionaryAttribute attr,
            Type type,
            List<Tuple<ThemeResourceDictionaryAttribute, Type>> list,
            Dictionary<string, ITheme> dic)
        {
            // ReSharper disable once InlineOutVariableDeclaration
            ITheme theme;

            if (!dic.TryGetValue(attr.Name, out theme))
            {
                theme = new TypeTheme(attr.Name, type, attr.Order);
                dic.Add(attr.Name, theme);

                if (!string.IsNullOrEmpty(attr.ParentThemeName))
                {
                    var parentTuple = list.Single(x => x.Item1.Name == attr.ParentThemeName);
                    var parentTheme = (ThemeTree) GetOrCreateTheme(parentTuple.Item1, parentTuple.Item2, list, dic);
                    parentTheme.AddNode(theme);
                }
            }

            return theme;
        }

        private void Unload(ResourceDictionary[] resources)
        {
            Debug.Assert(resources != null);

            foreach (var rd in resources)
            {
                Application.Current.Resources.MergedDictionaries.Remove(rd);
                _dispatcher.BeginInvoke(new Action(() =>
                {
                    _dispatcher.CheckAccess();
                    rd.Clear();
                }));
            }
        }

        private void Load(ITheme theme)
        {
            Debug.Assert(_resources == null);

            _resources = theme.LoadResources();

            foreach (var rd in _resources)
            {
                Application.Current.Resources.MergedDictionaries.Add(rd);
            }
        }

        private void Refresh()
        {
            if (_resources != null)
            {
                Unload(_resources);
                _resources = null;
            }

            if (_currentTheme != null)
            {
                Load(_currentTheme);
            }
        }

        private void SetCurrentTheme(ITheme theme)
        {
            if (_currentTheme == theme)
            {
                return;
            }

            if (_currentTheme != null)
            {
                var tnc = _currentTheme as IThemeNotifyChanged;
                if (tnc != null)
                {
                    tnc.Updated -= Updated;
                }
                Unload(_resources);
                _resources = null;
            }

            _currentTheme = theme;

            if (_currentTheme != null)
            {
                var tnc = _currentTheme as IThemeNotifyChanged;
                if (tnc != null)
                {
                    tnc.Updated += Updated;
                }
                Load(_currentTheme);
            }

            // ReSharper disable once ExplicitCallerInfoArgument
            OnPropertyChanged(nameof(CurrentTheme));

            CurrentIndex = _themes.IndexOfValue(CurrentTheme);
        }

        private void Updated(object sender, EventArgs e)
        {
            Refresh();
        }

        #endregion

        #region INotfyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var args = new PropertyChangedEventArgs(propertyName);

            if (!_dispatcher.CheckAccess())
            {
                _dispatcher.Invoke(() => { PropertyChanged(this, args); });
            }
            else
            {
                PropertyChanged(this, args);
            }
        }

        #endregion

        #region IThemesManager

        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                if (_isBusy == value)
                {
                    return;
                }

                _isBusy = value;
                OnPropertyChanged();
            }
        }

        public ITheme CurrentTheme
        {
            get { return _currentTheme; }
            set { SetCurrentTheme(value); }
        }

        public IEnumerable<ITheme> Themes
        {
            get { return _themes.Values; }
        }

        public int CurrentIndex
        {
            get { return _currentIndex; }
            set
            {
                if (_currentIndex == value)
                {
                    return;
                }

                _currentIndex = value;
                OnPropertyChanged();

                var key = _themes.Keys[_currentIndex];
                var theme = _themes[key];
                SetCurrentTheme(theme);
                CurrentTheme = _themes[key];
            }
        }

        #endregion
    }
}