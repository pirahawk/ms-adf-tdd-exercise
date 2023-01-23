param targetLocation string
param storageAccountName string
param adfName string
param adfPrincipalId string

resource adf 'Microsoft.DataFactory/factories@2018-06-01' existing = {
  name: adfName
}

resource storageAccount 'Microsoft.Storage/storageAccounts@2022-09-01' existing = {
  name: storageAccountName
}

// var storageContributorRole = '/providers/Microsoft.Authorization/roleDefinitions/b24988ac-6180-42a0-ab88-20f7382dd24c'
resource ContributorRole 'Microsoft.Authorization/roleDefinitions@2022-04-01' existing = {
  name: 'b24988ac-6180-42a0-ab88-20f7382dd24c'
}

resource storageAccountReaderAdf 'Microsoft.Authorization/roleAssignments@2022-04-01' ={
  name: adfPrincipalId
  scope: storageAccount
  properties:{
    principalId: adfPrincipalId
    roleDefinitionId: ContributorRole.id
    principalType: 'ServicePrincipal'
    description: 'Assigning contributor role on ${storageAccount} to ${adf.name}'
  }
}
