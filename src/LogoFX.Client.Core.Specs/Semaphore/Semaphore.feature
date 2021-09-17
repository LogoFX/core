Feature: Semaphore
	In order to be able to develop concurrent apps
	As an app developer
	I want the framework to provide semaphore functionality

Scenario: Accessing semaphore after is has been raised once should result in not locked state
	When The semaphore is created
	And The semaphore is raised
	Then The semaphore should not be locked

Scenario: Accessing semaphore after is has been raised twice should result in locked state
	When The semaphore is created
	And The semaphore is raised
	And The semaphore is raised
	Then The semaphore should be locked

Scenario: Accessing semaphore after is has been raised three times should result in locked state
	When The semaphore is created
	And The semaphore is raised
	And The semaphore is raised
	And The semaphore is raised
	Then The semaphore should be locked

Scenario: Accessing semaphore after is has been raised twice and disposed should result in not locked state
	When The semaphore is created
	And The semaphore is raised
	And The semaphore is raised and disposed
	Then The semaphore should not be locked

Scenario: Accessing semaphore after is has been raised three times and disposed should result in locked state
	When The semaphore is created
	And The semaphore is raised
	And The semaphore is raised
	And The semaphore is raised and disposed
	Then The semaphore should be locked
