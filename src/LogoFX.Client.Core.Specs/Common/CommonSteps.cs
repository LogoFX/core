using System.Reflection;
using LogoFX.Client.Core.Specs.Dispatcher;
using LogoFX.Core.Specs.Common;

namespace LogoFX.Client.Core.Specs.Common
{
    [Binding]
    public sealed class CommonSteps
    {
        private readonly DispatcherScenarioDataStore<FakeDispatch> _dispatcherScenarioDataStore;
        private readonly InvocationScenarioDataStore _invocationScenarioDataStore;

        public CommonSteps(ScenarioContext scenarioContext)
        {
            _dispatcherScenarioDataStore = new DispatcherScenarioDataStore<FakeDispatch>(scenarioContext);
            _invocationScenarioDataStore = new InvocationScenarioDataStore(scenarioContext);
        }

        [When(@"The '(.*)' is created")]
        public void WhenTheIsCreated(string name)
        {
            var @class = TestClassFactory.CreateTestClass(Assembly.GetExecutingAssembly(),  name);
            if (@class != null)
            {
                var isCalledRef = TestClassHelper.ListenToPropertyChange(@class, "Number");
                _invocationScenarioDataStore.Class = @class;
                _invocationScenarioDataStore.IsCalledRef = isCalledRef;
            }
        }

        [When(@"The '(.*)' is created with dispatcher")]
        public void WhenTheIsCreatedWithDispatcher(string name)
        {
            var @class = TestClassFactory.CreateTestClass(Assembly.GetExecutingAssembly(), name, _dispatcherScenarioDataStore.Dispatch);
            if (@class != null)
            {
                var isCalledRef = TestClassHelper.ListenToPropertyChange(@class, "Number");
                _invocationScenarioDataStore.Class = @class;
                _invocationScenarioDataStore.IsCalledRef = isCalledRef;
            }
        }

        [When(@"The '(.*)' is created and empty notification is listened to")]
        public void WhenTheIsCreatedAndEmptyNotificationIsListenedTo(string name)
        {
            var @class = TestClassFactory.CreateTestClass(Assembly.GetExecutingAssembly(), name);
            if (@class != null)
            {
                var isCalledRef = TestClassHelper.ListenToPropertyChange(@class, string.Empty);
                _invocationScenarioDataStore.Class = @class;
                _invocationScenarioDataStore.IsCalledRef = isCalledRef;
            }
        }

        [When(@"The all properties change is invoked")]
        public void WhenTheAllPropertiesChangeIsInvoked()
        {
            var @class = _invocationScenarioDataStore.Class as TestNameClass;
            @class.Refresh();
        }
    }
}