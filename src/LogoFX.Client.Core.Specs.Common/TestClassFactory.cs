using System.ComponentModel;
using System.Reflection;

namespace LogoFX.Client.Core.Specs.Common
{
    public static class TestClassFactory
    {
        public static INotifyPropertyChanged CreateTestClass(Assembly assembly, string name, params object?[]? args)
        {
            return TestClassHelper.CreateTestClassImpl(assembly, name, args);
        }
    }
}
