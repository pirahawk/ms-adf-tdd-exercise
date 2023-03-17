param targetLocation string
param storageAccountName string

param adfInputContainerName string = 'adfinput'
param adfOutputContainerName string = 'adfoutput'

resource storageAccount 'Microsoft.Storage/storageAccounts@2022-09-01' = {
  name: storageAccountName
  location: targetLocation
  sku: {
    name: 'Standard_LRS'
  }
  kind: 'StorageV2'

  resource containerBlobService 'blobServices@2022-09-01' = {
    name: 'default'

    resource adfInputContainer 'containers@2022-09-01' = {
      name: adfInputContainerName
    }

    resource adfOutputContainer 'containers@2022-09-01' = {
      name: adfOutputContainerName
    }
  }
}

output adfInputContainerName string = adfInputContainerName
output adfOutputContainerName string = adfOutputContainerName
