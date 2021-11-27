using System.ComponentModel;

namespace LogoFX.Core.Specs
{
    public abstract class TestClassBase : INotifyPropertyChanged
    {
        public abstract event PropertyChangedEventHandler? PropertyChanged;

        public abstract int Number
        {
            get;
            set;
        }
    }
}
