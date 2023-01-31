using FluentAssertions;
using Microsoft.Azure.Management.DataFactory;
using Microsoft.Extensions.Configuration;
using Ms.Tdd.Adf.Tests.Specs.Models;
using TechTalk.SpecFlow;
using Xunit;

namespace Ms.Tdd.Adf.Tests.Specs.Steps
{
    [Binding]
    public class TestSteps
    {
        private readonly ScenarioContext scenarioContext;
        private readonly IConfiguration configuration;
        private readonly AzureDataFactoryConfiguration azureDataFactoryConfiguration;

        public TestSteps(ScenarioContext scenarioContext, IConfiguration configuration, AzureDataFactoryConfiguration azureDataFactoryConfiguration)
        {
            this.scenarioContext = scenarioContext ?? throw new ArgumentNullException(nameof(scenarioContext));
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this.azureDataFactoryConfiguration = azureDataFactoryConfiguration ?? throw new ArgumentNullException(nameof(azureDataFactoryConfiguration));
        }

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
        }

        private DataFactoryManagementClient? DataFactoryManagementClient => this.scenarioContext.Get<DataFactoryManagementClient>(ScenarioContextValues.ADF_CLIENT);
    }
}