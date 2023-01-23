param resourceGroupName string
param targetLocation string

param storageAccountName string
param adfName string

module deployStorageAccount 'modules/storage.bicep' = {
  name: 'deployStorageAccount'
  params:{
    targetLocation: targetLocation
    storageAccountName: storageAccountName
  }
}

module deployAdf 'modules/data-factory.bicep' = {
  name: 'deployAdf'
  dependsOn: [ deployStorageAccount ]
  params: {
    targetLocation: targetLocation
    storageAccountName: storageAccountName
    adfName : adfName
    storageInputDatasetContainerName: deployStorageAccount.outputs.adfInputContainerName
    storageOutputDatasetContainerName: deployStorageAccount.outputs.adfOutputContainerName
  }
}

module rbacAssign 'modules/rbac-assign.bicep' = {
  name: 'assignRbacRoles'
  dependsOn: [ deployStorageAccount, deployAdf]
  params:{
    targetLocation: targetLocation
    storageAccountName: storageAccountName
    adfName : adfName
    adfPrincipalId: deployAdf.outputs.adfPrincipalId
  }
}

output targetResourceGroup string = resourceGroupName
output storageAccountName string = storageAccountName

