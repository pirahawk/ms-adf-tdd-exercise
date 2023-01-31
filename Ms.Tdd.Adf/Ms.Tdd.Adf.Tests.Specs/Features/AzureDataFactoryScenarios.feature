Feature: AzureDataFactoryScenarios

I want to write some Specflow Tests to Test Azure Data Factory


@RequiresAdf
Scenario: I am trying to ensure I can invoke an Azure Data Factory Pipeline
	Given: I am trying to invoke the Azure Data Factory Pipeline
	When I invoke the Azure Data Factory Pipeline 'copyBlobBetweenContainersPipeline'
