#set parameters 
$TenantId = Read-Host "Tenant Id"
$subscriptionId = Read-Host "Subscription Id"
$location = "uksouth"
$projectname = Read-Host "Project Name - Lower Case No Spaces"
$resource_group = $projectname + "-rg-uks"
$storageAccName = $projectname + "stg001"
$adfName = $projectname + "adf001"
$sqlservername = $projectname + "sqlsvr"
$sqldbname = $projectname + "sqldb001"
$sqladmin = Read-Host "Sql Admin Login"
$sqlAdminPassword = Read-Host "Set Sql Admin Password"
$email = Read-Host "Email Address"
$sid = Read-Host "Admin Object ID"
#SID (object ID) of the server administrator.
$bicepPath = Read-Host "Bicep Script Location"

#login to azure 
az login --tenant $TenantId
az account set --subscription $subscriptionId

Write-Host "creating resource group"
az group create --location $location --name $resource_group --subscription $subscriptionId 

Write-Host "Deploying Resources"
az deployment group create --name ResourceDeployment --resource-group $resource_group --template-file $bicepPath --parameters resourceGroupName="$resource_group" targetLocation="$location" storageAccountName="$storageAccName" adfName="$adfName" sqlServerName="$sqlservername" sqlAdminName="$sqladmin" sqlAdminPassword="$sqlAdminPassword" sqlDbName"$sqldbname" mail="$email" sid="$sid" tenantId="$TenantId"

