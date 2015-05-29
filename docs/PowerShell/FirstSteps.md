# First steps: Azure PowerShell with Data Lake

This guide assumes you have previously followed the steps in the main [Getting Started guide](../GettingStarted.md).

------------

### Getting started

#### Initialization
1. Open a new PowerShell window.
1. Select your subscription by entering the following:
        Select-AzureSubscription -SubscriptionId <your_subscription_id>


#### Getting a resource group
To create an resource in Azure, you must select a resource group.

To enumerate the resource groups in your subscription:
    
    Get-AzureResourceGroup
    
To create a new resource group:

    New-AzureResourceGroup -Name <resource_group_name> -Location "East US 2"
    

#### Creating a new Azure Data Lake account

    New-AzureDataLakeAccount -ResourceGroupName <resource_group_name> -Name <datalake_account_name> -Location "East US 2"


#### Learn more

* [PowerShell Tutorials](Tutorials.md) - Learn how to perform some basic activities with your Azure Data Lake in PowerShell.
* [PowerShell User Manual](UserManual.md) - See how to use the Azure Data Lake PowerShell cmdlets.
    
------------

### Useful links

Browse the following pages:

* *(place table of contents here)*