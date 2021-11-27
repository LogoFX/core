using System.Reflection;
using LogoFX.Core.Specs.Common;

namespace LogoFX.Core.Specs
{
    [Binding]
    public sealed class CommonSteps
    {
        private readonly InvocationScenarioDataStoreBase _invocationScenarioDataStoreBase;

        public CommonSteps(ScenarioContext scenarioContext)
        {
            _invocationScenarioDataStoreBase = new InvocationScenarioDataStoreBase(scenarioContext);
        }

        [When(@"The '(.*)' is created")]
        public void WhenTheIsCreated(string name)
        {
            var @class = TestClassFactory.CreateTestClass(Assembly.GetExecutingAssembly(), name);
            if (@class != null)
            {
                var isCalledRef = TestClassHelper.ListenToPropertyChange(@class, "Number");
                _invocationScenarioDataStoreBase.Class = @class;
                _invocationScenarioDataStoreBase.IsCalledRef = isCalledRef;
            }
        }
    }
}
