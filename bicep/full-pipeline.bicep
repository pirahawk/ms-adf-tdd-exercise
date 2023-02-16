param resourceGroupName string
param targetLocation string

param storageAccountName string
param adfName string

param sqlServerName string
param sqlAdminName string
@secure()
param sqlAdminPassword string
param sqlDbName string

param mail string
param sid string
param tenantId string

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

module deployAzSqlServer './modules/sqlserver.bicep' = {
  name: 'deployAzSqlServer'
  params: {
    targetLocation: targetLocation
    sqlServerName: sqlServerName
    sqlAdminName: sqlAdminName
    sqlAdminPassword: sqlAdminPassword
    sqlDbName: sqlDbName
    login: mail
    sid: sid
    tenantId: tenantId
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

output mail string = resourceGroupName
output sid string = storageAccountName
output tenantId string = resourceGroupName
