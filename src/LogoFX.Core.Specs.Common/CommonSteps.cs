using FluentAssertions;
using TechTalk.SpecFlow;

namespace LogoFX.Core.Specs.Common
{
    [Binding]
    public sealed class CommonSteps
    {
        private readonly InvocationScenarioDataStoreBase _invocationScenarioDataStoreBase;

        public CommonSteps(ScenarioContext scenarioContext)
        {
            _invocationScenarioDataStoreBase = new InvocationScenarioDataStoreBase(scenarioContext);
        }

        [Then(@"The property change notification result is '(.*)'")]
        public void ThenThePropertyChangeNotificationResultIs(string expectedResultStr)
        {
            bool.TryParse(expectedResultStr, out var expectedResult);
            var isCalledRef = _invocationScenarioDataStoreBase.IsCalledRef;
            isCalledRef.Value.Should().Be(expectedResult);
        }
    }
}
