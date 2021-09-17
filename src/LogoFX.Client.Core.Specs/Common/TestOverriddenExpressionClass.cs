using System.Threading;

namespace LogoFX.Client.Core.Specs.Common
{
    public class TestOverriddenExpressionClass : TestNameClass
    {
        private readonly IDispatch _dispatch;

        public TestOverriddenExpressionClass(IDispatch dispatch)
        {
            _dispatch = dispatch;
        }

        protected override IDispatch GetDispatch()
        {
            return _dispatch;
        }
    }
}