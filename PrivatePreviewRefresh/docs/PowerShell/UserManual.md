# User Manual: Azure PowerShell with Data Lake


### Prerequisite steps

This guide assumes that you previously followed the steps in the main Data Lake [Getting Started](../GettingStarted.md) guide.


#### Initialization
1. Open a new PowerShell window.
1. Select your subscription by entering the following:

        Select-AzureSubscription -SubscriptionId $subscriptionId

#### Help
To get information about a specific cmdlet, such as the syntax or the return type, enter the following:
    
    help Get-AzureDataLakeAccount


------------

#### Data Lake PowerShell cmdlets

##### Account management

* **List** Data Lake accounts within the current subscription
    
        Get-AzureDataLakeAccount
    
* **List** Data Lake accounts within a specific resource group
    
        Get-AzureDataLakeAccount -ResourceGroupName $resourceGroupName
    
* **Get details** of a specific Data Lake account
    
        Get-AzureDataLakeAccount -Name $dataLakeAccountName

* **Test existence** of a specific Data Lake account

        Test-AzureDataLakeAccount -Name $dataLakeAccountName

* **Create** a new Data Lake account

        New-AzureDataLakeAccount `
            -ResourceGroupName $resourceGroupName `
            -Name $dataLakeAccountName `
            -Location "East US 2"


##### Data operations

* **List** files within a specific folder

		Get-AzureDataLakeChildItem -Path $folderPath

* **Get details** of a specific file or folder

		Get-AzureDataLakeItem -Path $path

* **Test existence** of a specific file or folder

		Test-AzureDataLakeItem -Path $path

* **Create** a new file or folder

* **Upload** a specific file or folder from local machine to Data Lake

* **Download** a specific file from Data Lake to local machine

* **Delete** a specific file or folder

* **Rename or move** a specific file or folder

* **Concatenate** (destructively) two or more files

* **Append** content to a specific file

* **Read** content of a specific file


##### File/folder access control

* **Get** access settings for a specific file or folder

* **Set** access settings for a specific file or folder

------------

### Learn more

To learn more, browse the following pages:

(place table of contents here)