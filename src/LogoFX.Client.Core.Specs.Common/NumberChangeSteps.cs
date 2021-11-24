﻿using TechTalk.SpecFlow;

namespace LogoFX.Client.Core.Specs.Common
{
    [Binding]
    internal sealed class NumberChangeSteps
    {
        private readonly InvocationScenarioDataStoreBase _invocationScenarioDataStoreBase;

        public NumberChangeSteps(ScenarioContext scenarioContext)
        {
            _invocationScenarioDataStoreBase = new InvocationScenarioDataStoreBase(scenarioContext);
        }

        [When(@"The number is changed to (.*)  in regular mode")]
        public void WhenTheNumberIsChangedToInRegularMode(int value)
        {
            var @class = _invocationScenarioDataStoreBase.Class as TestClassBase;
            @class.Number = value;
        }

        [When(@"The number is changed to (.*) via SetProperty API")]
        public void WhenTheNumberIsChangedToViaSetPropertyAPI(int value)
        {
            var @class = _invocationScenarioDataStoreBase.Class as TestClassBase;
            @class.Number = value;
        }
    }
}
