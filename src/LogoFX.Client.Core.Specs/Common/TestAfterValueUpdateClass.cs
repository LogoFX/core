namespace LogoFX.Client.Core.Specs.Common
{
    public class TestAfterValueUpdateClass : TestClassBase
    {
        private int _number = 4;
        public override int Number
        {
            get => _number;
            set => SetProperty(ref _number, value, new SetPropertyOptions()
            {
                AfterValueUpdate = () => _number = 6
            });
        }
    }
}