using Azure.Identity;
using Azure.Storage.Blobs;
using BoDi;
using Microsoft.Extensions.Configuration;
using Ms.Tdd.Adf.Tests.Specs.Models;
using TechTalk.SpecFlow;

namespace Ms.Tdd.Adf.Tests.Specs.Hooks
{
    [Binding]
    public class AzureBloblStorageClientBuilderHook
    {
        [BeforeScenario(Order = 1)]
        public static async Task BuildAzureBlobStorageClient(
            ScenarioContext scenarioContext, 
            IObjectContainer objectContainer, 
            IConfiguration configuration, 
            ADFTestConfiguration adfTestConfiguration)
        {
            if (configuration is null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            AzureBlobStorageConfiguraton? azureBlobStorageConfiguraton = configuration.GetSection("AzureBlobStorage").Get<AzureBlobStorageConfiguraton>();
            var blobServiceClient = new BlobServiceClient(new Uri(azureBlobStorageConfiguraton!.Uri), new AzureCliCredential());
            
            objectContainer.RegisterInstanceAs(azureBlobStorageConfiguraton);
            objectContainer.RegisterInstanceAs(blobServiceClient);
        }
    }
}