# Getting Started: Azure PowerShell with Kona and Data Lake

### Links
* Microsoft-Internal SharePoint: http://aka.ms/Kona
* External Private Preview SharePoint: http://aka.ms/ProjectKona

### Prerequisite steps

This guide assumes you have previously followed the steps in the main Getting Started [LINK] guide.

### Getting started

#### User manual

If you would rather see a more detailed user manual for the PowerShell cmdlets, [click here](UserManual.md).


#### Initialization
1. Open a new PowerShell window.
1. Select your subscription by entering the following:
        Select-AzureSubscription -SubscriptionId <your_subscription_id>


#### Resource group
To create an resource in Azure, you must select a resource group.

To enumerate the resource groups in your subscription:
    
    Get-AzureResourceGroup
    
To create a new resource group:

    New-AzureResourceGroup -Name <resource_group_name> -Location "East US 2"
    
#### New Data Lake account

    New-AzureDataLakeAccount -ResourceGroupName <resource_group_name> -Name <datalake_account_name> -Location "East US 2"

   
#### New Kona account

    New-AzureKonaAccount -ResourceGroupName <resource_group_name> -Name <kona_account_name> -Location "East US 2"

    
#### Run your first job:

    Submit-AzureKonaJob -AccountName <kona_account_name> -Sqlip -Name "TestJob" -Script "DROP TABLE IF EXISTS MyNewTable_ABCDEFG12345;"

    
#### Upload a file to your Data Lake:

    Copy-AzureDataLakeItem -Path "C:\<parent_folder_path>\testFile.txt" -Destination "swebhdfs://<kona_account_name>.azuredatalake.com/testFile.txt"
    
    
### Learn more

To learn more about how to interact with Kona and Data Lake, browse the following pages:

#### Azure Portal
* [Getting Started](../GettingStarted.md)

#### SQLIP Studio
* [Getting Started](../SQLIPStudio/GettingStarted.md)
* [Job Authoring](../SQLIPStudio/JobAuthoring.md)

#### PowerShell
* [Getting Started](../GettingStarted.md)
* [User Manual - Kona](../UserManual.md#kona-powershell-cmdlets)
* [User Manual - Data Lake](../UserManual.md#data-lake-powershell-cmdlets)

#### SDK
* [Getting Started](../SDK/GettingStarted.md)
* [User Manual - Kona](../SDK/UserManual.md#kona-sdk)
* [User Manual - Data Lake](../SDK/UserManual.md#data-lake-sdk)