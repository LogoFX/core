using System.Windows;

namespace LogoFX.Client.Theming
{
    public interface ITheme
    {
        string Name { get; }

        int Order { get; }

        ResourceDictionary[] LoadResources();

        void ApplyColorThemes(params ColorTheme[] colorThemes);
    }
}