using System;
using System.Threading;

namespace LogoFX.Client.Core.Specs.Dispatcher
{
    class FakeDispatch : IDispatch
    {
        internal bool IsBeginOnUiThreadCalled { get; private set; }

        internal bool IsOnUiThreadCalled { get; private set; }

        public void BeginOnUiThread(Action action)
        {
            IsBeginOnUiThreadCalled = true;
        }

        public void OnUiThread(Action action)
        {
            IsOnUiThreadCalled = true;
        }

        public void InitializeDispatch() {}
    }
}
