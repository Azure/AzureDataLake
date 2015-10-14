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


#### Initialization
1. Open a new PowerShell window.
2. Log in to your Azure account. If you are attempting to connect to a dogfood environment, run the following script first: https://github.com/MicrosoftBigData/ProjectKona/blob/master/PowerShellSamples/Initialize-AzureEnvironment.ps1

        Install-Module "C:\Program Files (x86)\Microsoft SDKs\Azure\PowerShell\ResourceManager\AzureResourceManager\AzureResourceManager.psd1"

        Login-AzureRmAccount
        - OR (for dogfood after running the above script)-
        Login-AzureRmAccount -Environment dogfood

3. Select your subscription by entering the following:

        $subId = "<your Subscription ID>"
        Set-AzureRmContext -SubscriptionId $subId


#### Getting a resource group
To create an resource in Azure, you must select a resource group.

To enumerate the resource groups in your subscription:
    
    Get-AzureRmResourceGroup
    
To create a new resource group:

    $resourceGroupName = "<your new resource group name>"
    New-AzureRmResourceGroup -Name $resourceGroupName -Location "East US 2"
    

#### Creating a new Azure Data Lake Store account

If you are creating a Data Lake Store account for the first time:

	Register-AzureRmResourceProvider -ProviderNamespace "Microsoft.DataLake" 

> NOTE: The account name must only contain lowercase letters and numbers.

    $dataLakeAccountName = "<your new Data Lake account name>"
    
    New-AzureRmDataLakeStoreAccount `
        -ResourceGroupName $resourceGroupName `
        -Name $adlStoreAccountName `
        -Location "East US 2"

#### That's it!

Now you can get started using our [Tutorials](Tutorials.md) and the [User Manual](UserManual.md), which shows how to use each cmdlet.

#### Learn more

* Azure Data Lake Store
    * [PowerShell Tutorials](https://github.com/MicrosoftBigData/AzureDataLake/blob/master/docs/PowerShell/Tutorials.md) - Learn how to perform some basic activities with your Azure Data Lake Store account in PowerShell.
    * [PowerShell User Manual](https://github.com/MicrosoftBigData/AzureDataLake/tree/master/docs/PowerShell/UserManual.md) - See how to use the Azure Data Lake Store PowerShell cmdlets.

------------

### Useful links

Browse the following pages:

* [Getting Started](../GettingStarted.md)
* Tools
    * [Data Lake Store in the Azure Portal](../AzurePortal/FirstSteps.md)
    * [Data Lake Store PowerShell](../PowerShell/FirstSteps.md)
    * [Data Lake Store .NET SDK](../SDK/FirstSteps.md)
