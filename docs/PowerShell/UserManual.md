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
        Cmdlet          Add-AzureDataLakeContent                           0.9.4      Azure
        Cmdlet          Export-AzureDataLakeItem                           0.9.4      Azure
        Cmdlet          Get-AzureDataLakeAccount                           0.9.4      Azure
        Cmdlet          Get-AzureDataLakeChildItem                         0.9.4      Azure
        Cmdlet          Get-AzureDataLakeItem                              0.9.4      Azure
        Cmdlet          Get-AzureDataLakeItemAcl                           0.9.4      Azure
        Cmdlet          Get-AzureDataLakeItemContent                       0.9.4      Azure
        Cmdlet          Get-AzureDataLakeItemOwner                         0.9.4      Azure
        Cmdlet          Get-AzureDataLakeItemPermissions                   0.9.4      Azure
        Cmdlet          Import-AzureDataLakeItem                           0.9.4      Azure
        Cmdlet          Join-AzureDataLakeItem                             0.9.4      Azure
        Cmdlet          Move-AzureDataLakeItem                             0.9.4      Azure
        Cmdlet          New-AzureDataLakeAccount                           0.9.4      Azure
        Cmdlet          New-AzureDataLakeItem                              0.9.4      Azure
        Cmdlet          Remove-AzureDataLakeAccount                        0.9.4      Azure
        Cmdlet          Remove-AzureDataLakeItem                           0.9.4      Azure
        Cmdlet          Remove-AzureDataLakeItemAcl                        0.9.4      Azure
        Cmdlet          Remove-AzureDataLakeItemAclEntry                   0.9.4      Azure
        Cmdlet          Set-AzureDataLakeAccount                           0.9.4      Azure
        Cmdlet          Set-AzureDataLakeItemAcl                           0.9.4      Azure
        Cmdlet          Set-AzureDataLakeItemAclEntry                      0.9.4      Azure
        Cmdlet          Set-AzureDataLakeItemOwner                         0.9.4      Azure
        Cmdlet          Set-AzureDataLakeItemPermissions                   0.9.4      Azure
        Cmdlet          Test-AzureDataLakeAccount                          0.9.4      Azure
        Cmdlet          Test-AzureDataLakeItem                             0.9.4      Azure

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

            Get-AzureDataLakeItem -AccountName $dataLakeAccountName -Path $path

    * Get file size in bytes
            
            $fileInfo = Get-AzureDataLakeItem -AccountName $dataLakeAccountName -Path $path
            $fileInfo.Size

* **List** files within a specific folder

        Get-AzureDataLakeChildItem -AccountName $dataLakeAccountName -Path $folderPath

* **Test existence** of a specific file or folder

        Test-AzureDataLakeItem -AccountName $dataLakeAccountName -Path $path

* **Create** a new file or folder

        New-AzureDataLakeItem -AccountName $dataLakeAccountName -Path $filePath

* **Upload** a specific file or folder from local machine to Data Lake

        Import-AzureDataLakeItem -AccountName $dataLakeAccountName -Path $localPath -Destination $remotePath

* **Download** a specific file or folder from Data Lake to local machine

        Export-AzureDataLakeItem -AccountName $dataLakeAccountName -Path $remotePath -Destination $localPath

* **Delete** a specific file or folder

        Remove-AzureDataLakeItem -AccountName $dataLakeAccountName -Path $filePath

* **Rename or move** a specific file or folder

        Move-AzureDataLakeItem -AccountName $dataLakeAccountName -Path $filePath -Destination $filePath

* **Concatenate** (destructively) two or more files

        Join-AzureDataLakeItem -AccountName $dataLakeAccountName -Paths $filePaths -Destination $filePath

* **Append** content to a specific file

        Add-AzureDataLakeItemContent -AccountName $dataLakeAccountName -Path $filePath

* **Read** content of a specific file

        Get-AzureDataLakeItemContent -AccountName $dataLakeAccountName -Path $filePath


#### File system permissions

* **Read** permissions of a specific file or folder

        # NOTE: During Data Lake preview, file and folder permissions are permanently set to 777.
        
        $filePath = "/"
        Get-AzureDataLakeItemPermissions -AccountName $dataLakeAccountName -Path $filePath

* **Set** permissions of a specific file or folder

        # NOTE: During Data Lake preview, file and folder permissions are permanently set to 777.
        
        #There are three classes of permissions for a given item:
        #   User owner, Group owner, Other
        #   r = read
        #   w = write
        #   x = execute
        #   rwxrwxr-- means:
        #       the user owner can read, write, and execute the item
        #       all others in the AAD tenant can read the item
        #   7 => 111 in binary, mapping to rwx
        #   4 => 100 in binary, mapping to r--
        
        $perm = "774"
        Get-AzureDataLakeItemPermissions -AccountName $dataLakeAccountName -Path $filePath -Permissions $perm
        
        $perm = "rwxrwxr--"
        Get-AzureDataLakeItemPermissions -AccountName $dataLakeAccountName -Path $filePath -Permissions $perm

------------

### Useful links

Browse the following pages:

* [Getting Started](../GettingStarted.md)
* Tools
    * [Azure Portal](../AzurePortal/FirstSteps.md)
    * [PowerShell](../PowerShell/FirstSteps.md)
    * [SDK](../SDK/FirstSteps.md)
