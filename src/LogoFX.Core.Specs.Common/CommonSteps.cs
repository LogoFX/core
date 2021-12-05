using FluentAssertions;
using TechTalk.SpecFlow;

namespace LogoFX.Core.Specs.Common
{
    [Binding]
    public sealed class CommonSteps
    {
        private readonly InvocationScenarioDataStore _invocationScenarioDataStore;

        public CommonSteps(ScenarioContext scenarioContext)
        {
            _invocationScenarioDataStore = new InvocationScenarioDataStore(scenarioContext);
        }

        [Then(@"The property change notification result is '(.*)'")]
        public void ThenThePropertyChangeNotificationResultIs(string expectedResultStr)
        {
            bool.TryParse(expectedResultStr, out var expectedResult);
            var isCalledRef = _invocationScenarioDataStore.IsCalledRef;
            isCalledRef.Value.Should().Be(expectedResult);
        }
    }
}
