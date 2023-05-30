# To run:  .\bicep\deploy-ptat.ps1

$tenantId = $(az account show --query "tenantId").Trim('"')
$sid =  $(az ad user show --id "b3bd2758-369c-4e4c-8bb6-c2ab93db82ad" --query "id").Trim('"')
$mail =  $(az ad user show --id "b3bd2758-369c-4e4c-8bb6-c2ab93db82ad" --query "mail").Trim('"')


# az deployment group create --resource-group 'test-adf-rg' --name 'ptat-deploy-adf-pipeline' --template-file '.\bicep\full-pipeline.bicep' `
# --parameters `
# resourceGroupName="test-adf-rg" `
# targetLocation="uksouth" `
# storageAccountName="ptatstorage833793522582" `
# adfName="ptatadf0699bd0b094f" `
# sqlServerName="ptatadfsql0ebdcb79f71a" `
# sqlDbName="ptatadfdb" `
# sqlAdminName="catatumboptat" `
# sqlAdminPassword="83953eF52654!" `
# mail=$mail `
# userPrincipalId=$sid `
# tenantId=$tenantId


az deployment group create --resource-group 'adf-show-demo-rg' --name 'ptat-deploy-adf-pipeline' --template-file '.\bicep\full-pipeline.bicep' `
--parameters `
resourceGroupName="adf-show-demo-rg" `
targetLocation="uksouth" `
storageAccountName="ptatstorage833793522592" `
adfName="ptatadf0699bd0b094g" `
sqlServerName="ptatadfsql0ebdcb79f76a" `
sqlDbName="ptatadfdb" `
sqlAdminName="catatumboptat" `
sqlAdminPassword="83953eF52654!" `
mail=$mail `
userPrincipalId=$sid `
tenantId=$tenantId