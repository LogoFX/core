namespace LogoFX.Client.Core.Specs.Common
{
    public class TestNameClass : TestClassBase
    {
        public override int Number
        {
            get => 0;
            set => NotifyOfPropertyChange();
        }
    }
}