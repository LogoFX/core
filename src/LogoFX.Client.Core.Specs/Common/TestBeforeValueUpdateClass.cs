namespace LogoFX.Client.Core.Specs.Common
{
    public class TestBeforeValueUpdateClass : TestClassBase
    {
        private int _number = 4;
        public override int Number
        {
            get => _number;
            set => SetProperty(ref _number, value, new SetPropertyOptions()
            {
                BeforeValueUpdate = () => PreviousValue = _number
            });
        }

        public int PreviousValue { get; private set; }
    }
}