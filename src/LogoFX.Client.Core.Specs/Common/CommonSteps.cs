using LogoFX.Client.Core.Specs.Dispatcher;
using TechTalk.SpecFlow;

namespace LogoFX.Client.Core.Specs.Common
{
    [Binding]
    internal sealed class CommonSteps
    {
        private readonly DispatcherScenarioDataStoreBase<FakeDispatch> _dispatcherScenarioDataStoreBase;
        private readonly InvocationScenarioDataStoreBase _invocationScenarioDataStoreBase;

        public CommonSteps(ScenarioContext scenarioContext)
        {
            _dispatcherScenarioDataStoreBase = new DispatcherScenarioDataStoreBase<FakeDispatch>(scenarioContext);
            _invocationScenarioDataStoreBase = new InvocationScenarioDataStoreBase(scenarioContext);
        }

        [When(@"The '(.*)' is created")]
        public void WhenTheIsCreated(string name)
        {
            var @class = TestClassFactory.CreateTestClass(name);
            if (@class != null)
            {
                var isCalledRef = TestClassHelper.ListenToPropertyChange(@class, "Number");
                _invocationScenarioDataStoreBase.Class = @class;
                _invocationScenarioDataStoreBase.IsCalledRef = isCalledRef;
            }
        }

        [When(@"The '(.*)' is created with dispatcher")]
        public void WhenTheIsCreatedWithDispatcher(string name)
        {
            var @class = TestClassFactory.CreateTestClass(name, _dispatcherScenarioDataStoreBase.Dispatch);
            if (@class != null)
            {
                var isCalledRef = TestClassHelper.ListenToPropertyChange(@class, "Number");
                _invocationScenarioDataStoreBase.Class = @class;
                _invocationScenarioDataStoreBase.IsCalledRef = isCalledRef;
            }
        }

        [When(@"The '(.*)' is created and empty notification is listened to")]
        public void WhenTheIsCreatedAndEmptyNotificationIsListenedTo(string name)
        {
            var @class = TestClassFactory.CreateTestClass(name);
            if (@class != null)
            {
                var isCalledRef = TestClassHelper.ListenToPropertyChange(@class, string.Empty);
                _invocationScenarioDataStoreBase.Class = @class;
                _invocationScenarioDataStoreBase.IsCalledRef = isCalledRef;
            }
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

        [When(@"The all properties change is invoked")]
        public void WhenTheAllPropertiesChangeIsInvoked()
        {
            var @class = _invocationScenarioDataStoreBase.Class as TestNameClass;
            @class.Refresh();
        }
    }
}
