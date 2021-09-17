using System.Collections.Generic;

namespace LogoFX.Client.Theming
{
    public interface IThemeTree : ITheme
    {
        IEnumerable<ITheme> Nodes { get; }

        ITheme ActiveNode { get; set; }
    }
}