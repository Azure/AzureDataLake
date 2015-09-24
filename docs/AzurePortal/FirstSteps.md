# First Steps: Portal for Data Lake

### First steps

To create an Azure Data Lake account:

1. Go to the new Azure Portal using [this link](https://portal.azure.com/?microsoft_azure_biganalytics=trueµsoft_azure_datalake=trueµsoft_azure_kona=true&hubsExtension_ItemHideKey=AzureDataLake_BigStorage,AzureBigAnalytics_BigCompute,AzureKona_BigCompute). The link has special keys which will let you search for Kona and Data Lake.

1. Click on Marketplace.

   ![](../img/Portal/AzurePortal.png)

1. In the Search Bar, type "Azure Data Lake".

1. Click Create.

### Known Errors ###
####Error registering resource providers####
#####Detailed error#####
Registering the resource providers has failed. Additional details from the underlying API that might be helpful: The resource namespace ("Microsoft.Kona" OR "Mirosoft.DataLake") is invalid.
#####What this means#####
This usually means the subscription you tried to use wasn't whitelisted. Send "konaonboard@microsoft.com" an email with the subscription ID you want to use and we will whitelist it.

### Useful links

Browse the following pages:

* [Getting Started](../GettingStarted.md)
* Tools
    * [Azure Portal](../AzurePortal/FirstSteps.md)
    * [Data Lake PowerShell](../PowerShell/FirstSteps.md)
    * [SDK](../SDK/FirstSteps.md)
