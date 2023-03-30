using Azure.Core;
using Azure.Identity;
using BoDi;
using Microsoft;
using Microsoft.Azure.Management.DataFactory;
using Microsoft.Extensions.Configuration;
using Microsoft.Rest;
using Ms.Tdd.Adf.Tests.Specs.Models;
using TechTalk.SpecFlow;

namespace Ms.Tdd.Adf.Tests.Specs.Hooks
{
    [Binding]
    public class AzureDataFactoryClientBuilderHook
    {
        [BeforeScenario(Order = 1)]
        public async Task BuildDataFactoryManagementClient(
            ScenarioContext scenarioContext,
            IObjectContainer objectContainer,
            IConfiguration configuration,
            ADFTestConfiguration adfTestConfiguration)
        {
            Assumes.NotNull(configuration);
            Assumes.NotNull(adfTestConfiguration);

            AzureDataFactoryConfiguration? azureDataFactoryConfiguration = configuration.GetSection("AzureDataFactory").Get<AzureDataFactoryConfiguration>();
            DataFactoryManagementClient adfClient = await BuildAdfTestClient(azureDataFactoryConfiguration, adfTestConfiguration).ConfigureAwait(false);

            objectContainer.RegisterInstanceAs(azureDataFactoryConfiguration!);
            objectContainer.RegisterInstanceAs(adfClient);
        }

        private async Task<DataFactoryManagementClient> BuildAdfTestClient(AzureDataFactoryConfiguration? azureDataFactoryConfiguration, ADFTestConfiguration? adfTestConfiguration)
        {
            // TODO: Change Credentials to obey `adfTestConfiguration.UseAzureCliCredentials` flag when ready.
            var azureCliCredentials = new AzureCliCredential();
            var requiredScopes = new[]
            {
                "https://management.azure.com/.default"
            };
            var accessToken = await azureCliCredentials.GetTokenAsync(new TokenRequestContext(requiredScopes, tenantId: azureDataFactoryConfiguration?.TenantId)).ConfigureAwait(false);
            var adfClient = new DataFactoryManagementClient(new TokenCredentials(accessToken.Token))
            {
                SubscriptionId = azureDataFactoryConfiguration?.SubscriptionId,
            };
            return adfClient;
        }
    }
}