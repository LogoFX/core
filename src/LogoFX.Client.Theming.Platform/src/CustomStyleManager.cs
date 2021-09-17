using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;

namespace LogoFX.Client.Theming
{
    public sealed class CustomStyleManager : ICustomStyleManager
    {
        #region Fields

        private readonly Dictionary<Guid, List<CustomStyleBase>> _customStyles =
            new Dictionary<Guid, List<CustomStyleBase>>();

        #endregion

        #region Private Members

        private void AddCustomStyle(Guid id, CustomStyleBase customStyle)
        {
            if (!_customStyles.TryGetValue(id, out var list))
            {
                list = new List<CustomStyleBase>();
                _customStyles.Add(id, list);
            }

            list.Add(customStyle);
        }

        private void AddDirectAssembly(Assembly assembly)
        {
            var types = assembly.ExportedTypes.Where(x => x.IsSubclassOf(typeof(ResourceDictionary)));

            foreach (var resourceDictionaryType in types)
            {
                var at = resourceDictionaryType.GetCustomAttribute<CustomControlStyleAttribute>();
                if (at == null)
                {
                    continue;
                }

                AddCustomStyle(at.Id, new CompiledCustomStyle(at.Name, resourceDictionaryType));
            }
        }

        private void AddXaml(Guid id, string name, string xamlText)
        {
            AddCustomStyle(id, new RawCustomStyle(name, xamlText));
        }

        private bool RemoveXaml(Guid id, string name)
        {
            if (!_customStyles.TryGetValue(id, out var list))
            {
                return false;
            }

            var item = list.SingleOrDefault(x => x.Name == name);

            if (item == null)
            {
                return false;
            }

            list.Remove(item);

            if (list.Count == 0)
            {
                _customStyles.Remove(id);
            }

            return true;
        }

        #endregion

        #region ICustomStyleManager

        string[] ICustomStyleManager.GetStyleNames(Guid id)
        {
            return _customStyles.TryGetValue(id, out var customStyles)
                ? customStyles.Select(x => x.Name).ToArray()
                : null;
        }

        CustomColor[] ICustomStyleManager.GetColors(Guid id, string customStyleName)
        {
            var colors = _customStyles.TryGetValue(id, out var customStyles)
                ? customStyles.SingleOrDefault(x => x.Name == customStyleName)?.GetColors()
                : null;

            return colors?.ToArray();
        }

        ResourceDictionary ICustomStyleManager.GetCustomResourceDictionary(Guid id, string name, ColorEntry[] colorEntries)
        {
            if (!_customStyles.TryGetValue(id, out var customStyles))
            {
                return null;
            }

            var customStyle = customStyles.SingleOrDefault(x => x.Name == name);
            return customStyle?.GetCustomResourceDictionary(colorEntries);
        }

        void ICustomStyleManager.AddDirectAssembly(Assembly assembly)
        {
            AddDirectAssembly(assembly);
        }
        
        void ICustomStyleManager.AddXaml(Guid id, string name, string xamlText)
        {
            AddXaml(id, name, xamlText);
        }

        bool ICustomStyleManager.RemoveXaml(Guid id, string name)
        {
            return RemoveXaml(id, name);
        }

        #endregion
    }
}