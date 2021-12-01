using System.Reflection;
using LogoFX.Core.Specs.Common;

namespace LogoFX.Core.Specs
{
    [Binding]
    public sealed class CommonSteps
    {
        private readonly InvocationScenarioDataStore _invocationScenarioDataStore;

        public CommonSteps(ScenarioContext scenarioContext)
        {
            _invocationScenarioDataStore = new InvocationScenarioDataStore(scenarioContext);
        }

        [When(@"The '(.*)' is created")]
        public void WhenTheIsCreated(string name)
        {
            var @class = TestClassFactory.CreateTestClass(Assembly.GetExecutingAssembly(), name);
            if (@class != null)
            {
                var isCalledRef = TestClassHelper.ListenToPropertyChange(@class, "Number");
                _invocationScenarioDataStore.Class = @class;
                _invocationScenarioDataStore.IsCalledRef = isCalledRef;
            }
        }
    }
}
