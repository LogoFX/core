using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Threading;

namespace LogoFX.Client.Theming
{
    internal sealed class TypeTheme : ThemeTree
    {
        private readonly Dispatcher _dispatcher;
        private readonly Type _resourceDictionaryType;

        public TypeTheme(string name, Type resourceDictionaryType, int order)
            : base(name, order)
        {
            _dispatcher = Application.Current.Dispatcher;
            _resourceDictionaryType = resourceDictionaryType;
        }

        protected override ResourceDictionary[] LoadResoucesInternal(HashSet<string> dics)
        {
            var result = new List<ResourceDictionary>(base.LoadResoucesInternal(dics));

            ResourceDictionary resourceDictionary = null;
            ResourceDictionary rd = null;

            _dispatcher.Invoke(() =>
            {
                resourceDictionary = new ResourceDictionary();
                var ctor = _resourceDictionaryType.GetConstructor(Type.EmptyTypes);
                Debug.Assert(ctor != null, "ctor != null");
                rd = (ResourceDictionary) ctor.Invoke(new object[] {});
            });

            AddEntries(rd, resourceDictionary, dics);

            result.Add(resourceDictionary);
            
            return result.ToArray();
        }

        private void AddEntries(ResourceDictionary source, ResourceDictionary dest, HashSet<string> dics)
        {
            var localPath = source.Source?.LocalPath;

            if (localPath != null && dics.Contains(localPath))
            {
                return;
            }

            dics.Add(localPath);

            foreach (var md in source.MergedDictionaries)
            {
                AddEntries(md, dest, dics);
            }

            foreach (DictionaryEntry de in source)
            {
                var key = de.Key;
                if (dest.Contains(key))
                {
                    continue;
                }

                object value = de.Value;
                var colorThemes = GetColorThemes();
                if (colorThemes != null && colorThemes.Length > 0)
                {
                    //TODO: optimise Color Theme code
                    var colorEntry = colorThemes
                        .SelectMany(x => x.Entries)
                        .OfType<ColorEntry>()
                        .SingleOrDefault(x => Equals(x.ResourceKey, key));
                    
                    if (colorEntry != null)
                    {
                        value = colorEntry.ToColor();
                    }
                }

                _dispatcher.Invoke(() => dest.Add(key, value));
            }
        }
    }
}