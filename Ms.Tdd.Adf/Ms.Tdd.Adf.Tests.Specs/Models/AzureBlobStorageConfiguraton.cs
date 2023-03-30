namespace Ms.Tdd.Adf.Tests.Specs.Models
{
    public record AzureBlobStorageConfiguraton
    {
        public string? Uri { get; set; }
        public string? UploadContainerName { get; set; }
    }
}