using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace LogoFX.Core.Specs.Common
{
    public static class TestClassFactory
    {
        public static INotifyPropertyChanged CreateTestClass(Assembly assembly, string name, params object?[]? args)
        {
            var types = assembly.DefinedTypes.ToArray();
            var type = types.FirstOrDefault(t => t.Name == name)?.AsType();
            return type == null ? null : Activator.CreateInstance(type, args) as INotifyPropertyChanged;
        }
    }
}
