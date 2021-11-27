using System.ComponentModel;

namespace LogoFX.Core.Specs
{
    public class TestRegularClass : TestClassBase
    {
        protected readonly NotifyPropertyChangedCore<TestRegularClass> NotifyPropertyChanged;

        public TestRegularClass()
        {
            NotifyPropertyChanged = new NotifyPropertyChangedCore<TestRegularClass>(this);
        }

        private int _number;

        public override int Number
        {
            get => _number;
            set => NotifyPropertyChanged.SetProperty(ref _number, value);
        }

        public override event PropertyChangedEventHandler? PropertyChanged
        {
            add => NotifyPropertyChanged.PropertyChanged += value;
            remove => NotifyPropertyChanged.PropertyChanged -= value;
        }
    }
}
