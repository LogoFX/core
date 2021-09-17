#nullable enable

using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using LogoFX.Client.Core.Specs.Common;
using TechTalk.SpecFlow;

namespace LogoFX.Client.Core.Specs.Notification
{
    [Binding]
    internal sealed class NotifyPropertyChangedSteps
    {
        private readonly InvocationScenarioDataStoreBase _invocationScenarioDataStoreBase;

        public NotifyPropertyChangedSteps(ScenarioContext scenarioContext)
        {
            _invocationScenarioDataStoreBase = new InvocationScenarioDataStoreBase(scenarioContext);
        }

        [When(@"The number is changed to (.*) in silent mode")]
        public void WhenTheNumberIsChangedToInSilentMode(int value)
        {
            var @class = _invocationScenarioDataStoreBase.Class as TestClassBase;
            @class.UpdateSilent(() =>
            {
                @class.Number = value;
            });
        }

        [When(@"The quantity is changed to (.*) via SetProperty API")]
        public void WhenTheQuantityIsChangedToViaSetPropertyAPI(int value)
        {
            var @class = _invocationScenarioDataStoreBase.Class as TestMultipleClass;
            @class.Quantity = value;
        }

        [When(@"The '(.*)' is created and all notifications are listened to")]
        public void WhenTheIsCreatedAndAllNotificationsAreListenedTo(string name)
        {
            var @class = TestClassFactory.CreateTestClass(name);
            if (@class != null)
            {
                _invocationScenarioDataStoreBase.Class = @class;
                var isCallRefCollection = new List<ValueWrapper>();
                var isQuantityCalledRef = TestClassHelper.ListenToPropertyChange(@class, "Quantity");
                isCallRefCollection.Add(isQuantityCalledRef);
                var isTotalCalledRef = TestClassHelper.ListenToPropertyChange(@class, "Total");
                isCallRefCollection.Add(isTotalCalledRef);
                _invocationScenarioDataStoreBase.IsCalledRefCollection = isCallRefCollection;
            }
        }

        [Then(@"The property change notification result is '(.*)'")]
        public void ThenThePropertyChangeNotificationResultIs(string expectedResultStr)
        {
            bool.TryParse(expectedResultStr, out var expectedResult);
            var isCalledRef = _invocationScenarioDataStoreBase.IsCalledRef;
            isCalledRef.Value.Should().Be(expectedResult);
        }

        [Then(@"The property change notification result is '(.*)' for all notifications")]
        public void ThenThePropertyChangeNotificationResultIsForAllNotifications(string expectedResultStr)
        {
            bool.TryParse(expectedResultStr, out var expectedResult);
            var isCalledRefCollection = _invocationScenarioDataStoreBase.IsCalledRefCollection;
            isCalledRefCollection.Select(t => t.Value).Should().AllBeEquivalentTo(expectedResult);
        }

        [Then(@"The before value update logic is invoked before the value update")]
        public void ThenTheBeforeValueUpdateLogicIsInvokedBeforeTheValueUpdate()
        {
            var @class = _invocationScenarioDataStoreBase.Class as TestBeforeValueUpdateClass;
            @class.PreviousValue.Should().Be(4);
        }

        [Then(@"The after value update logic is invoked after the value update")]
        public void ThenTheAfterValueUpdateLogicIsInvokedAfterTheValueUpdate()
        {
            var @class = _invocationScenarioDataStoreBase.Class as TestAfterValueUpdateClass;
            @class.Number.Should().Be(6);
        }
    }
}
