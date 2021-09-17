using System;
using System.Collections.Generic;
using System.Windows;

namespace LogoFX.Client.Theming
{
    internal abstract class ThemeBase : ITheme, IThemeNotifyChanged
    {
        #region Fields

        private ResourceDictionary[] _cache;
        
        private ColorTheme[] _colorThemes;

        #endregion

        #region Constructors

        protected ThemeBase(string name, int order)
        {
            Name = name;
            Order = order;
        }

        #endregion

        #region Protected

        protected void RaiseUpdated()
        {
            Updated(this, EventArgs.Empty);
        }

        #endregion

        #region ITheme

        public string Name { get; private set; }

        public int Order { get; private set; }

        protected abstract ResourceDictionary[] LoadResoucesInternal(HashSet<string> dics);

        protected ColorTheme[] GetColorThemes()
        {
            return _colorThemes;
        }

        public void ApplyColorThemes(params ColorTheme[] colorThemes)
        {
            _colorThemes = colorThemes;
            RaiseUpdated();
        }

        public ResourceDictionary[] LoadResources()
        {
            var dics = new HashSet<string>();
            _cache = LoadResoucesInternal(dics);
            return _cache;
        }

        #endregion

        #region IThemeNotifyChanged

        public event EventHandler Updated = delegate { };

        #endregion
    }
}