﻿Feature: Notification
	In order to implement different app scenarios
	As an app developer
	I want the framework to handle property notifications properly

Scenario Outline: Single property change in regular mode should raise property change notification
	When The '<Name>' is created
	And The number is changed to 5 in regular mode
	Then The property change notification result is '<Result>'

Examples:
| Name             | Result |
| TestRegularClass | true   |

Scenario Outline: Single property change in silent mode should not raise property change notification
	When The '<Name>' is created
	And The number is changed to 5 in silent mode
	Then The property change notification result is '<Result>'

Examples:
| Name                  | Result |
| TestNameClass         | false  |
| TestExpressionClass   | false  |

Scenario: Changing single property via SetProperty API should raise property change notification
	When The 'TestRegularClass' is created
	And The number is changed to 5 via SetProperty API
	Then The property change notification result is 'true'

Scenario: Changing single property with before value update logic via SetProperty API should invoke this logic before property value changes
	When The 'TestBeforeValueUpdateClass' is created
	And The number is changed to 5 via SetProperty API
	Then The before value update logic is invoked before the value update

Scenario: Changing single property with after value update logic via SetProperty API should invoke this logic after property value changes
	When The 'TestAfterValueUpdateClass' is created
	And The number is changed to 5 via SetProperty API
	Then The after value update logic is invoked after the value update


