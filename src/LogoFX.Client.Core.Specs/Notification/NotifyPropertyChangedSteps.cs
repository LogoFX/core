﻿#nullable enable

using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using LogoFX.Client.Core.Specs.Common;
using LogoFX.Core.Specs.Common;

namespace LogoFX.Client.Core.Specs.Notification
{
    [Binding]
    internal sealed class NotifyPropertyChangedSteps
    {
        private readonly InvocationScenarioDataStore _invocationScenarioDataStore;

        public NotifyPropertyChangedSteps(ScenarioContext scenarioContext)
        {
            _invocationScenarioDataStore = new InvocationScenarioDataStore(scenarioContext);
        }

        [When(@"The number is changed to (.*) in silent mode")]
        public void WhenTheNumberIsChangedToInSilentMode(int value)
        {
            var @class = _invocationScenarioDataStore.Class as TestClassBase;
            @class.UpdateSilent(() =>
            {
                @class.Number = value;
            });
        }

        [When(@"The quantity is changed to (.*) via SetProperty API")]
        public void WhenTheQuantityIsChangedToViaSetPropertyAPI(int value)
        {
            var @class = _invocationScenarioDataStore.Class as TestMultipleClass;
            @class.Quantity = value;
        }

        [When(@"The '(.*)' is created and all notifications are listened to")]
        public void WhenTheIsCreatedAndAllNotificationsAreListenedTo(string name)
        {
            var @class = TestClassFactory.CreateTestClass(Assembly.GetExecutingAssembly(), name);
            if (@class != null)
            {
                _invocationScenarioDataStore.Class = @class;
                var isCallRefCollection = new List<ValueWrapper>();
                var isQuantityCalledRef = TestClassHelper.ListenToPropertyChange(@class, "Quantity");
                isCallRefCollection.Add(isQuantityCalledRef);
                var isTotalCalledRef = TestClassHelper.ListenToPropertyChange(@class, "Total");
                isCallRefCollection.Add(isTotalCalledRef);
                _invocationScenarioDataStore.IsCalledRefCollection = isCallRefCollection;
            }
        }

        [Then(@"The property change notification result is '(.*)' for all notifications")]
        public void ThenThePropertyChangeNotificationResultIsForAllNotifications(string expectedResultStr)
        {
            bool.TryParse(expectedResultStr, out var expectedResult);
            var isCalledRefCollection = _invocationScenarioDataStore.IsCalledRefCollection;
            isCalledRefCollection.Select(t => t.Value).Should().AllBeEquivalentTo(expectedResult);
        }

        [Then(@"The before value update logic is invoked before the value update")]
        public void ThenTheBeforeValueUpdateLogicIsInvokedBeforeTheValueUpdate()
        {
            var @class = _invocationScenarioDataStore.Class as TestBeforeValueUpdateClass;
            @class.PreviousValue.Should().Be(4);
        }

        [Then(@"The after value update logic is invoked after the value update")]
        public void ThenTheAfterValueUpdateLogicIsInvokedAfterTheValueUpdate()
        {
            var @class = _invocationScenarioDataStore.Class as TestAfterValueUpdateClass;
            @class.Number.Should().Be(6);
        }
    }
}
