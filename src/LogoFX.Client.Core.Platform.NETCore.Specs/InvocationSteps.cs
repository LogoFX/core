using System.Reflection;
using System.Threading;
using FluentAssertions;
using LogoFX.Client.Core.Specs.Common;
using TechTalk.SpecFlow;

namespace LogoFX.Client.Core.Platform.NETCore.Specs
{
    [Binding]
    internal sealed class InvocationSteps
    {
        private readonly DispatcherScenarioDataStoreBase<TestPlatformDispatch> _dispatcherScenarioDataStoreBase;
        private readonly InvocationScenarioDataStoreBase _invocationScenarioDataStoreBase;

        public InvocationSteps(ScenarioContext scenarioContext)
        {
            _dispatcherScenarioDataStoreBase =
                new DispatcherScenarioDataStoreBase<TestPlatformDispatch>(scenarioContext);
            _invocationScenarioDataStoreBase = new InvocationScenarioDataStoreBase(scenarioContext);
        }

        [Given(@"The dispatcher is set to custom dispatcher")]
        public void GivenTheDispatcherIsSetToCustomDispatcher()
        {
            _dispatcherScenarioDataStoreBase.Dispatch = new TestPlatformDispatch(new PlatformDispatch());
        }

        //TODO: Merge with the same class in LogoFX.Client.Core.Specs - pay attention to different dispatcher types
        [When(@"The '(.*)' is created here with dispatcher")]
        public void WhenTheIsCreatedHereWithParameter(string name)
        {
            var @class = TestClassHelper.CreateTestClassImpl(Assembly.GetExecutingAssembly(), name,
                _dispatcherScenarioDataStoreBase.Dispatch);
            if (@class != null)
            {
                var isCalledRef = TestClassHelper.ListenToPropertyChange(@class, "Number");
                _invocationScenarioDataStoreBase.Class = @class;
                _invocationScenarioDataStoreBase.IsCalledRef = isCalledRef;
            }
        }

        [Then(@"The property change notification is raised via the custom action invocation")]
        public void ThenThePropertyChangeNotificationIsRaisedViaTheCustomActionInvocation()
        {
            var fakeDispatch = _dispatcherScenarioDataStoreBase.Dispatch;
            fakeDispatch.IsCustomActionInvoked.Should().BeTrue();
        }
    }
}
