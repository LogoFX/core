using FluentAssertions;
using TechTalk.SpecFlow;

namespace LogoFX.Client.Core.Specs.Semaphore
{
    [Binding]
    internal sealed class SemaphoreSteps
    {
        private readonly SemaphoreScenarioDataStoreBase _scenarioDataStoreBase;

        public SemaphoreSteps(ScenarioContext scenarioContext)
        {
            _scenarioDataStoreBase = new SemaphoreScenarioDataStoreBase(scenarioContext);
        }

        [When(@"The semaphore is created")]
        public void WhenTheSemaphoreIsCreated()
        {
            var semaphore = new ReentranceGuard();
            _scenarioDataStoreBase.Semaphore = semaphore;
        }

        [When(@"The semaphore is raised")]
        public void WhenTheSemaphoreIsRaised()
        {
            var semaphore = _scenarioDataStoreBase.Semaphore;
            semaphore.Raise();
        }

        [When(@"The semaphore is raised and disposed")]
        public void WhenTheSemaphoreIsRaisedAndDisposed()
        {
            var semaphore = _scenarioDataStoreBase.Semaphore;
            using (semaphore.Raise()) {}
        }

        [Then(@"The semaphore should not be locked")]
        public void ThenTheSemaphoreShouldNotBeLocked()
        {
            var semaphore = _scenarioDataStoreBase.Semaphore;
            semaphore.IsLocked.Should().BeFalse();
        }

        [Then(@"The semaphore should be locked")]
        public void ThenTheSemaphoreShouldBeLocked()
        {
            var semaphore = _scenarioDataStoreBase.Semaphore;
            semaphore.IsLocked.Should().BeTrue();
        }
    }
}
