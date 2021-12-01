using Attest.Testing.Context.SpecFlow;
using TechTalk.SpecFlow;

namespace LogoFX.Client.Core.Specs.Common
{
    [Binding]
    public sealed class DispatcherScenarioDataStore<TDispatch> : ScenarioDataStoreBase
    {
        public DispatcherScenarioDataStore(ScenarioContext scenarioContext) : base(scenarioContext)
        {
        }

        public TDispatch Dispatch
        {
            get => GetValue<TDispatch>();
            set => SetValue(value);
        }
    }
}
