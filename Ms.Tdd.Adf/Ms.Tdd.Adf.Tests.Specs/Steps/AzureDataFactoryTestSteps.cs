using FluentAssertions;
using Microsoft.Azure.Management.DataFactory;
using Microsoft.Extensions.Configuration;
using Ms.Tdd.Adf.Tests.Specs.Models;
using Microsoft.Azure.Management.DataFactory.Models;
using TechTalk.SpecFlow;
using Polly;

namespace Ms.Tdd.Adf.Tests.Specs.Steps
{
    [Binding]
    public class AzureDataFactoryTestSteps
    {
        private readonly ScenarioContext scenarioContext;
        private readonly AzureDataFactoryConfiguration azureDataFactoryConfiguration;
        private readonly DataFactoryManagementClient dataFactoryManagementClient;

        public AzureDataFactoryTestSteps(ScenarioContext scenarioContext, AzureDataFactoryConfiguration azureDataFactoryConfiguration, DataFactoryManagementClient dataFactoryManagementClient)
        {
            this.scenarioContext = scenarioContext ?? throw new ArgumentNullException(nameof(scenarioContext));
            this.azureDataFactoryConfiguration = azureDataFactoryConfiguration ?? throw new ArgumentNullException(nameof(azureDataFactoryConfiguration));
            this.dataFactoryManagementClient = dataFactoryManagementClient ?? throw new ArgumentNullException(nameof(dataFactoryManagementClient));
        }

        [When(@"I invoke the Azure Data Factory Pipeline '([^']*)'")]
        public async Task WhenIInvokeTheAzureDataFactoryPipelineCopyBlobBetweenContainersPipeline(string adfPipelineName)
        {
            adfPipelineName.Should().NotBeNullOrWhiteSpace();

            var adfRunResponse = await dataFactoryManagementClient.Pipelines.CreateRunAsync(
                azureDataFactoryConfiguration.ResourceGroupName,
                azureDataFactoryConfiguration.FactoryName,
                adfPipelineName).ConfigureAwait(false);

            adfRunResponse.Should().NotBeNull();
            adfRunResponse.RunId.Should().NotBeNullOrWhiteSpace();
            scenarioContext.Add(ScenarioContextValues.ADF_PIPELINE_RUN_ID_KEY, adfRunResponse);
        }

        [When(@"I wait for Azure Data Factory Pipeline to complete with pipeline status '([^']*)' allowing a maximum of '([^']*)' retries")]
        public async Task WhenIWaitForAzureDataFactoryPipelineToCompleteWithStatusAllowingAMaximumOfRetries(string expectedStatus, int maxRetryCount)
        {
            scenarioContext.TryGetValue<CreateRunResponse>(ScenarioContextValues.ADF_PIPELINE_RUN_ID_KEY, out var adfRunResponse).Should().BeTrue();

            var pipelineRunResponse = await dataFactoryManagementClient.PipelineRuns
                .GetAsync(azureDataFactoryConfiguration.ResourceGroupName, azureDataFactoryConfiguration.FactoryName, adfRunResponse.RunId)
                .ConfigureAwait(false);

            var pipelineRetryPolicy = Policy
            .HandleResult<PipelineRun>(pr => !pr.Status.Equals(expectedStatus))
            .WaitAndRetryAsync(
                maxRetryCount, 
                retryAttempt => TimeSpan.FromSeconds(retryAttempt * 2));

            var response = await pipelineRetryPolicy
                .ExecuteAsync(() => dataFactoryManagementClient.PipelineRuns.GetAsync(azureDataFactoryConfiguration.ResourceGroupName, azureDataFactoryConfiguration.FactoryName, adfRunResponse.RunId));

            response.Status.Should().Be(expectedStatus);
        }
    }
}