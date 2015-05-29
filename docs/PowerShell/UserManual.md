# User Manual: Azure PowerShell with Data Lake

This guide assumes you have previously followed the steps in the main [Getting Started guide](../GettingStarted.md).

------------

### The Basics

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

    Sample response:

        ...
    
* **List** Data Lake accounts within a specific resource group
    
        Get-AzureDataLakeAccount -ResourceGroupName $resourceGroupName

    Sample response:

        ...
    
* **Get details** of a specific Data Lake account
    
        Get-AzureDataLakeAccount -Name $dataLakeAccountName

    Sample response:

        ...

* **Test existence** of a specific Data Lake account

        Test-AzureDataLakeAccount -Name $dataLakeAccountName

    Sample response:

        False

* **Create** a new Data Lake account

        New-AzureDataLakeAccount `
            -ResourceGroupName $resourceGroupName `
            -Name $dataLakeAccountName `
            -Location "East US 2"

    Sample response:

        True

#### Data operations

* **Get details** of a specific file or folder

    * List all details

    		Get-AzureDataLakeItem -Path $path
    
        Sample response:
    
            Name            Value
            ----            -----
            Type            File
            Name            zebras.txt
            LastModified    2015-08-12 13:02 UTC
            Size            21231
            UserOwner       mahi@contoso.com
            GroupOwner      adl.users@contoso.com
            Permissions     rwxrwx---
            ACL             {Users, Groups}

    * Get file size in bytes
            
            $fileInfo = Get-AzureDataLakeItem -Path $path
            $fileInfo.Size
        
        Sample response
    
            21231

* **List** files within a specific folder

		Get-AzureDataLakeChildItem -Path $folderPath

    Sample response:

        Type     Permission   LastModified           Name                        Size
        ----     ----------   ------------           ----                        ----
        Folder   rwxr-x---    2015-05-28 14:12 UTC   thisFolder              
        File     rwx------    2015-07-31 11:11 UTC   thisBigFileWit   120334212965252
                                                     hLongName.dat 
        File     rwxrw----    2015-08-12 13:02 UTC   zebras.txt                   120

* **Test existence** of a specific file or folder

		Test-AzureDataLakeItem -Path $path

    Sample response:

        False

* **Create** a new file or folder

        New-AzureDataLakeItem -Path $filePath

    Sample response:

        True

* **Upload** a specific file or folder from local machine to Data Lake

        Copy-AzureDataLakeItem -Path $localPath -Destination $remotePath

    Sample response:

        True

* **Download** a specific file or folder from Data Lake to local machine

        Copy-AzureDataLakeItem -Path $remotePath -Destination $localPath

    Sample response:

        True

* **Delete** a specific file or folder

        Delete-AzureDataLakeItem -Path $filePath

    Sample response:

        True

* **Rename or move** a specific file or folder

        Move-AzureDataLakeItem -Path $filePath -Destination $filePath

    Sample response:

        True

* **Concatenate** (destructively) two or more files

		Join-AzureDataLakeItem -Paths $filePaths -Destination $filePath

    Sample response:

        True

* **Append** content to a specific file

        Add-AzureDataLakeItemContent -Path $filePath

    Sample response:

        True

* **Read** content of a specific file

        Get-AzureDataLakeItemContent -Path $filePath

    Sample response:

        abc123

#### File/folder access info

* **Get** access settings for a specific file or folder

    * Get the user owner of a file or folder
 
            Get-AzureDataLakeItemAccess -Path $path -UserOwner

        Sample response:
    
            emma@contoso.com

    * Get the group owner of a file or folder
  
            Get-AzureDataLakeItemAccess -Path $path -GroupOwner

        Sample response:
    
            adl.users@contoso.com

    * Get the permissions of a file or folder  (octal format)
  
            Get-AzureDataLakeItemAccess -Path $path -Permissions

        Sample response:
    
            rwxr-x---

    * Get the access control list of a file or folder

            Get-AzureDataLakeItemAccess -Path $path -ACL

        Sample response:
    
            Name                           Value
            ----                           -----
            Users                          {emma@contoso.com, saveen@contoso.com}
            Groups                         {}

        * See the users in the access control list

                $accessControlList = Get-AzureDataLakeItemAccess -Path $path -ACL
                $accessControlList.Users
    
            Sample response:
        
                Name                           Value
                ----                           -----
                mahi@contoso.com               rw-
                saveen@contoso.com             r--

* **Add or modify** an entry to the access control list of a specific file or folder

    * Add/modify a user entry

            $username = "saveen@contoso.com"
            $permissions = "rwx"
    
            $accessControlList = Get-AzureDataLakeItemAccess -Path $path -ACL
            $accessControlList.Users[$username] = $permissions
    
            Set-AzureDataLakeItemAccess -Path $path -ACL $accessControlList

        Sample response:
    
            True

    * Add/modify a group entry

            $groupname = "bobs.directs@contoso.com"
            $permissions = "r--"
    
            $accessControlList = Get-AzureDataLakeItemAccess -Path $path -ACL
            $accessControlList.Groups[$groupname] = $permissions
    
            Set-AzureDataLakeItemAccess -Path $path -ACL $accessControlList

        Sample response:
    
            True

* **Remove** an entry from the access control list of a specific file or folder

    * Remove a user entry

            $username = "saveen@contoso.com"

            $accessControlList = Get-AzureDataLakeItemAccess -Path $path -ACL
            $accessControlList.Users.Remove($username)

            Set-AzureDataLakeItemAccess -Path $path -ACL $accessControlList

        Sample response:
    
            True

    * Remove a group entry

            $groupname = "bobs.directs@contoso.com"

            $accessControlList = Get-AzureDataLakeItemAccess -Path $path -ACL
            $accessControlList.Groups.Remove($groupname)

            Set-AzureDataLakeItemAccess -Path $path -ACL $accessControlList

        Sample response:
    
            True

* **Test** whether an entry is in the access control list of a file or folder

        $username = "saveen@contoso.com"

        $accessControlList = Get-AzureDataLakeItemAccess -Path $path -ACL

        $accessControlList.Users.Contains($username)

    Sample response:

        False

* **Clear** the access control list of a file or folder

        Set-AzureDataLakeItemAccess -Path $path -ACL $null

    Sample response:

        True

* **Set** access settings for a specific file or folder
    * Set the user owner of a file or folder

            $username = "emma@contoso.com"

            Set-AzureDataLakeItemAccess -Path $path -UserOwner $username

        Sample response:
    
            True

    * Set the group owner of a file or folder

            $groupname = "emmasdirects@contoso.com"

            Set-AzureDataLakeItemAccess -Path $path -GroupOwner $groupname

        Sample response:
    
            True

    * Set the permissions of a file or folder (symbolic format)

            $permissions = "rwxr-x---"

            Set-AzureDataLakeItemAccess -Path $path -Permissions $permissions

        Sample response:
    
            True

    * Set the permissions of a file or folder  (octal format)

            $permissions = "750"

            Set-AzureDataLakeItemAccess -Path $path -Permissions $permissions

        Sample response:
    
            True

* **Copy** the access control list of one file or folder to another

        $accessControlList = Get-AzureDataLakeItemAccess -Path $firstPath -ACL

        Set-AzureDataLakeItemAccess -Path $secondPath -ACL $accessControlList

    Sample response:

        True

* **Replace** an entry in an access control list with another entry

        $oldUsername = "billg@contoso.com"
        $newUsername = "alan@contoso.com" 
       
        $accessControlList = Get-AzureDataLakeItemAccess -Path $firstPath -ACL
        $accessControlList.Users[$newUsername] = $accessControlList.Users[$oldUsername]
        $accessControlList.Users.Remove($newUsername)
    
        Set-AzureDataLakeItemAccess -Path $secondPath -ACL $accessControlList

    Sample response:

        True

* **Get** the permissions of an access control list entry

        $username = "saveen@contoso.com"

        $accessControlList = Get-AzureDataLakeItemAccess -Path $path -ACL

        $accessControlList.Users[$username]

    Sample response:

        r-x


------------

### Useful links

Browse the following pages:

* *(place table of contents here)*