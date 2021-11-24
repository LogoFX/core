using System.Collections.Generic;
using System.ComponentModel;
using Attest.Testing.Context.SpecFlow;
using TechTalk.SpecFlow;

namespace LogoFX.Client.Core.Specs.Common
{
    [Binding]
    public sealed class InvocationScenarioDataStoreBase : ScenarioDataStoreBase
    {
        public InvocationScenarioDataStoreBase(ScenarioContext scenarioContext) : base(scenarioContext)
        {
        }

        public INotifyPropertyChanged Class
        {
            get => GetValue<INotifyPropertyChanged>();
            set => SetValue(value);
        }

        public ValueWrapper IsCalledRef
        {
            get => GetValue<ValueWrapper>();
            set => SetValue(value);
        }

        public List<ValueWrapper> IsCalledRefCollection
        {
            get => GetValue<List<ValueWrapper>>();
            set => SetValue(value);
        }
    }
}