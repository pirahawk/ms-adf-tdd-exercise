param storageAccountName string
param adfName string
param adfPrincipalId string
param sqlServerName string

resource adf 'Microsoft.DataFactory/factories@2018-06-01' existing = {
  name: adfName
}

resource storageAccount 'Microsoft.Storage/storageAccounts@2022-09-01' existing = {
  name: storageAccountName
}

resource sqlServer 'Microsoft.Sql/servers@2022-05-01-preview' existing = {
  name: sqlServerName
}

// var storageContributorRole = '/providers/Microsoft.Authorization/roleDefinitions/b24988ac-6180-42a0-ab88-20f7382dd24c'
resource ContributorRole 'Microsoft.Authorization/roleDefinitions@2022-04-01' existing = {
  name: 'b24988ac-6180-42a0-ab88-20f7382dd24c'
}

resource SqlDbContributorRole 'Microsoft.Authorization/roleDefinitions@2022-04-01' existing = {
  name: '9b7fa17d-e63e-47b0-bb0a-15c516ac86ec'
}

resource storageAccountReaderAdf 'Microsoft.Authorization/roleAssignments@2022-04-01' ={
  // name: adfPrincipalId
  name: guid(storageAccount.id, adfPrincipalId, ContributorRole.name)
  scope: storageAccount
  properties:{
    principalId: adfPrincipalId
    roleDefinitionId: ContributorRole.id
    principalType: 'ServicePrincipal'
    description: 'Assigning contributor role on ${storageAccount} to ${adf.name}'
  }
}

resource sqlServerContributorAdf 'Microsoft.Authorization/roleAssignments@2022-04-01' ={
  // name: adfPrincipalId
  name: guid(sqlServer.id, adfPrincipalId, SqlDbContributorRole.name)
  scope: sqlServer
  properties:{
    principalId: adfPrincipalId
    roleDefinitionId: SqlDbContributorRole.id
    principalType: 'ServicePrincipal'
    description: 'Assigning contributor role on ${sqlServer} to ${adf.name}'
  }
}

output storageAccountReaderAdfName string = storageAccountReaderAdf.name
output sqlServerContributorAdfName string = sqlServerContributorAdf.name
