Feature: AzureDataFactoryScenarios
I want to write some Specflow Tests to Test Azure Data Factory


Scenario: I am trying to ensure I can invoke an Azure Data Factory Pipeline
	Given I upload a sample data file 'matchScores' to the Azure Blob Storage
	When I invoke the Azure Data Factory Pipeline 'copyBlobBetweenContainersPipeline'
	And I wait for Azure Data Factory Pipeline to complete with pipeline status 'Succeeded' allowing a maximum of '5' retries
