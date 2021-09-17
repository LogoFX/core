using System.Windows.Threading;
using LogoFX.Client.Core.Specs.Common;

namespace LogoFX.Client.Core.Platform.NETCore.Specs
{
    public class TestCustomActionInvocationClass : TestClassBase
    {
        private readonly TestPlatformDispatch _dispatch;

        public TestCustomActionInvocationClass(TestPlatformDispatch dispatch)
        {
            _dispatch = dispatch;
            _dispatch.InitializeDispatch();
        }

        private int _number;
        public override int Number
        {
            get => _number;
            set => SetProperty(ref _number, value, new SetPropertyOptions
            {
               CustomActionInvocation = action => _dispatch.OnUiThread(DispatcherPriority.DataBind, action)
            });
        }
    }
}
