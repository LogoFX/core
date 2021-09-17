Feature: Invocation
	In order to support different app use cases
	As an app developer
	I want the framework to support platform-specific action invocation

Scenario: Changing single property with custom action invocation should invoke actions via custom route
	Given The dispatcher is set to custom dispatcher
	When The 'TestCustomActionInvocationClass' is created here with dispatcher
	And The number is changed to 5 via SetProperty API
	Then The property change notification is raised via the custom action invocation
