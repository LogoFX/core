using System.ComponentModel;
using System.Reflection;

namespace LogoFX.Client.Core.Specs.Common
{
    internal static class TestClassFactory
    {
        internal static INotifyPropertyChanged CreateTestClass(string name, params object?[]? args)
        {
            return TestClassHelper.CreateTestClassImpl(Assembly.GetExecutingAssembly(), name, args);
        }
    }
}
