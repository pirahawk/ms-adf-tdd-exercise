using FluentAssertions;
using Microsoft.Azure.Management.DataFactory;
using Microsoft.Extensions.Configuration;
using Ms.Tdd.Adf.Tests.Specs.Models;
using Microsoft.Azure.Management.DataFactory.Models;
using TechTalk.SpecFlow;
using Xunit;
using System.Collections.Generic;
using Polly;
using System.Threading;
using System.Diagnostics.CodeAnalysis;

namespace Ms.Tdd.Adf.Tests.Specs.Steps
{
    [Binding]
    public class AzureDataFactoryTestSteps
    {
        private readonly ScenarioContext scenarioContext;
        private readonly IConfiguration configuration;
        private readonly AzureDataFactoryConfiguration azureDataFactoryConfiguration;

        public AzureDataFactoryTestSteps(ScenarioContext scenarioContext, IConfiguration configuration, AzureDataFactoryConfiguration azureDataFactoryConfiguration)
        {
            this.scenarioContext = scenarioContext ?? throw new ArgumentNullException(nameof(scenarioContext));
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this.azureDataFactoryConfiguration = azureDataFactoryConfiguration ?? throw new ArgumentNullException(nameof(azureDataFactoryConfiguration));
        }

        private DataFactoryManagementClient? DataFactoryManagementClient => this.scenarioContext.Get<DataFactoryManagementClient>(ScenarioContextValues.ADF_CLIENT);

        [When(@"I invoke the Azure Data Factory Pipeline '([^']*)'")]
        public async Task WhenIInvokeTheAzureDataFactoryPipelineCopyBlobBetweenContainersPipeline(string adfPipelineName)
        {
            adfPipelineName.Should().NotBeNullOrWhiteSpace();

            var adfClient = DataFactoryManagementClient;

            if (adfClient == null)
            {
                Assert.Fail("Could not create DataFactoryManagementClient");
            }

            var adfRunResponse = await adfClient!.Pipelines.CreateRunAsync(
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

            var pipelineRunResponse = await DataFactoryManagementClient!.PipelineRuns
                .GetAsync(azureDataFactoryConfiguration.ResourceGroupName, azureDataFactoryConfiguration.FactoryName, adfRunResponse.RunId)
                .ConfigureAwait(false);

            var pipelineRetryPolicy = Policy
            .HandleResult<PipelineRun>(pr => !pr.Status.Equals(expectedStatus))
            .WaitAndRetryAsync(
                maxRetryCount, 
                retryAttempt => TimeSpan.FromSeconds(retryAttempt * 2));

            var response = await pipelineRetryPolicy
                .ExecuteAsync(() => DataFactoryManagementClient!.PipelineRuns.GetAsync(azureDataFactoryConfiguration.ResourceGroupName, azureDataFactoryConfiguration.FactoryName, adfRunResponse.RunId));

            response.Status.Should().Be(expectedStatus);
        }
    }
}