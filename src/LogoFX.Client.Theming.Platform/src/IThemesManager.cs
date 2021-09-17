using System.Collections.Generic;
using System.ComponentModel;

namespace LogoFX.Client.Theming
{
    public interface IThemesManager : INotifyPropertyChanged
    {
        bool IsBusy { get; }

        ITheme CurrentTheme { get; set; }

        int CurrentIndex { get; set; }

        IEnumerable<ITheme> Themes { get; }
    }
}