# User Manual: Azure PowerShell with Data Lake

This guide assumes you have previously followed the steps in the main [Getting Started guide](../GettingStarted.md).

------------

### The Basics

#### Initialization
1. Open a new PowerShell window.
1. Select your subscription by entering the following:

        Select-AzureSubscription -SubscriptionId $subscriptionId

#### Help
To get information about a specific cmdlet, such as the syntax or the return type, enter the following:
    
    help Get-AzureDataLakeAccount

#### Account management permissions

* To learn how Azure Resource Groups and Role-Based Access Control work, take a look at the [Azure documentation on Resource Group management](https://azure.microsoft.com/en-us/documentation/articles/resource-group-rbac/).

------------

### Data Lake PowerShell cmdlets

#### Account management

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

#### Data operations

* **List** files within a specific folder

		Get-AzureDataLakeChildItem -Path $folderPath

* **Get details** of a specific file or folder

		Get-AzureDataLakeItem -Path $path

* **Test existence** of a specific file or folder

		Test-AzureDataLakeItem -Path $path

* **Create** a new file or folder

        New-AzureDataLakeItem -Path $filePath

* **Upload** a specific file or folder from local machine to Data Lake

        Copy-AzureDataLakeItem -Path $localPath -Destination $remotePath

* **Download** a specific file or folder from Data Lake to local machine

        Copy-AzureDataLakeItem -Path $remotePath -Destination $localPath

* **Delete** a specific file or folder

        Delete-AzureDataLakeItem -Path $filePath

* **Rename or move** a specific file or folder

        Move-AzureDataLakeItem -Path $filePath -Destination $filePath

* **Concatenate** (destructively) two or more files

		Join-AzureDataLakeItem -Paths $filePaths -Destination $filePath

* **Append** content to a specific file

        Add-AzureDataLakeItemContent -Path $filePath

* **Read** content of a specific file

        Get-AzureDataLakeItemContent -Path $filePath


#### File/folder access info

* **Get** access settings for a specific file or folder

    * Get the user owner of a file or folder
 
            Get-AzureDataLakeItemAccess -Path $path -UserOwner

    * Get the group owner of a file or folder
  
            Get-AzureDataLakeItemAccess -Path $path -GroupOwner

    * Get the permissions of a file or folder  (octal format)
  
            Get-AzureDataLakeItemAccess -Path $path -Permissions

    * Get the access control list of a file or folder

            Get-AzureDataLakeItemAccess -Path $path -ACL

* **Add or modify** an entry to the access control list of a specific file or folder

    * Add/modify a user entry

            $username = "saveen@contoso.com"
            $permissions = "rwx"
    
            $accessControlList = Get-AzureDataLakeItemAccess -Path $path -ACL
            $accessControlList.Users[$username] = $permissions
    
            Set-AzureDataLakeItemAccess -Path $path -ACL $accessControlList

    * Add/modify a group entry

            $groupname = "bobs.directs@contoso.com"
            $permissions = "r--"
    
            $accessControlList = Get-AzureDataLakeItemAccess -Path $path -ACL
            $accessControlList.Groups[$groupname] = $permissions
    
            Set-AzureDataLakeItemAccess -Path $path -ACL $accessControlList

* **Remove** an entry from the access control list of a specific file or folder

    * Remove a user entry

            $username = "saveen@contoso.com"

            $accessControlList = Get-AzureDataLakeItemAccess -Path $path -ACL
            $accessControlList.Users.Remove($username)

            Set-AzureDataLakeItemAccess -Path $path -ACL $accessControlList

    * Remove a group entry

            $groupname = "bobs.directs@contoso.com"

            $accessControlList = Get-AzureDataLakeItemAccess -Path $path -ACL
            $accessControlList.Groups.Remove($groupname)

            Set-AzureDataLakeItemAccess -Path $path -ACL $accessControlList

* **Test** whether an entry is in the access control list of a file or folder

        $username = "saveen@contoso.com"

        $accessControlList = Get-AzureDataLakeItemAccess -Path $path -ACL

        $accessControlList.Users.Contains($username)

* **Clear** the access control list of a file or folder

        Set-AzureDataLakeItemAccess -Path $path -ACL $null

* **Copy** the access control list of one file or folder to another

        $accessControlList = Get-AzureDataLakeItemAccess -Path $firstPath -ACL

        Set-AzureDataLakeItemAccess -Path $secondPath -ACL $accessControlList

* **Replace** an entry in an access control list with another entry

        $oldUsername = "billg@contoso.com"
        $newUsername = "alan@contoso.com" 
       
        $accessControlList = Get-AzureDataLakeItemAccess -Path $firstPath -ACL
        $accessControlList.Users[$newUsername] = $accessControlList.Users[$oldUsername]
        $accessControlList.Users.Remove($newUsername)
    
        Set-AzureDataLakeItemAccess -Path $secondPath -ACL $accessControlList

* **Get** permissions of an access control list entry

        $username = "saveen@contoso.com"

        $accessControlList = Get-AzureDataLakeItemAccess -Path $path -ACL

        $accessControlList.Users[$username]

* **Set** access settings for a specific file or folder
    * Set the user owner of a file or folder

            $username = "emma@contoso.com"

            Set-AzureDataLakeItemAccess -Path $path -UserOwner $username

    * Set the group owner of a file or folder

            $groupname = "emmasdirects@contoso.com"

            Set-AzureDataLakeItemAccess -Path $path -GroupOwner $groupname

    * Set the permissions of a file or folder (symbolic format)

            $permissions = "rwxr-x---"

            Set-AzureDataLakeItemAccess -Path $path -Permissions $permissions

    * Set the permissions of a file or folder  (octal format)

            $permissions = "750"

            Set-AzureDataLakeItemAccess -Path $path -Permissions $permissions


------------

### Useful links

Browse the following pages:

* *(place table of contents here)*