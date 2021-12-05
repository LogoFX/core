using System.Threading;
using LogoFX.Client.Core.Specs.Common;

namespace LogoFX.Client.Core.Specs.Dispatcher
{
    [Binding]
    internal sealed class DispatcherSteps
    {
        private readonly DispatcherScenarioDataStore<FakeDispatch> _scenarioDataStore;

        public DispatcherSteps(ScenarioContext scenarioContext)
        {
            _scenarioDataStore = new DispatcherScenarioDataStore<FakeDispatch>(scenarioContext);
        }

        [Given(@"The dispatcher is set to test dispatcher")]
        public void GivenTheDispatcherIsSetToTestDispatcher()
        {
            var dispatch = new FakeDispatch();
            _scenarioDataStore.Dispatch = dispatch;
            Dispatch.Current = dispatch;
        }

        [Given(@"The dispatcher is set to overridden dispatcher")]
        public void GivenTheDispatcherIsSetToOverriddenDispatcher()
        {
            var dispatch = new OverriddenDispatch();
            _scenarioDataStore.Dispatch = dispatch;
        }

        [Then(@"The property change notification is raised via the test dispatcher")]
        public void ThenThePropertyChangeNotificationIsRaisedViaTheTestDispatcher()
        {
            var fakeDispatch = _scenarioDataStore.Dispatch;
            fakeDispatch.IsOnUiThreadCalled.Should().BeTrue();
        }

        [Then(@"The property change notification is raised via the overridden dispatcher")]
        public void ThenThePropertyChangeNotificationIsRaisedViaTheOverriddenDispatcher()
        {
            var fakeDispatch = _scenarioDataStore.Dispatch;
            fakeDispatch.IsOnUiThreadCalled.Should().BeTrue();
            fakeDispatch.Should().BeOfType<OverriddenDispatch>();
        }
    }
}
