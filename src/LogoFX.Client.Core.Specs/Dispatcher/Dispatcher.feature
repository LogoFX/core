Feature: Dispatcher
	In order to support multi-threaded UI apps
	As an app developer
	I want the framework to synchronize the UI-bound operations

Background: 
	Given The dispatcher is set to test dispatcher

Scenario Outline: Single property change in regular mode with defined dispatcher should raise notifications via dispatcher
	When The '<Name>' is created
	And The number is changed to 5  in regular mode
	Then The property change notification is raised via the test dispatcher

Examples:
| Name                  |
| TestNameClass         |
| TestPropertyInfoClass |
| TestExpressionClass   |

Scenario: Changing single property with defined dispatcher should raise notifications via dispatcher	
	When The 'TestRegularClass' is created
	And The number is changed to 5 via SetProperty API
	Then The property change notification is raised via the test dispatcher

Scenario: Invoking all properties change with defined dispatcher should raise notification via dispatcher	
	When The 'TestNameClass' is created and empty notification is listened to
	And The all properties change is invoked
	Then The property change notification is raised via the test dispatcher

Scenario Outline: Single property change in regular mode with overridden dispatcher should raise notifications via the overridden dispatcher
	Given The dispatcher is set to overridden dispatcher
	When The '<Name>' is created with dispatcher
	And The number is changed to 5  in regular mode
	Then The property change notification is raised via the overridden dispatcher

Examples:
| Name                            |
| TestOverridenNameClass          |
| TestOverridenPropertyInfoClass |
| TestOverriddenExpressionClass   |

Scenario: Changing single property with overridden dispatcher should raise notifications via the overridden dispatcher
	Given The dispatcher is set to overridden dispatcher
	When The 'TestOverriddenDispatcherClass' is created with dispatcher
	And The number is changed to 5 via SetProperty API
	Then The property change notification is raised via the overridden dispatcher