# User Manual: Azure PowerShell with Data Lake Store

This guide assumes you have previously followed the steps in the main [Getting Started guide](../GettingStarted.md).

------------

### The Basics

#### Data Lake Store Paths

When interacting with the data in your Data Lake Store, many of the cmdlets use the ``-Path`` parameter. The format of the path that you provide should be as follows:

        $path = "/foo/bar.txt"
        Get-AzureRmDataLakeStoreItem -Path $path

#### Initialization
1. Open a new PowerShell window.
1. If you haven't logged in yet, enter the following:

        Login-AzureRmAccount

1. Select your subscription by entering the following:

        Set-AzureRmContext -SubscriptionId $subscriptionId

#### Help

* To get a list of Azure Data Lake Store cmdlets, enter the following:

        Get-Command *Azure*DataLakeStore*

    Expected response:

        CommandType     Name                                               Version    Source
        -----------     ----                                               -------    ------
        Cmdlet          Add-AzureRmDataLakeStoreContent                    0.9.10     AzureResourceManager
        Cmdlet          Export-AzureRmDataLakeStoreItem                    0.9.10     AzureResourceManager
        Cmdlet          Get-AzureRmDataLakeStoreAccount                    0.9.10     AzureResourceManager
        Cmdlet          Get-AzureRmDataLakeStoreChildItem                  0.9.10     AzureResourceManager
        Cmdlet          Get-AzureRmDataLakeStoreItem                       0.9.10     AzureResourceManager
        Cmdlet          Get-AzureRmDataLakeStoreItemAcl                    0.9.10     AzureResourceManager
        Cmdlet          Get-AzureRmDataLakeStoreItemContent                0.9.10     AzureResourceManager
        Cmdlet          Get-AzureRmDataLakeStoreItemOwner                  0.9.10     AzureResourceManager
        Cmdlet          Import-AzureRmDataLakeStoreItem                    0.9.10     AzureResourceManager
        Cmdlet          Join-AzureRmDataLakeStoreItem                      0.9.10     AzureResourceManager
        Cmdlet          Move-AzureRmDataLakeStoreItem                      0.9.10     AzureResourceManager
        Cmdlet          New-AzureRmDataLakeStoreAccount                    0.9.10     AzureResourceManager
        Cmdlet          New-AzureRmDataLakeStoreItem                       0.9.10     AzureResourceManager
        Cmdlet          Remove-AzureRmDataLakeStoreAccount                 0.9.10     AzureResourceManager
        Cmdlet          Remove-AzureRmDataLakeStoreItem                    0.9.10     AzureResourceManager
        Cmdlet          Remove-AzureRmDataLakeStoreItemAcl                 0.9.10     AzureResourceManager
        Cmdlet          Remove-AzureRmDataLakeStoreItemAclEntry            0.9.10     AzureResourceManager
        Cmdlet          Set-AzureRmDataLakeStoreAccount                    0.9.10     AzureResourceManager
        Cmdlet          Set-AzureRmDataLakeStoreItemAcl                    0.9.10     AzureResourceManager
        Cmdlet          Set-AzureRmDataLakeStoreItemAclEntry               0.9.10     AzureResourceManager
        Cmdlet          Set-AzureRmDataLakeStoreItemOwner                  0.9.10     AzureResourceManager
        Cmdlet          Test-AzureRmDataLakeStoreAccount                   0.9.10     AzureResourceManager
        Cmdlet          Test-AzureRmDataLakeStoreItem                      0.9.10     AzureResourceManager

* To get information about a specific cmdlet, such as the syntax or the return type, enter the following:
    
        Get-Help Get-AzureRmDataLakeStoreAccount

#### Account management permissions

