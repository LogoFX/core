#nullable enable

using System.ComponentModel;

namespace LogoFX.Core.Specs.Common
{
    public static class TestClassHelper
    {
        public static ValueWrapper ListenToPropertyChange(INotifyPropertyChanged @class, string propertyName)
        {
            var isCalled = false;
            var isCalledRef = new ValueWrapper(isCalled);
            @class.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == propertyName)
                {
                    isCalledRef.Value = true;
                }
            };
            return isCalledRef;
        }
    }
}