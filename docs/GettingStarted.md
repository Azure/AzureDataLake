# Getting Started: Azure Data Lake

------------

### Getting started

To use this new Azure service, follow these simple steps:

#### Your Azure subscription
1. If you're already part of an Azure subscription, skip this step.<br />If you're not, [create a new Azure subscription here](https://account.windowsazure.com/Subscriptions).
1. Visit [the Azure Portal](https://portal.azure.com) and log in.
1. Click *Browse* and select *Subscriptions*.
1. Select the desired subscription.
1. Write down the given Subscription ID. You'll need this later.
   

#### Install Azure PowerShell with Data Lake
1. Download the [Azure PowerShell module here](https://microsoft.sharepoint.com/teams/AzureDataLake/Documents/PrivatePreviewRefresh/AzurePowerShell.zip).
1. Extract the contents of *AzurePowerShell.zip*.
1. Right click on *install.ps1* and click *Run with PowerShell*.
1. Follow the steps in the installation wizard.


#### Register for early access
Open a new PowerShell window and enter the following:
    
    Select-AzureSubscription -SubscriptionId <your_subscription_id>
    Register-AzureProviderFeature -FeatureName "Data Lake" -ProviderNamespace "Microsoft.DataLake"

    
#### Check for existing accounts
Run the following PowerShell cmdlets to see if you already have access to an Azure Data Lake account.

    Get-AzureDataLakeAccount

Note: If you want to access an existing Azure Data Lake account, contact the account's owner.


#### Creating a new account

If you want to create a new Azure Data Lake account, you can do so using these tools:

* [Azure Portal](AzurePortal/FirstSteps.md)
* [Azure PowerShell](PowerShell/FirstSteps.md)


#### Available tools

We offer the following tools that can you can use to interact with your Data Lake account:

* [Azure Portal](AzurePortal/FirstSteps.md)
* [Azure PowerShell](PowerShell/FirstSteps.md)
* [Azure .NET SDK](SDK/FirstSteps.md)
* [WebHDFS support](WebHDFS/FirstSteps.md)
    
------------

### Useful links

Browse the following pages:

* *(place table of contents here)*