#nullable enable

using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace LogoFX.Client.Core.Specs.Common
{
    public static class TestClassHelper
    {
        public static INotifyPropertyChanged CreateTestClassImpl(Assembly assembly, string name, params object?[]? args)
        {
            var types = assembly.DefinedTypes.ToArray();
            var type = types.FirstOrDefault(t => t.Name == name)?.AsType();
            return type == null ? null : Activator.CreateInstance(type, args) as INotifyPropertyChanged;
        }

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