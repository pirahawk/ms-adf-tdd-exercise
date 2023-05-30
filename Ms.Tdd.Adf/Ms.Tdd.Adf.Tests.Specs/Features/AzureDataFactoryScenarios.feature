Feature: AzureDataFactoryScenarios
I want to write some Specflow Tests to Test Azure Data Factory

Background:
	Given I want to truncate all MatchScores data before running a test.

Scenario: I am trying to ensure I can invoke an Azure Data Factory Pipeline
	Given I upload a sample data file 'matchScores' to the Azure Blob Storage
	When I invoke the Azure Data Factory Pipeline 'copyBlobToSQLPipeline'
	And I wait for Azure Data Factory Pipeline to complete with pipeline status 'Succeeded' allowing a maximum of '5' retries
	Then the SQL table MatchScores contains the following entries
		| Home         | Away      | ScoreHome | ScoreAway | Result |
		| Bournemouth  | Cardiff   | 2         | 0         | H      |
		| Huddersfield | Chelsea   | 0         | 3         | A      |
		| Newcastle    | Tottenham | 1         | 2         | A      |
		| Watford      | Brighton  | 2         | 0         | H      |