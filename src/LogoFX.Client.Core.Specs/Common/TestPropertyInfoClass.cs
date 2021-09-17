using System.Reflection;

namespace LogoFX.Client.Core.Specs.Common
{
    public class TestPropertyInfoClass : TestClassBase
    {
        private readonly PropertyInfo _propertyInfo;

        public TestPropertyInfoClass()
        {
            _propertyInfo = GetType().GetRuntimeProperty("Number");
        }

        public override int Number
        {
            get => 0;
            set => NotifyOfPropertyChange(_propertyInfo);
        }
    }
}