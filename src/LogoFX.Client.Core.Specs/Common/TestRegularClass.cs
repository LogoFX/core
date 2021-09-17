namespace LogoFX.Client.Core.Specs.Common
{
    public class TestRegularClass : TestClassBase
    {
        private int _number;
        public override int Number
        {
            get => _number;
            set => SetProperty(ref _number, value);
        }
    }
}
