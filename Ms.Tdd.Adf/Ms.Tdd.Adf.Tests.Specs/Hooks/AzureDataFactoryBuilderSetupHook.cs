using Azure.Core;
using Azure.Identity;
using BoDi;
using Microsoft.Azure.Management.DataFactory;
using Microsoft.Extensions.Configuration;
using Microsoft.Rest;
using Ms.Tdd.Adf.Tests.Specs.Models;
using TechTalk.SpecFlow;

namespace Ms.Tdd.Adf.Tests.Specs.Hooks
{
    [Binding]
    public class AzureDataFactoryBuilderSetupHook
    {
        private readonly string[] requiredScopes = new[]
        {
                "https://management.azure.com/.default"
        };

        [BeforeScenario("RequiresAdf", Order = 1)]
        public async Task LoadConfigurationBeforeScenario(ScenarioContext scenarioContext, IObjectContainer objectContainer, IConfiguration configuration)
        {
            if (configuration is null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            AzureDataFactoryConfiguration? azureDataFactoryConfiguration = configuration.GetSection("AzureDataFactory").Get<AzureDataFactoryConfiguration>();
            objectContainer.RegisterInstanceAs(azureDataFactoryConfiguration!);

            ADFTestConfiguration? adfTestConfiguration = configuration.GetSection("").Get<ADFTestConfiguration>();
            DataFactoryManagementClient adfClient = await BuildAdfTestClient(azureDataFactoryConfiguration, adfTestConfiguration).ConfigureAwait(false);

            scenarioContext.Add(ScenarioContextValues.ADF_CLIENT, adfClient);
        }

        private async Task<DataFactoryManagementClient> BuildAdfTestClient(AzureDataFactoryConfiguration? azureDataFactoryConfiguration, ADFTestConfiguration? adfTestConfiguration)
        {
            // TODO: Change Credentials to obey `adfTestConfiguration.UseAzureCliCredentials` flag when ready.
            var azureCliCredentials = new AzureCliCredential();
            var accessToken = await azureCliCredentials.GetTokenAsync(new TokenRequestContext(requiredScopes, tenantId: azureDataFactoryConfiguration?.TenantId)).ConfigureAwait(false);
            var adfClient = new DataFactoryManagementClient(new TokenCredentials(accessToken.Token))
            {
                SubscriptionId = azureDataFactoryConfiguration?.SubscriptionId,
            };
            return adfClient;
        }
    }
}