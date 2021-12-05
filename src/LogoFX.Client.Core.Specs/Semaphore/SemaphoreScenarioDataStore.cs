using Attest.Testing.Context.SpecFlow;

namespace LogoFX.Client.Core.Specs.Semaphore
{
    internal sealed class SemaphoreScenarioDataStore : ScenarioDataStoreBase
    {
        public SemaphoreScenarioDataStore(ScenarioContext scenarioContext) : base(scenarioContext)
        {
        }

        public ReentranceGuard Semaphore
        {
            get => GetValue<ReentranceGuard>();
            set => SetValue(value);
        }
    }
}
