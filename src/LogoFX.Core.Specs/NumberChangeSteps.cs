﻿using LogoFX.Core.Specs.Common;

namespace LogoFX.Core.Specs
{
    [Binding]
    internal sealed class NumberChangeSteps
    {
        private readonly InvocationScenarioDataStore _invocationScenarioDataStore;

        public NumberChangeSteps(ScenarioContext scenarioContext)
        {
            _invocationScenarioDataStore = new InvocationScenarioDataStore(scenarioContext);
        }

        [When(@"The number is changed to (.*) in silent mode")]
        public void WhenTheNumberIsChangedToInSilentMode(int value)
        {
            var @class = _invocationScenarioDataStore.Class as TestClassBase;
            @class.UpdateSilent(() =>
            {
                @class.Number = value;
            });
        }

        [When(@"The number is changed to (.*) in regular mode")]
        public void WhenTheNumberIsChangedToInRegularMode(int value)
        {
            var @class = _invocationScenarioDataStore.Class as TestClassBase;
            @class.Number = value;
        }

        [When(@"The number is changed to (.*) via SetProperty API")]
        public void WhenTheNumberIsChangedToViaSetPropertyAPI(int value)
        {
            var @class = _invocationScenarioDataStore.Class as TestClassBase;
            @class.Number = value;
        }
    }
}
