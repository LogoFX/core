using System.Threading;

namespace LogoFX.Client.Core.Specs.Common
{
    public class TestOverridenPropertyInfoClass : TestNameClass
    {
        private readonly IDispatch _dispatch;

        public TestOverridenPropertyInfoClass(IDispatch dispatch)
        {
            _dispatch = dispatch;
        }

        protected override IDispatch GetDispatch()
        {
            return _dispatch;
        }
    }
}