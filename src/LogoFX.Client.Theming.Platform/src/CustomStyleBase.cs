using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace LogoFX.Client.Theming
{
    internal abstract class CustomStyleBase
    {
        #region Fields
            
        private CustomColor[] _colors;

        #endregion

        #region Constructors

        protected CustomStyleBase(string name)
        {
            Name = name;
        }

        #endregion

        #region Public Methods

        public string Name { get; private set; }

        #endregion

        #region Public Methods

        public CustomColor[] GetColors()
        {
            return _colors ?? (_colors = CreateColors());
        }

        public ResourceDictionary GetCustomResourceDictionary(ColorEntry[] colorEntries)
        {
            var rd = CreateResourceDictionary();

            ResourceDictionary result = new ResourceDictionary();

            foreach (var md in rd.MergedDictionaries)
            {
                result.MergedDictionaries.Add(md);
            }

            HashSet<object> colorKeys;
            if (colorEntries == null || colorEntries.Length == 0)
            {
                colorKeys = new HashSet<object>();
            }
            else
            {
                colorKeys = new HashSet<object>(colorEntries.Select(x => x.ResourceKey));
                foreach (var colorEntry in colorEntries)
                {
                    result.Add(colorEntry.ResourceKey, colorEntry.ToColor());
                }
            }

            foreach (DictionaryEntry de in rd)
            {
                if (de.Value is Color && colorKeys.Contains(de.Key))
                {
                    continue;
                }

                result.Add(de.Key, de.Value);
            }

            return result;
        }

        #endregion

        #region Protected

        protected abstract ResourceDictionary CreateResourceDictionary();

        #endregion

        #region Private Members

        private CustomColor[] CreateColors()
        {
            var result = new List<CustomColor>();

            var rd = CreateResourceDictionary();
            foreach (DictionaryEntry de in rd)
            {
                if (de.Value is Color)
                {
                    result.Add(new CustomColor {Key = de.Key, Color = (Color) de.Value});
                }
            }

            return result.ToArray();
        }

        #endregion
    }
}