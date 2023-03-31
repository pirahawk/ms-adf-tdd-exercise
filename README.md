# ms-adf-tdd-exercise

# Deployment of Resources with Powershell Script

## Pre Deployment Tasks

You will need the following information before running the deployment script:

1. Tenant Id
2. Subscription Id
3. Project Name (Lower Case No Spaces or special characters)
4. Sql Admin Login
5. Set Sql Admin Password
6. Email Address
7. Admin Object ID
8. Bicep Script Path on your computer
9. ADF ARM Path on your computer
10. ADF ARM Parameters Path on your computer
11. Sample Data Path Folder on your computer

## Amend the Azure Data Factory ARM Template Parameters File

1. Navigate to Infrastructure/AdfARM/ARMTemplateParametersForFactory.json
2. Amend factory name parameter replacing <factoryName> with your ADF name - <projectname> + "adf001", eg testprojectadf001
3. Amend adfSqlDb_connectionString parameter replacing <serverName> with your sql server name - <projectname> + "sqlsvr", eg testprojectsqlsvr
4. Amend adfSqlDb_connectionString parameter replacing <databaseName> with your sql db name -  <projectname> + "sqldb001", eg testprojectsqldb001
5. Amend adfblobstorage_properties_typeProperties_serviceEndpoint parameter replacing <storageAccountName> with your storage account name - <projectname> + "stg001", eg testprojectstg001 
6. Save your changes

## Running the Powershell Deployment Script

1. Open Powershell as Administrator
2. Enter command powershell.exe -File <ps1 location> using the location of the Infrastructure/deploymentScript.ps1 powershell script on your computer.
1. Follow the prompts to enter the required parameters for deployment.