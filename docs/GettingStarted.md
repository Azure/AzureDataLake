# Getting Started: Azure Data Lake

------------

### Getting started

To use this new Azure service, follow these simple steps:

#### Your Azure subscription
1. If you're already part of an Azure subscription, skip this step.<br />If you're not, [create a new Azure subscription here](https://account.windowsazure.com/Subscriptions).
1. Visit [the Azure Portal](https://portal.azure.com) and log in.
1. Click **Browse** and select **Subscriptions**.
1. Select the desired subscription.
1. Write down the given Subscription ID. You'll need this later.
   
#### Email your TAP Buddy
Send your Subscription ID to your TAP Buddy to receive access to Azure Data Lake.

#### Install Azure Data Lake
1. Download the [Azure PowerShell module here](https://github.com/MicrosoftBigData/AzureDataLake/releases/download/PowerShellSDK/AzurePS_KonaDataLake.zip).

1. Extract the contents of **AzurePS_KonaDataLake.zip**.

1. Right click on **INSTALL_RunAsAdministrator** and click **Run as administrator**.

1. Follow the steps in the installation wizard.

1. In a new PowerShell window, enter the following:

        Select-AzureSubscription -SubscriptionId <the Subscription ID that your wrote down previously>
        Register-AzureProvider -ProviderNamespace "Microsoft.DataLake"
    
#### Check for existing accounts
Open a new PowerShell window. Run the following PowerShell cmdlets to see if you already have access to a Data Lake account.

    Get-AzureDataLakeAccount

#### Create a new account

If you want to create a new Azure Data Lake account, you can do so through the Portal or PowerShell:

* [Azure PowerShell](PowerShell/FirstSteps.md)
* [Azure Portal](AzurePortal/FirstSteps.md) *Note: Temporary limitation - only supports single subscriptions

------------

### Useful links

* [Getting Started](GettingStarted.md)
* Tools
    * [Azure Portal](AzurePortal/FirstSteps.md)
    * [Data Lake PowerShell](PowerShell/FirstSteps.md)
    * [SDK](SDK/FirstSteps.md)
