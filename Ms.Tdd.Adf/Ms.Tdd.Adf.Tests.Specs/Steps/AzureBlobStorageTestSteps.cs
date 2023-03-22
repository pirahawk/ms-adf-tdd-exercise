using Azure.Storage.Blobs;
using Ms.Tdd.Adf.Tests.Specs.Models;
using TechTalk.SpecFlow;

namespace Ms.Tdd.Adf.Tests.Specs.Steps
{
    [Binding]
    public class AzureBlobStorageTestSteps
    {
        private readonly ScenarioContext scenarioContext;
        private readonly AzureBlobStorageConfiguraton azureBlobStorageConfiguraton;
        private readonly BlobServiceClient blobServiceClient;

        public AzureBlobStorageTestSteps(ScenarioContext scenarioContext, AzureBlobStorageConfiguraton azureBlobStorageConfiguraton, BlobServiceClient blobServiceClient)
        {
            this.scenarioContext = scenarioContext ?? throw new ArgumentNullException(nameof(scenarioContext));
            this.azureBlobStorageConfiguraton = azureBlobStorageConfiguraton ?? throw new ArgumentNullException(nameof(azureBlobStorageConfiguraton));
            this.blobServiceClient = blobServiceClient ?? throw new ArgumentNullException(nameof(blobServiceClient));
        }

        [Given(@"I upload a sample data file '([^']*)' to the Azure Blob Storage")]
        public void GivenIUploadASampleDataFileToTheAzureBlobStorage(string dataFileName)
        {
            var blobContainerClient = blobServiceClient.GetBlobContainerClient(azureBlobStorageConfiguraton.UploadContainerName);
        }

    }
}