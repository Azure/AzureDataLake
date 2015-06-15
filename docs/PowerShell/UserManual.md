# User Manual: Azure PowerShell with Data Lake

This guide assumes you have previously followed the steps in the main [Getting Started guide](../GettingStarted.md).

------------

### The Basics

#### Data Lake Paths

When interacting with the data in your Data Lake, many of the cmdlets use the ``-Path`` parameter. The format of the path that you provide should be as follows:

        $path = "swebhdfs://myDataLakeAccount.azuredatalake.net/foo/bar.txt"
        Get-AzureDataLakeItem -Path $path

#### Initialization
1. Open a new PowerShell window.
1. Select your subscription by entering the following:

        Select-AzureSubscription -SubscriptionId $subscriptionId

#### Help

* To get a list of Azure Data Lake cmdlets, enter the following:

        Get-Command *AzureDataLake*

    Expected response:

        CommandType     Name                                               Version    Source
        -----------     ----                                               -------    ------
        Cmdlet          Add-AzureDataLakeContent                           0.9.1      Azure
        Cmdlet          Copy-AzureDataLakeItem                             0.9.1      Azure
        Cmdlet          Get-AzureDataLakeAccount                           0.9.1      Azure
        Cmdlet          Get-AzureDataLakeChildItem                         0.9.1      Azure
        Cmdlet          Get-AzureDataLakeItem                              0.9.1      Azure
        Cmdlet          Get-AzureDataLakeItemAccess                        0.9.1      Azure
        Cmdlet          Get-AzureDataLakeItemContent                       0.9.1      Azure
        Cmdlet          Join-AzureDataLakeItem                             0.9.1      Azure
        Cmdlet          Move-AzureDataLakeItem                             0.9.1      Azure
        Cmdlet          New-AzureDataLakeAccount                           0.9.1      Azure
        Cmdlet          New-AzureDataLakeItem                              0.9.1      Azure
        Cmdlet          Remove-AzureDataLakeAccount                        0.9.1      Azure
        Cmdlet          Remove-AzureDataLakeItem                           0.9.1      Azure
        Cmdlet          Set-AzureDataLakeAccount                           0.9.1      Azure
        Cmdlet          Set-AzureDataLakeItemAccess                        0.9.1      Azure
        Cmdlet          Test-AzureDataLakeAccount                          0.9.1      Azure
        Cmdlet          Test-AzureDataLakeItem                             0.9.1      Azure

* To get information about a specific cmdlet, such as the syntax or the return type, enter the following:
    
        Get-Help Get-AzureDataLakeAccount

#### Account management permissions

* To learn how Azure Resource Groups and Role-Based Access Control work, take a look at the [Azure documentation on Resource Group management](https://azure.microsoft.com/en-us/documentation/articles/resource-group-rbac/).

------------

### Usage examples

#### Account management

* **List** Data Lake accounts within the current subscription

    Sample usage:
    
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
        
* **Delete** a Data Lake account

        Remove-AzureDataLakeAccount `
            -Name $dataLakeAccountName
        

#### Data operations

* **Get details** of a specific file or folder

    * List all details

            Get-AzureDataLakeItem -Path $path

    * Get file size in bytes
            
            $fileInfo = Get-AzureDataLakeItem -Path $path
            $fileInfo.Size

* **List** files within a specific folder

        Get-AzureDataLakeChildItem -Path $folderPath

* **Test existence** of a specific file or folder

        Test-AzureDataLakeItem -Path $path

* **Create** a new file or folder

        New-AzureDataLakeItem -Path $filePath

* **Upload** a specific file or folder from local machine to Data Lake

        Copy-AzureDataLakeItem -Path $localPath -Destination $remotePath

* **Download** a specific file or folder from Data Lake to local machine

        Copy-AzureDataLakeItem -Path $remotePath -Destination $localPath

* **Delete** a specific file or folder

        Remove-AzureDataLakeItem -Path $filePath

* **Rename or move** a specific file or folder

        Move-AzureDataLakeItem -Path $filePath -Destination $filePath

* **Concatenate** (destructively) two or more files

        Join-AzureDataLakeItem -Paths $filePaths -Destination $filePath

* **Append** content to a specific file

        Add-AzureDataLakeItemContent -Path $filePath

* **Read** content of a specific file

        Get-AzureDataLakeItemContent -Path $filePath

------------

### Useful links

Browse the following pages:

* [Getting Started](../GettingStarted.md)
* Tools
    * [Azure Portal](../AzurePortal/FirstSteps.md)
    * [PowerShell](../PowerShell/FirstSteps.md)
    * [SDK](../SDK/FirstSteps.md)
