using System.Threading;

namespace LogoFX.Client.Core.Specs.Common
{
    public class TestOverridenNameClass : TestNameClass
    {
        private readonly IDispatch _dispatch;

        public TestOverridenNameClass(IDispatch dispatch)
        {
            _dispatch = dispatch;
        }

        protected override IDispatch GetDispatch()
        {
            return _dispatch;
        }
    }
}