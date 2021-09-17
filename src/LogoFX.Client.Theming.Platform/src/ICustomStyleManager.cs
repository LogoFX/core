using System;
using System.Reflection;
using System.Windows;

namespace LogoFX.Client.Theming
{   
    public interface ICustomStyleManager
    {
        string[] GetStyleNames(Guid id);       

        CustomColor[] GetColors(Guid id, string customStyleName);
        
        ResourceDictionary GetCustomResourceDictionary(Guid id, string name, ColorEntry[] colorEntries);

        void AddDirectAssembly(Assembly assembly);

        void AddXaml(Guid id, string name, string xamlText);

        bool RemoveXaml(Guid id, string name);
    }
}