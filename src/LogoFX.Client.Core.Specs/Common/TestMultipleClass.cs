namespace LogoFX.Client.Core.Specs.Common
{
    public class TestMultipleClass : NotifyPropertyChangedBase<TestMultipleClass>
    {
        private double _cost;
        public double Cost
        {
            get => _cost;
            set => SetProperty(ref _cost, value);
        }

        private int _quantity;
        public int Quantity
        {
            get => _quantity;
            set => SetProperty(ref _quantity, value, new SetPropertyOptions
            {
                AfterValueUpdate = () => NotifyOfPropertyChange(() => Total)
            });
        }

        public double Total => _cost * _quantity;
    }
}