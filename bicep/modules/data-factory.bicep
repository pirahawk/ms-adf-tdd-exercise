param targetLocation string
param storageAccountName string
param adfName string
param storageInputDatasetContainerName string
param storageOutputDatasetContainerName string

resource storageAccount 'Microsoft.Storage/storageAccounts@2022-09-01' existing = {
  name: storageAccountName
}

// NOTE: Need to register the resource provider `Microsoft.DataFactory` and `Microsoft.EventGrid` on the Subscription or the Triggers will not work correctly

resource azuredatafactory 'Microsoft.DataFactory/factories@2018-06-01' = {
  name: adfName
  location: targetLocation
  identity:{
    type: 'SystemAssigned'
  }

  // Linked Services
  resource azblobstoragelinkedservice 'linkedservices@2018-06-01' = {
    name: 'adfblobstorage'
    properties: {
      type:'AzureBlobStorage'
      typeProperties:{
        connectionString: 'DefaultEndpointsProtocol=https;AccountName=${storageAccount.name};AccountKey=${storageAccount.listKeys().keys[0].value}'
      }
    }
  }

  // DataSets
  resource azBlobInputDataset 'datasets@2018-06-01' = {
    name: 'azBlobInputDataset'
    properties: {
      type:'Binary'
      typeProperties:{
        location: {
          type: 'AzureBlobStorageLocation'
          container: storageInputDatasetContainerName
        }
      }
      linkedServiceName:{
        referenceName: azblobstoragelinkedservice.name
        type: 'LinkedServiceReference'
      }
    }
  }

  resource azBlobOutpuDataset 'datasets@2018-06-01' = {
    name: 'azBlobOutpuDataset'
    properties: {
      type:'Binary'
      typeProperties:{
        location: {
          type: 'AzureBlobStorageLocation'
          container: storageOutputDatasetContainerName
        }
      }
      linkedServiceName:{
        referenceName: azblobstoragelinkedservice.name
        type: 'LinkedServiceReference'
      }
    }
  }


  // Pipelines

  resource copyBlobBetweenContainersPipeline 'pipelines@2018-06-01' = {
    name: 'copyBlobBetweenContainersPipeline'
    properties: {
      activities: [
        {
          name: 'CopyBlobFromInputToOutputContainer'
          type: 'Copy'
          typeProperties: {
            source: {
              type: 'BinarySource'
              storeSettings: {
                type: 'AzureBlobStorageReadSettings'
                recursive: true
              }
            }
            sink: {
              type: 'BinarySink'
              storeSettings: {
                type: 'AzureBlobStorageWriteSettings'
              }
            }
          }
          inputs: [
            {
              referenceName: azBlobInputDataset.name
              type: 'DatasetReference'
            }
          ]

          outputs: [
            {
              referenceName: azBlobOutpuDataset.name
              type: 'DatasetReference'
            }
          ]
        }
      ]
    }
  }

// Triggers
resource copyBlobTrigger 'triggers@2018-06-01' = {
  name: 'copyBlobTrigger'
  properties: {
    type: 'BlobEventsTrigger'
    typeProperties:{
      events:[
        'Microsoft.Storage.BlobCreated'
      ]
      blobPathBeginsWith:'/${storageInputDatasetContainerName}/blobs/'
      ignoreEmptyBlobs:true
      scope: storageAccount.id
    }

    pipelines:[
      {
        parameters: {}
        pipelineReference:{
          type: 'PipelineReference'
          referenceName: copyBlobBetweenContainersPipeline.name
          name: copyBlobBetweenContainersPipeline.name
        }
      }
    ]
  }
}


}


output adfPrincipalId string = azuredatafactory.identity.principalId
