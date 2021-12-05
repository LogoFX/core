namespace LogoFX.Client.Core.Specs.Semaphore
{
    [Binding]
    internal sealed class SemaphoreSteps
    {
        private readonly SemaphoreScenarioDataStore _scenarioDataStore;

        public SemaphoreSteps(ScenarioContext scenarioContext)
        {
            _scenarioDataStore = new SemaphoreScenarioDataStore(scenarioContext);
        }

        [When(@"The semaphore is created")]
        public void WhenTheSemaphoreIsCreated()
        {
            var semaphore = new ReentranceGuard();
            _scenarioDataStore.Semaphore = semaphore;
        }

        [When(@"The semaphore is raised")]
        public void WhenTheSemaphoreIsRaised()
        {
            var semaphore = _scenarioDataStore.Semaphore;
            semaphore.Raise();
        }

        [When(@"The semaphore is raised and disposed")]
        public void WhenTheSemaphoreIsRaisedAndDisposed()
        {
            var semaphore = _scenarioDataStore.Semaphore;
            using (semaphore.Raise()) {}
        }

        [Then(@"The semaphore should not be locked")]
        public void ThenTheSemaphoreShouldNotBeLocked()
        {
            var semaphore = _scenarioDataStore.Semaphore;
            semaphore.IsLocked.Should().BeFalse();
        }

        [Then(@"The semaphore should be locked")]
        public void ThenTheSemaphoreShouldBeLocked()
        {
            var semaphore = _scenarioDataStore.Semaphore;
            semaphore.IsLocked.Should().BeTrue();
        }
    }
}
