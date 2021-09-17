using Attest.Testing.Context.SpecFlow;
using TechTalk.SpecFlow;

namespace LogoFX.Client.Core.Specs.Semaphore
{
    internal sealed class SemaphoreScenarioDataStoreBase : ScenarioDataStoreBase
    {
        public SemaphoreScenarioDataStoreBase(ScenarioContext scenarioContext) : base(scenarioContext)
        {
        }

        public ReentranceGuard Semaphore
        {
            get => GetValue<ReentranceGuard>();
            set => SetValue(value);
        }
    }
}
