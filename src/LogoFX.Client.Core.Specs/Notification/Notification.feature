Feature: Notification
	In order to have reactive user interface
	As an app developer
	I want the framework to handle property notifications properly

Scenario Outline: Single property change in regular mode should raise property change notification
	When The '<Name>' is created
	And The number is changed to 5  in regular mode
	Then The property change notification result is '<Result>'

Examples:
| Name                  | Result |
| TestNameClass         | true   |
| TestPropertyInfoClass | true   |
| TestExpressionClass   | true   |

Scenario Outline: Single property change in silent mode should not raise property change notification
	When The '<Name>' is created
	And The number is changed to 5 in silent mode
	Then The property change notification result is '<Result>'

Examples:
| Name                  | Result |
| TestNameClass         | false  |
| TestPropertyInfoClass | false  |
| TestExpressionClass   | false  |

Scenario: Invoking all properties change should raise empty property change notification
	When The 'TestNameClass' is created and empty notification is listened to
	And The all properties change is invoked
	Then The property change notification result is 'true'

Scenario: Changing single property via SetProperty API should raise property change notification
	When The 'TestRegularClass' is created
	And The number is changed to 5 via SetProperty API
	Then The property change notification result is 'true'

Scenario: Changing single property with multiple notifications via SetProperty API should raise all property change notifications
	When The 'TestMultipleClass' is created and all notifications are listened to
	And The quantity is changed to 5 via SetProperty API
	Then The property change notification result is 'true' for all notifications

Scenario: Changing single property with before value update logic via SetProperty API should invoke this logic before property value changes
	When The 'TestBeforeValueUpdateClass' is created
	And The number is changed to 5 via SetProperty API
	Then The before value update logic is invoked before the value update

Scenario: Changing single property with after value update logic via SetProperty API should invoke this logic after property value changes
	When The 'TestAfterValueUpdateClass' is created
	And The number is changed to 5 via SetProperty API
	Then The after value update logic is invoked after the value update

