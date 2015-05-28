# Getting Started: Kona

### Getting started

To use these two new Azure services, follow these easy steps:

#### Your Azure subscription
1. If you're already part of an Azure subscription, skip this step.<br />If you're not, [create a new Azure subscription here](https://account.windowsazure.com/Subscriptions).
1. Visit [the Azure Portal](https://portal.azure.com) and log in.
1. Click *Browse* and select *Subscriptions*.
1. Select the desired subscription.
1. Write down the given Subscription ID. You'll need this later.
   

#### Install Azure PowerShell with Kona and Data Lake
1. Download the [Azure PowerShell module here](https://microsoft.sharepoint.com/teams/ProjectKona/Documents/PrivatePreviewRefresh/AzurePowerShell.zip).
1. Extract the contents of *AzurePowerShell.zip*.
1. Right click on *install.ps1* and click *Run with PowerShell*.
1. Follow the steps in the installation wizard.


#### Register for early access
Open a new PowerShell window and enter the following:
    
    Select-AzureSubscription -SubscriptionId <your_subscription_id>
    Register-AzureProviderFeature -FeatureName "Kona" -ProviderNamespace "Microsoft.Kona"
    Register-AzureProviderFeature -FeatureName "Data Lake" -ProviderNamespace "Microsoft.DataLake"

    
#### Check for existing accounts
Run the following PowerShell cmdlets to see if you already have access to a Kona account.

    Get-AzureKonaAccount

Note: If you want to access an existing Azure Kona account, contact the account's owner.


#### Creating a new account

If you want to create a new Azure Kona account, you can do so through the Portal or PowerShell:

* [Azure Portal](AzurePortal/GettingStarted.md)
* [Azure PowerShell](PowerShell/GettingStarted.md)

    
### Learn more

To learn more about how to interact with Kona, browse the following pages:


#### Links
* [Private Preview SharePoint](http://aka.ms/ProjectKona)

#### Azure Portal
* [Getting Started](GettingStarted.md)

#### SQLIP Studio
* [Getting Started](SQLIPStudio/GettingStarted.md)
* [Job Authoring](SQLIPStudio/JobAuthoring.md)

#### PowerShell
* [Getting Started](PowerShell/GettingStarted.md)
* [User Manual - Kona](PowerShell/UserManual.md#kona-powershell-cmdlets)
* [User Manual - Data Lake](PowerShell/UserManual.md#data-lake-powershell-cmdlets)

#### SDK
* [Getting Started](SDK/GettingStarted.md)
* [User Manual - Kona](SDK/UserManual.md#kona-sdk)
* [User Manual - Data Lake](SDK/UserManual.md#data-lake-sdk)