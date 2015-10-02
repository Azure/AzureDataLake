<properties 
   pageTitle="Get started with Data Lake | Azure" 
   description="Use Azure PowerShell to create a Data Lake account and perform basic operations in the Data Lake" 
   services="data-lake" 
   documentationCenter="" 
   authors="nitinme" 
   manager="paulettm" 
   editor="cgronlun"/>
 
<tags
   ms.service="data-lake"
   ms.devlang="na"
   ms.topic="article"
   ms.tgt_pltfrm="na"
   ms.workload="big-data" 
   ms.date="09/29/2015"
   ms.author="nitinme"/>

# Get started with Azure Data Lake using Azure PowerShell

> [AZURE.SELECTOR]
- [Portal](azure-data-lake-get-started-portal.md)
- [PowerShell](azure-data-lake-get-started-powershell.md)
- [.NET SDK](azure-data-lake-get-started-net-sdk.md)

Learn how to use Azure PowerShell to create an Azure Data Lake account and perform basic operations such as create folders, upload and download data files, delete your account, etc. For more information about Data Lake, see [Azure Data Lake](azure-data-lake-overview.md).

## Prerequisites

Before you begin this tutorial, you must have the following:

- **An Azure subscription**. See [Get Azure free trial](http://azure.microsoft.com/documentation/videos/get-azure-free-trial-for-testing-hadoop-in-hdinsight/).

## Install Azure PowerShell

1. Download the [Azure PowerShell module for Data Lake](https://github.com/MicrosoftBigData/AzureDataLake/releases).

2. Extract **Azure_PowerShell.msi** from the .zip and double-click to install.

3. From your desktop, open a new Azure PowerShell window, and enter the following snippet. When prompted to log in, make sure you log in as one of the subscription admininistrators/owner:

        # Log in to your Azure account
		Add-AzureAccount
        
		# List all the subscriptions associated to your account
		Get-AzureSubscription
		
		# Select a subscription 
		Select-AzureSubscription -SubscriptionName <subscription name>
        
		# Switch to Azure Resource Manager mode and register for Data Lake
		Switch-AzureMode AzureResourceManager
        Register-AzureProvider -ProviderNamespace "Microsoft.DataLake" 


## Create an Azure Data Lake account

Follow these steps to create an Azure Data Lake account.

1. An Azure Data Lake account is associated with an Azure Resource Group. Start by creating an Azure Resource Group.

		$resourceGroupName = "<your new resource group name>"
    	New-AzureResourceGroup -Name $resourceGroupName -Location "East US 2"

	![Create an Azure Resource Group](./media/azure-data-lake-get-started-powershell/ADL.PS.CreateResourceGroup.png "Create an Azure Resource Group")

2. Create an Azure Data Lake account. The account name you specify must only contain lowercase letters and numbers.

		$dataLakeAccountName = "<your new Data Lake account name>"
    	New-AzureDataLakeAccount -ResourceGroupName $resourceGroupName -Name $dataLakeAccountName -Location "East US 2"

	![Create an Azure Data Lake account](./media/azure-data-lake-get-started-powershell/ADL.PS.CreateADLAcc.png "Create an Azure Data Lake account")

3. Verify that the account is successfully created.

		Test-AzureDataLakeAccount -Name <Data Lake account name>

	The output for this should be **True**.

## Create directory structures in your Azure Data Lake account

You can create directories under your Azure Data Lake account to manage and store data. 

1. Specify a root directory.

		$myrootdir = "swebhdfs://<Data Lake account name>.azuredatalake.net"

2. Create a new directory called **mynewdirectory** under the specified root.

		New-AzureDataLakeItem -Folder AccountName <Data Lake account> -Path $myrootdir/mynewdirectory

3. Verify that the new directory is successfully created.

		Get-AzureDataLakeChildItem -AccountName <Data Lake account> -Path $myrootdir

	It should show an output like the following:

	![Verify Directory](./media/azure-data-lake-get-started-powershell/ADL.PS.Verify.Dir.Creation.png "Verify Directory")


## Upload data to your Azure Data Lake account

You can upload your data to an Azure Data Lake account directly at the root level or to a directory that you created within the account. The snippets below demonstrate how to upload some sample data to the directory (**mynewdirectory**) you created in the previous section.

If you are looking for some sample data to upload, you can get the **OlympicAthletes.tsv** file from the [AzureDataLake Git Repository](https://github.com/MicrosoftBigData/AzureDataLake/raw/master/Samples/SampleData/OlympicAthletes.tsv). Download the file and store it in a local directory on your computer, such as  C:\sampledata\.

	Import-AzureDataLakeItem -AccountName <Data Lake account> -Path "C:\sampledata\OlympicAthletes.tsv" -Destination $myrootdir\mynewdirectory\OlympicAthletes.tsv

You can upload more than one file from the source folder to the destination folder by giving the path to the folder name and omitting the file name. For example, the following command will upload all files in C:\sampledata\ to **mynewdirectory** in your Azure Data Lake account.

	Import-AzureDataLakeItem -AccountName <Data Lake account> -Path "C:\sampledata\" -Destination $myrootdir\mynewdirectory\

## Rename, download, and delete data from your Azure Data Lake account

To rename a file, use the following command:

    Move-AzureDataLakeItem -AccountName <Data Lake account> -Path $myrootdir\mynewdirectory\OlympicAthletes.tsv -Destination $myrootdir\mynewdirectory\OlympicAthletes_Copy.tsv

To download a file, use the following command:

	Export-AzureDataLakeItem -AccountName <Data Lake account> -Path $myrootdir\mynewdirectory\OlympicAthletes_Copy.tsv -Path "C:\sampledata\OlympicAthletes_Copy.tsv"

To delete a file, use the following command:

	Remove-AzureDataLakeItem -AccountName <Data Lake account> -Paths $myrootdir\mynewdirectory\OlympicAthletes_Copy.tsv 
	
When prompted, enter **Y** to delete the item. If you have more than one file to delete, you can provide all the paths separated by comma.

	Remove-AzureDataLakeItem -AccountName <Data Lake account> -Paths $myrootdir\mynewdirectory\OlympicAthletes.tsv, $myrootdir\mynewdirectory\OlympicAthletes_Copy.tsv

## Secure your data

You can secure the data stored in your Azure Data Lake account by using access control and providing expiry settings on the data. For instructions on how to do that, see [ TBD: Link to topic ].


## Delete your Azure Data Lake account

Use the following command to delete your Data Lake account.

	Remove-AzureDataLakeAccount <Data Lake account>

When prompted, enter **Y** to delete the account.

	
## See Also

[ TBD: Add links ]