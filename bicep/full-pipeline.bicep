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
param userPrincipalId string
param tenantId string

module deployStorageAccount 'modules/storage.bicep' = {
  name: 'deployStorageAccount'
  params:{
    targetLocation: targetLocation
    storageAccountName: storageAccountName
  }
}

module deployAzSqlServer './modules/sqlserver.bicep' = {
  name: 'deployAzSqlServer'
  params: {
    targetLocation: targetLocation
    sqlServerName: sqlServerName
    sqlDbName: sqlDbName
    sqlAdminName: sqlAdminName
    sqlAdminPassword: sqlAdminPassword
    login: mail
    sid: userPrincipalId
    tenantId: tenantId
  }
}

module deployAdf 'modules/data-factory.bicep' = {
  name: 'deployAdf'
  dependsOn: [ deployStorageAccount, deployAzSqlServer ]
  params: {
    targetLocation: targetLocation
    storageAccountName: storageAccountName
    adfName : adfName
    storageInputDatasetContainerName: deployStorageAccount.outputs.adfInputContainerName
    storageOutputDatasetContainerName: deployStorageAccount.outputs.adfOutputContainerName
    sqlServerName: sqlServerName
    sqlDbName: sqlDbName
    sqlAdminName: sqlAdminName
    sqlAdminPassword: sqlAdminPassword
  }
}



module rbacAssign 'modules/rbac-assign.bicep' = {
  name: 'assignRbacRoles'
  dependsOn: [ deployStorageAccount, deployAzSqlServer, deployAdf]
  params:{
    storageAccountName: storageAccountName
    sqlServerName: sqlServerName
    adfName : adfName
    adfPrincipalId: deployAdf.outputs.adfPrincipalId
    userPrincipalId: userPrincipalId
  }
}



output targetResourceGroup string = resourceGroupName
output storageAccountName string = storageAccountName

output mail string = resourceGroupName
output sid string = storageAccountName
output tenantId string = resourceGroupName
