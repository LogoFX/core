using System;
using System.Threading;
using System.Windows.Threading;

namespace LogoFX.Client.Core.Platform.NETCore.Specs
{
    public class TestPlatformDispatch : PlatformDispatch
    {
        private readonly PlatformDispatch _dispatch;

        public TestPlatformDispatch(PlatformDispatch dispatch)
        {
            _dispatch = dispatch;
        }

        internal bool IsCustomActionInvoked { get; private set; }

        internal new void OnUiThread(DispatcherPriority priority, Action action)
        {
            IsCustomActionInvoked = true;
            _dispatch.OnUiThread(priority, action);
        }

        internal new void InitializeDispatch()
        {
            _dispatch.InitializeDispatch();
        }
    }
}
