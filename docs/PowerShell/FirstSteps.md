# First steps: Azure PowerShell with Data Lake

**NOTE:** This guide assumes you have previously followed the steps in the main [Getting Started guide](../GettingStarted.md).

------------

### First Steps
#### Installation
If you already followed the [Getting Started guide](../GettingStarted.md), you can go to the Initialization section.

Install Azure PowerShell with Data Lake:

1. Download the [Azure PowerShell module here](https://github.com/MicrosoftBigData/AzureDataLake/releases).
1. Run AzurePowerShell.msi
1. Follow the steps in the installation wizard.
1. Enter in PowerShell: ``Add-AzureAccount``


#### Initialization
1. Open a new PowerShell window.
1. Select your subscription by entering the following:

        $subId = "<your Subscription ID>"
        Select-AzureSubscription -SubscriptionId $subId
        Switch-AzureMode AzureResourceManager


#### Getting a resource group
To create an resource in Azure, you must select a resource group.

To enumerate the resource groups in your subscription:
    
    Get-AzureResourceGroup
    
To create a new resource group:

    $resourceGroupName = "<your new resource group name>"
    New-AzureResourceGroup -Name $resourceGroupName -Location "East US 2"
    

#### Creating a new Azure Data Lake account

> NOTE: The account name must only contain lowercase letters and numbers.

	$dataLakeAccountName = "<your new Data Lake account name>"
    
    New-AzureDataLakeAccount `
        -ResourceGroupName $resourceGroupName `
        -Name $dataLakeAccountName `
        -Location "East US 2"

#### That's it!

Now you can get started using our [Tutorials](Tutorials.md) and the [User Manual](UserManual.md), which shows how to use each cmdlet.

#### Learn more

* Azure Data Lake
    * [PowerShell Tutorials](https://github.com/MicrosoftBigData/AzureDataLake/blob/master/docs/PowerShell/Tutorials.md) - Learn how to perform some basic activities with your Azure Data Lake account in PowerShell.
    * [PowerShell User Manual](https://github.com/MicrosoftBigData/AzureDataLake/tree/master/docs/PowerShell/UserManual.md) - See how to use the Azure Data Lake PowerShell cmdlets.

------------

### Useful links

Browse the following pages:

* [Getting Started](../GettingStarted.md)
* Tools
    * [Azure Portal](../AzurePortal/FirstSteps.md)
    * [Data Lake PowerShell](../PowerShell/FirstSteps.md)
    * [SDK](../SDK/FirstSteps.md)
