using System;
using System.Linq;
using FluentAssertions;
using TechTalk.SpecFlow;

namespace LogoFX.Practices.IoC.Specs
{
    [Binding]
    internal sealed class ExtendedSimpleContainerSteps
    {
        private readonly ContainerScenarioDataStore _scenarioDataStore;

        public ExtendedSimpleContainerSteps(ScenarioContext scenarioContext)
        {
            _scenarioDataStore = new ContainerScenarioDataStore(scenarioContext);
        }

        [When(@"The container is created")]
        public void WhenTheContainerIsCreated()
        {
            var container = new ExtendedSimpleContainer();
            _scenarioDataStore.Container = container;
        }

        [When(@"The dependency with named parameter is registered in transient fashion")]
        public void WhenTheDependencyWithNamedParameterIsRegisteredInTransientFashion()
        {
            var container = _scenarioDataStore.Container;
            container.RegisterPerRequest(typeof(ITestNamedParameterDependency), null,
                typeof(TestNamedParameterDependency));
        }

        [When(@"The dependency with typed parameter is registered in transient fashion")]
        public void WhenTheDependencyWithTypedParameterIsRegisteredInTransientFashion()
        {
            var container = _scenarioDataStore.Container;
            container.RegisterPerRequest(typeof(ITestTypedParameterDependency), null,
                typeof(TestTypedParameterDependency));
        }

        [When(@"The dependency with named parameter is registered in singleton fashion")]
        public void WhenTheDependencyWithNamedParameterIsRegisteredInSingletonFashion()
        {
            var container = _scenarioDataStore.Container;
            container.RegisterSingleton<ITestModule>((c, r) => new TestModule {Name = "1"});
        }

        [When(@"The same type dependency is registered in instance fashion using first instance")]
        public void WhenTheSameTypeDependencyIsRegisteredInInstanceFashionUsingFirstInstance()
        {
            RegisterInstanceDependency((r, v) => r.FirstModule = v, new TestModule {Name = "1"});
        }

        [When(@"The same type dependency is registered in instance fashion using second instance")]
        public void WhenTheSameTypeDependencyIsRegisteredInInstanceFashionUsingSecondInstance()
        {
            RegisterInstanceDependency((r, v) => r.SecondModule = v, new TestModule { Name = "2" });
        }

        private void RegisterInstanceDependency(Action<ContainerScenarioDataStore, ITestModule> valueSetter, ITestModule instance)
        {
            var container = _scenarioDataStore.Container;
            var module = new TestModule { Name = "1" };
            container.RegisterHandler(typeof(ITestModule), null, (c, r) => instance);
            valueSetter(_scenarioDataStore, instance);
        }

        [When(@"The dependency is resolved with value '(.*)' for named parameter")]
        public void WhenTheDependencyIsResolvedWithValueForNamedParameter(string parameter)
        {
            var container = _scenarioDataStore.Container;
            var dependency = container.GetInstance(typeof(ITestNamedParameterDependency), null,
                new IParameter[] { new NamedParameter("model", parameter) }) as ITestNamedParameterDependency;
            _scenarioDataStore.Dependency = dependency;
        }

        [When(@"The dependency is resolved with value (.*) for typed parameter")]
        public void WhenTheDependencyIsResolvedWithValueForTypedParameter(int parameter)
        {
            var container = _scenarioDataStore.Container;
            const int val = 6;
            var dependency = container.GetInstance(typeof(ITestTypedParameterDependency), null,
                new IParameter[] { new TypedParameter(typeof(int), val) }) as ITestTypedParameterDependency;
            _scenarioDataStore.Dependency = dependency;
        }

        [When(@"The collection of dependencies is resolved")]
        public void WhenTheCollectionOfDependenciesIsResolved()
        {
            var container = _scenarioDataStore.Container;
            var dependencies = container.GetAllInstances(typeof(ITestModule)).ToArray();
            _scenarioDataStore.Dependencies = dependencies;
        }

        [Then(@"Actual value of parameter inside the named dependency is '(.*)'")]
        public void ThenActualValueOfParameterInsideTheNamedDependencyIs(string parameter)
        {
            var dependency = _scenarioDataStore.Dependency as ITestNamedParameterDependency;
            var actualModel = dependency.Model;
            actualModel.Should().Be(parameter);
        }

        [Then(@"Actual value of parameter inside the typed dependency is (.*)")]
        public void ThenActualValueOfParameterInsideTheTypedDependencyIs(int parameter)
        {
            var dependency = _scenarioDataStore.Dependency as ITestTypedParameterDependency;
            var actualValue = dependency.Value;
            actualValue.Should().Be(parameter);
        }

        [Then(@"The collection of dependencies is equivalent to the collection of instances")]
        public void ThenTheCollectionOfDependenciesIsEquivalentToTheCollectionOfInstances()
        {
            var dependencies = _scenarioDataStore.Dependencies.OfType<ITestModule>().ToArray();
            var modules = new[]
                {_scenarioDataStore.FirstModule, _scenarioDataStore.SecondModule};
            dependencies.Should().BeSubsetOf(modules);
            modules.Should().BeSubsetOf(dependencies);
        }

        [Then(@"Multiple dependency resolutions yield same value")]
        public void ThenMultipleDependencyResolutionsYieldSameValue()
        {
            var container = _scenarioDataStore.Container;
            var module = container.GetInstance(typeof(ITestModule), null);
            var actualModule = container.GetInstance(typeof(ITestModule), null);

            actualModule.Should().BeSameAs(module);
        }
    }
}
