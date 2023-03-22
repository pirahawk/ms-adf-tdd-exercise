Feature: AzureDataFactoryScenarios
I want to write some Specflow Tests to Test Azure Data Factory

@RequiresAdf @RequiresBlobStorage
Scenario: I am trying to ensure I can invoke an Azure Data Factory Pipeline
	When I invoke the Azure Data Factory Pipeline 'copyBlobBetweenContainersPipeline'
	And I wait for Azure Data Factory Pipeline to complete with pipeline status 'Succeeded' allowing a maximum of '5' retries
