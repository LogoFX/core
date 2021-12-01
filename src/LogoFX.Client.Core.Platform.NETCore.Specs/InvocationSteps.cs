using System.Reflection;
using System.Threading;
using FluentAssertions;
using LogoFX.Client.Core.Specs.Common;
using LogoFX.Core.Specs.Common;
using TechTalk.SpecFlow;

namespace LogoFX.Client.Core.Platform.NETCore.Specs
{
    [Binding]
    internal sealed class InvocationSteps
    {
        private readonly DispatcherScenarioDataStore<TestPlatformDispatch> _dispatcherScenarioDataStore;
        private readonly InvocationScenarioDataStore _invocationScenarioDataStore;

        public InvocationSteps(ScenarioContext scenarioContext)
        {
            _dispatcherScenarioDataStore =
                new DispatcherScenarioDataStore<TestPlatformDispatch>(scenarioContext);
            _invocationScenarioDataStore = new InvocationScenarioDataStore(scenarioContext);
        }

        [Given(@"The dispatcher is set to custom dispatcher")]
        public void GivenTheDispatcherIsSetToCustomDispatcher()
        {
            _dispatcherScenarioDataStore.Dispatch = new TestPlatformDispatch(new PlatformDispatch());
        }

        //TODO: Merge with the same class in LogoFX.Client.Core.Specs - pay attention to different dispatcher types
        //Use Step Argument Transformation - context-dependent
        [When(@"The '(.*)' is created with dispatcher")]
        public void WhenTheIsCreatedWithDispatcher(string name)
        {
            var @class = TestClassFactory.CreateTestClass(Assembly.GetExecutingAssembly(), name,
                _dispatcherScenarioDataStore.Dispatch);
            if (@class != null)
            {
                var isCalledRef = TestClassHelper.ListenToPropertyChange(@class, "Number");
                _invocationScenarioDataStore.Class = @class;
                _invocationScenarioDataStore.IsCalledRef = isCalledRef;
            }
        }

        [Then(@"The property change notification is raised via the custom action invocation")]
        public void ThenThePropertyChangeNotificationIsRaisedViaTheCustomActionInvocation()
        {
            var fakeDispatch = _dispatcherScenarioDataStore.Dispatch;
            fakeDispatch.IsCustomActionInvoked.Should().BeTrue();
        }
    }
}
