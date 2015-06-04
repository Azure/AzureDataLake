# First steps: Azure PowerShell with Data Lake

This guide assumes you have previously followed the steps in the main [Getting Started guide](../GettingStarted.md).

------------

### First Steps

#### Pick which Subscription you want to use

1. Open a new PowerShell window.
1. Select your subscription by entering the following:
        Select-AzureSubscription -SubscriptionId $subscriptionID


#### Getting a Resource Group

All Azure resources belong a Resource Group

To see the resource groups in your subscription:
    
    Get-AzureResourceGroup
    
If you need to create a new Resource Group

    New-AzureResourceGroup -Name $resourceGroupName -Location "East US 2"
    
    NOTE: For now -Location MUST be set to "East US 2"

#### Creating a new Azure Data Lake account

    New-AzureDataLakeAccount -ResourceGroupName $resourceGroupName -Name $dataLakeAccountName -Location "East US 2"


#### Learn more

* [PowerShell Tutorials](Tutorials.md) - Learn how to perform some basic activities with your Azure Data Lake in PowerShell.
* [PowerShell User Manual](UserManual.md) - See how to use the Azure Data Lake PowerShell cmdlets.
    
------------

### Useful links

Browse the following pages:

* *(place table of contents here)*
