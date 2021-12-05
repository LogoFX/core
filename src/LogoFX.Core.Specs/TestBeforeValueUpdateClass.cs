namespace LogoFX.Core.Specs
{
    public class TestBeforeValueUpdateClass : TestClassBase
    {
        public TestBeforeValueUpdateClass()
        {
            NotifyPropertyChanged = new NotifyPropertyChangedCore<TestBeforeValueUpdateClass>(this);
        }

        protected override NotifyPropertyChangedCore<TestBeforeValueUpdateClass> NotifyPropertyChanged { get; }

        private int _number = 4;
        public override int Number
        {
            get => _number;
            set => NotifyPropertyChanged.SetProperty(ref _number, value, new SetPropertyOptions()
            {
                BeforeValueUpdate = () => PreviousValue = _number
            });
        }

        public int PreviousValue { get; private set; }
    }
}