* To learn how Azure Resource Groups and Role-Based Access Control work, take a look at the [Azure documentation on Resource Group management](https://azure.microsoft.com/en-us/documentation/articles/resource-group-rbac/).

------------

### Usage examples

#### Account management

* **List** Data Lake Store accounts within the current subscription

    Sample usage:
    
        Get-AzureRmDataLakeStoreAccount
    
* **List** Data Lake Store accounts within a specific resource group
    
        Get-AzureRmDataLakeStoreAccount -ResourceGroupName $resourceGroupName
    
* **Get details** of a specific Data Lake Store account
    
        Get-AzureRmDataLakeStoreAccount -Name $adlStoreAccountName

* **Test existence** of a specific Data Lake Store account

        Test-AzureRmDataLakeStoreAccount -Name $adlStoreAccountName

* **Create** a new Data Lake Store account

        New-AzureRmDataLakeStoreAccount `
            -ResourceGroupName $resourceGroupName `
            -Name $adlStoreAccountName `
            -Location "East US 2"
        
* **Delete** a Data Lake Store account

        Remove-AzureRmDataLakeStoreAccount `
            -Name $adlStoreAccountName
        

#### Data operations

* **Get details** of a specific file or folder

    * List all details

            Get-AzureRmDataLakeStoreItem -AccountName $adlStoreAccountName -Path $path

    * Get file size in bytes
            
            $fileInfo = Get-AzureRmDataLakeStoreItem -AccountName $adlStoreAccountName -Path $path
            $fileInfo.Size

* **List** files within a specific folder

        Get-AzureRmDataLakeStoreChildItem -AccountName $adlStoreAccountName -Path $folderPath

* **Test existence** of a specific file or folder

        Test-AzureRmDataLakeStoreItem -AccountName $adlStoreAccountName -Path $path

* **Create** a new file or folder

        New-AzureRmDataLakeStoreItem -AccountName $adlStoreAccountName -Path $filePath

* **Upload** a specific file or folder from local machine to Data Lake Store

        Import-AzureRmDataLakeStoreItem -AccountName $adlStoreAccountName -Path $localPath -Destination $remotePath

* **Download** a specific file from Data Lake Store to local machine

        Export-AzureRmDataLakeStoreItem -AccountName $adlStoreAccountName -Path $remotePath -Destination $localPath

* **Delete** a specific file or folder

        Remove-AzureRmDataLakeStoreItem -AccountName $adlStoreAccountName -Path $filePath

* **Rename or move** a specific file or folder

        Move-AzureRmDataLakeStoreItem -AccountName $adlStoreAccountName -Path $filePath -Destination $filePath

* **Concatenate** (destructively) two or more files

        Join-AzureRmDataLakeStoreItem -AccountName $adlStoreAccountName -Paths $filePaths -Destination $filePath

* **Append** content to a specific file

        Add-AzureRmDataLakeStoreContent -AccountName $adlStoreAccountName -Path $filePath

* **Read** content of a specific file

        Get-AzureRmDataLakeStoreItemContent -AccountName $adlStoreAccountName -Path $filePath


#### File system permissions

* **Read** permissions of the root directory

    > NOTE: During Data Lake Store preview, file and folder permissions are permanently set to 777.
        
        Get-AzureRmDataLakeStoreItemPermissions -AccountName $adlStoreAccountName -Path /

* **Set** permissions of the root directory

    > NOTE: During Data Lake Store preview, file and folder permissions are permanently set to 777.
        
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
        Get-AzureRmDataLakeStoreItemPermissions -AccountName $adlStoreAccountName -Path / -Permissions $perm
        
        $perm = "rwxrwxr--"
        Get-AzureRmDataLakeStoreItemPermissions -AccountName $adlStoreAccountName -Path / -Permissions $perm

* **Get** the root directory's access control list
        
    > NOTE: During Data Lake Store preview, only the root directory has a functional access control list.
        
        Get-AzureRmDataLakeStoreItemAcl -AccountName $adlStoreAccountName -Path /

* **Add or change** a user entry of the root directory's access control list

    > NOTE: During Data Lake Store preview, only the root directory has a functional access control list.
        
    * Give username@example.com Read, Write, and Execute permissions on the root directory.
                
                $user = Get-AzureADUser -Mail username@example.com
                $objectId = $user.Id
                Set-AzureRmDataLakeStoreItemAclEntry `
                        -AccountName $adlStoreAccountName `
                        -Path / `
                        -AceType User `
                        -Id $objectId `
                        -Permissions All `
                        -Force

    * Give otherperson@example.com Read permission on the root directory.
        
                $user = Get-AzureADUser -Mail otherperson@example.com
                $objectId = $user.Id
                Set-AzureRmDataLakeStoreItemAclEntry `
                        -AccountName $adlStoreAccountName `
                        -Path / `
                        -AceType User `
                        -Id $objectId `
                        -Permissions Read `
                        -Force

* **Clear** the root directory's access control list

    > NOTE: During Data Lake Store preview, only the root directory has a functional access control list.
        
        Remove-AzureRmDataLakeStoreItemAcl -AccountName $adlStoreAccountName -Path /

------------

### Useful links

Browse the following pages:

* [Getting Started](../GettingStarted.md)
* Tools
    * [Data Lake Store in the Azure Portal](../AzurePortal/FirstSteps.md)
    * [Data Lake Store PowerShell](../PowerShell/FirstSteps.md)
    * [Data Lake Store .NET SDK](../SDK/FirstSteps.md)
