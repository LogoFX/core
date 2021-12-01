using LogoFX.Core.Specs.Common;

namespace LogoFX.Core.Specs
{
    [Binding]
    public sealed class NotifyPropertyChangedSteps
    {
        private readonly InvocationScenarioDataStoreBase _invocationScenarioDataStoreBase;

        public NotifyPropertyChangedSteps(ScenarioContext scenarioContext)
        {
            _invocationScenarioDataStoreBase = new InvocationScenarioDataStoreBase(scenarioContext);
        }

        [Then(@"The before value update logic is invoked before the value update")]
        public void ThenTheBeforeValueUpdateLogicIsInvokedBeforeTheValueUpdate()
        {
            var @class = _invocationScenarioDataStoreBase.Class as TestBeforeValueUpdateClass;
            @class.PreviousValue.Should().Be(4);
        }

        [Then(@"The after value update logic is invoked after the value update")]
        public void ThenTheAfterValueUpdateLogicIsInvokedAfterTheValueUpdate()
        {
            var @class = _invocationScenarioDataStoreBase.Class as TestAfterValueUpdateClass;
            @class.Number.Should().Be(6);
        }
    }
}
