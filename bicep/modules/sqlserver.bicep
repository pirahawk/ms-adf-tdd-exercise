param targetLocation string
param sqlServerName string
param sqlDbName string

param sqlAdminName string
@secure()
param sqlAdminPassword string

param login string
param sid string
param tenantId string


resource sqlServer 'Microsoft.Sql/servers@2022-05-01-preview' = {
  name: sqlServerName
  location: targetLocation
  properties:{
    administratorLogin: sqlAdminName
    administratorLoginPassword: sqlAdminPassword
    administrators:{
      administratorType:'ActiveDirectory'
      principalType:'User'
      login: login
      sid: sid
      tenantId: tenantId
      azureADOnlyAuthentication:false
    }
  }

  resource database 'databases@2022-05-01-preview' = {
    name: sqlDbName
    location: targetLocation
    sku:{
      name: 'Standard'
      tier: 'Standard'
    }
    properties: {
      collation: 'SQL_Latin1_General_CP1_CI_AS'
      catalogCollation: 'SQL_Latin1_General_CP1_CI_AS'
      zoneRedundant: false
      isLedgerOn: false
    }
  }
}
