using System.Threading;

namespace LogoFX.Client.Core.Specs.Common
{
    public class TestOverriddenDispatcherClass : TestRegularClass
    {
        private readonly IDispatch _dispatch;

        public TestOverriddenDispatcherClass(IDispatch dispatch)
        {
            _dispatch = dispatch;
        }

        protected override IDispatch GetDispatch()
        {
            return _dispatch;
        }
    }
}