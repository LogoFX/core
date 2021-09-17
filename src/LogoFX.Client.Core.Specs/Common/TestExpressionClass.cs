namespace LogoFX.Client.Core.Specs.Common
{
    public class TestExpressionClass : TestClassBase
    {
        public override int Number
        {
            get => 0;
            set => NotifyOfPropertyChange(() => Number);
        }
    }
}