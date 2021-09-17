using System.Threading;
using FluentAssertions;
using LogoFX.Client.Core.Specs.Common;
using TechTalk.SpecFlow;

namespace LogoFX.Client.Core.Specs.Dispatcher
{
    [Binding]
    internal sealed class DispatcherSteps
    {
        private readonly DispatcherScenarioDataStoreBase<FakeDispatch> _scenarioDataStoreBase;

        public DispatcherSteps(ScenarioContext scenarioContext)
        {
            _scenarioDataStoreBase = new DispatcherScenarioDataStoreBase<FakeDispatch>(scenarioContext);
        }

        [Given(@"The dispatcher is set to test dispatcher")]
        public void GivenTheDispatcherIsSetToTestDispatcher()
        {
            var dispatch = new FakeDispatch();
            _scenarioDataStoreBase.Dispatch = dispatch;
            Dispatch.Current = dispatch;
        }

        [Given(@"The dispatcher is set to overridden dispatcher")]
        public void GivenTheDispatcherIsSetToOverriddenDispatcher()
        {
            var dispatch = new OverriddenDispatch();
            _scenarioDataStoreBase.Dispatch = dispatch;
        }

        [Then(@"The property change notification is raised via the test dispatcher")]
        public void ThenThePropertyChangeNotificationIsRaisedViaTheTestDispatcher()
        {
            var fakeDispatch = _scenarioDataStoreBase.Dispatch;
            fakeDispatch.IsOnUiThreadCalled.Should().BeTrue();
        }

        [Then(@"The property change notification is raised via the overridden dispatcher")]
        public void ThenThePropertyChangeNotificationIsRaisedViaTheOverriddenDispatcher()
        {
            var fakeDispatch = _scenarioDataStoreBase.Dispatch;
            fakeDispatch.IsOnUiThreadCalled.Should().BeTrue();
            fakeDispatch.Should().BeOfType<OverriddenDispatch>();
        }
    }
}
