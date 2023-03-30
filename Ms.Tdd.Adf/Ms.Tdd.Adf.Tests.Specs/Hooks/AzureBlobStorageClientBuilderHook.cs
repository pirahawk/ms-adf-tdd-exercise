using Azure.Identity;
using Azure.Storage.Blobs;
using BoDi;
using Microsoft;
using Microsoft.Extensions.Configuration;
using Ms.Tdd.Adf.Tests.Specs.Models;
using TechTalk.SpecFlow;

namespace Ms.Tdd.Adf.Tests.Specs.Hooks
{
    [Binding]
    public class AzureBlobStorageClientBuilderHook
    {
        [BeforeScenario(Order = 1)]
        public static void BuildAzureBlobStorageClient(
            ScenarioContext scenarioContext, 
            IObjectContainer objectContainer, 
            IConfiguration configuration)
        {
            Assumes.NotNull(configuration);

            AzureBlobStorageConfiguraton? azureBlobStorageConfiguraton = configuration.GetSection("AzureBlobStorage").Get<AzureBlobStorageConfiguraton>();
            Assumes.NotNull(azureBlobStorageConfiguraton);
            Assumes.NotNullOrEmpty(azureBlobStorageConfiguraton!.Uri);
            var blobServiceClient = new BlobServiceClient(new Uri(azureBlobStorageConfiguraton.Uri), new AzureCliCredential());
            
            objectContainer.RegisterInstanceAs(azureBlobStorageConfiguraton);
            objectContainer.RegisterInstanceAs(blobServiceClient);
        }
    }
}