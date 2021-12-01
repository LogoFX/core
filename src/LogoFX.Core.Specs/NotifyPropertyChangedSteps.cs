using LogoFX.Core.Specs.Common;

namespace LogoFX.Core.Specs
{
    [Binding]
    public sealed class NotifyPropertyChangedSteps
    {
        private readonly InvocationScenarioDataStore _invocationScenarioDataStore;

        public NotifyPropertyChangedSteps(ScenarioContext scenarioContext)
        {
            _invocationScenarioDataStore = new InvocationScenarioDataStore(scenarioContext);
        }

        [Then(@"The before value update logic is invoked before the value update")]
        public void ThenTheBeforeValueUpdateLogicIsInvokedBeforeTheValueUpdate()
        {
            var @class = _invocationScenarioDataStore.Class as TestBeforeValueUpdateClass;
            @class.PreviousValue.Should().Be(4);
        }

        [Then(@"The after value update logic is invoked after the value update")]
        public void ThenTheAfterValueUpdateLogicIsInvokedAfterTheValueUpdate()
        {
            var @class = _invocationScenarioDataStore.Class as TestAfterValueUpdateClass;
            @class.Number.Should().Be(6);
        }
    }
}
