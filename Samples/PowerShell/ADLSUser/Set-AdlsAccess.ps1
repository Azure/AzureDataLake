<#
.SYNOPSIS
Recursively sets permissions on a Data Lake Store filesystem, at the specified path.

.DESCRIPTION
For -SetAclEntry, this script recursively adds/modifies an Access Control List (ACL) entry at the specified path for (1) a given "User" or service 
principal, (2) a given "Group", or (3) "Other".
For -SetOwner, this script recursively sets the user or group owner at the specified path for (1) a given "User" or service principal or (2) a given "Group".

.PARAMETER Account
The name of the Data Lake Store account.

.PARAMETER EntityId
The Azure AD Object ID of the specified user or group. This should not be specified for 'other' permissions.
The recommendation to ensure the right user or group is added is to run Get-AzureRmAdUser or 
Get-AzureRmAdGroup and pass in the Object ID returned by that cmdlet.

.PARAMETER EntityType
Indicates if the entity to be added is a User or Group, or if the Other permission is being set.

.PARAMETER SetOwner
Indicates that the user or group owner of the affected files or folders should be modified.

.PARAMETER SetAclEntry
Indicates that ACL entries should be added or modified on the affected files or folders.

.PARAMETER Path
The path to start giving the specified entity permissions. This will also recursively propagate those permissions.

.PARAMETER Permissions
The permissions to give the user, group or other. This can be "All", "ReadExecute", or "None".

.EXAMPLE
$objectId = (Get-AzureRmAdUser -Mail john@contoso.com).Id
Set-AdlsAccess.ps1 -Account contosoadlsaccount -Path / -SetAclEntry -EntityId $objectId -EntityType User -Permissions All

.EXAMPLE
$objectId = (Get-AzureRmAdGroup -Mail securitygroup@contoso.com).Id
Set-AdlsAccess.ps1 -Account -Path / contosoadlsaccount -SetOwner -EntityId $objectId -EntityType User
#>
param
(
    [Parameter(Mandatory=$true)]
    [string] $Account,

    [Parameter(Mandatory=$true)]
    [string] $Path,

    [Parameter(ParameterSetName='SetAclEntry')]
    [switch] $SetAclEntry,

    [Parameter(ParameterSetName='SetOwner')]
    [switch] $SetOwner,

    [Parameter(Mandatory=$false)]
    [string] $EntityId,

    [ValidateSet("User", "Group", "Other")]
    [Parameter(Mandatory=$true)]
    [string] $EntityType,

    [ValidateSet("All", "ReadExecute", "None")]
    [Parameter(Mandatory=$true, ParameterSetName='SetAclEntry')]
    [string] $Permissions
)

function giveaccess
{
    param
    (
        [Parameter(Mandatory=$true)]
        [string] $Account,
        [Parameter(Mandatory=$true)]
        [string] $Path,
        [Parameter(Mandatory=$false)]
        [string] $Id,
        [Parameter(Mandatory=$true)]
        [string] $EntityType,
        [Parameter(Mandatory=$true)]
        [string] $Permissions,
        [Parameter(Mandatory=$false)]
        [switch] $IsDefault = $false
    )
    
    # There is not an easy way to check if the user is part of an existing security group with permissions, so we are going to need to just add the ACE

    if ($Permissions -ieq "All")
    {
        $perms = "rwx";
    }
    elseif ($Permissions -ieq "ReadExecute")
    {
        $perms = "r-x";
    }
    else
    {
        $perms = "---";
    }

    $aceToAdd = "$entityType`:$Id`:$perms"
    if($isDefault)
    {
        $aceToAdd = @("default:$aceToAdd","$aceToAdd")
    }
    
    Set-AzureRmDataLakeStoreItemAclEntry -Account $Account -Path $Path -Acl $aceToAdd | Out-Null
}

function setacerec
{
    param
    (
        [Parameter(Mandatory=$true)]
        [string] $Account,
        [Parameter(Mandatory=$true)]
        [string] $Path,
        [Parameter(Mandatory=$true)]
        [string] $Permissions,
        [Parameter(Mandatory=$false)]
        [string] $Id,
        [Parameter(Mandatory=$true)]
        [string] $EntityType
    )
    
    $itemList = Get-AzureRMDataLakeStoreChildItem -Account $Account -Path $Path;
    foreach($item in $itemList)
    {
        $pathToSet = Join-Path -Path $Path -ChildPath $item.PathSuffix;
        $pathToSet = $pathToSet.Replace("\", "/");
        
        if ($item.Type -ieq "FILE")
        {
            # set the ACL without default
            giveaccess -Account $Account -Path $pathToSet -Id $Id -EntityType $EntityType -Permissions $Permissions | Out-Null
        }
        elseif ($item.Type -ieq "DIRECTORY")
        {
            # set permission and recurse on the directory
            giveaccess -Account $Account -Path $pathToSet -Id $Id -EntityType $EntityType -Permissions $Permissions -IsDefault | Out-Null
            setacerec -Account $Account -Path $pathToSet -Permissions $Permissions -Id $Id -EntityType $EntityType | Out-Null
        }
        else
        {
            throw "Invalid path type of: $($item.Type). Valid types are 'DIRECTORY' and 'FILE'"
        }
    }
}

function setownerrec
{
    param
    (
        [Parameter(Mandatory=$true)]
        [string] $Account,
        [Parameter(Mandatory=$true)]
        [string] $Path,
        [Parameter(Mandatory=$false)]
        [string] $Id,
        [Parameter(Mandatory=$true)]
        [string] $EntityType
    )

    $itemList = Get-AzureRMDataLakeStoreChildItem -Account $Account -Path $Path;
    foreach($item in $itemList)
    {
        $pathToSet = Join-Path -Path $Path -ChildPath $item.PathSuffix;
        $pathToSet = $pathToSet.Replace("\", "/");
        
        if ($item.Type -ieq "FILE")
        {
            Set-AzureRmDataLakeStoreItemOwner -Account $Account -Path $Path -Type $EntityType -Id $EntityId | Out-Null
        }
        elseif ($item.Type -ieq "DIRECTORY")
        {
            Set-AzureRmDataLakeStoreItemOwner -Account $Account -Path $Path -Type $EntityType -Id $EntityId | Out-Null
            setownerrec -Account $Account -Path $pathToSet -Id $Id -EntityType $EntityType | Out-Null
        }
        else
        {
            throw "Invalid path type of: $($item.Type). Valid types are 'DIRECTORY' and 'FILE'"
        }
    }
}

# This script assumes the following:
# 1. The Azure PowerShell environment is installed
# 2. The current session has already run "Login-AzureRMAccount" with a user account that has permissions to the specified ADLS account

switch ($PsCmdlet.ParameterSetName) 
{ 
    “SetOwner”  {
        try
        {
            if($EntityType -ieq "other")
	        {
		        throw "SetOwner is not supported when 'Other' EntityType is specified"
	        }
	
	        if($EntityType -ine "other" -and [string]::IsNullOrEmpty($EntityId))
	        {
		        throw "EntityId is required when SetOwner is specified"
	        }
	
	        $ignored = [Guid]::Empty
	        if($EntityType -ine "other" -and !([Guid]::TryParse($EntityId, [ref]$ignored)))
	        {
		        throw "EntityId must be a valid GUID. EntityId value was: $EntityId"
	        }
    
            Write-Host "Request to change owner to entity $EntityId successfully submitted and will propagate over time depending on the size of the folder."
            Write-Host "Please do not close this PowerShell window; otherwise, the propagation will be cancelled."
            
            Set-AzureRmDataLakeStoreItemOwner -Account $Account -Path $Path -Type $EntityType -Id $EntityId -WarningAction SilentlyContinue | Out-Null

            setownerrec -Account $Account -Path $Path -Id $EntityId -EntityType $EntityType -WarningAction SilentlyContinue | Out-Null

            Write-Host "Done."
        }
        catch
        {
            Write-Error "Set owner operation failed with the following error: $($error[0])"
        }
        break
    } 
    “SetAclEntry”  {
        try
        {
            if($EntityType -ieq "other" -and !([string]::IsNullOrEmpty($EntityId)))
	        {
		        throw "EntityId is not supported when modifying permissions for 'Other' entity types"
	        }
	
	        if($EntityType -ine "other" -and [string]::IsNullOrEmpty($EntityId))
	        {
		        throw "EntityId is required when modifying permissions for 'User' and 'Group' entity types"
	        }
	
	        $ignored = [Guid]::Empty
	        if($EntityType -ine "other" -and !([Guid]::TryParse($EntityId, [ref]$ignored)))
	        {
		        throw "EntityId must be a valid GUID. EntityId value was: $EntityId"
	        }
    
            Write-Host "Request to add entity $EntityId successfully submitted and will propagate over time depending on the size of the folder."
            Write-Host "Please do not close this PowerShell window; otherwise, the propagation will be cancelled."

            if($EntityType -ieq "other")
            {
                giveaccess -Account $Account -Path $Path -EntityType $EntityType -Permissions $Permissions -IsDefault -WarningAction SilentlyContinue | Out-Null
            }
            else
            {
                giveaccess -Account $Account -Path $Path -Id $EntityId -EntityType $EntityType -Permissions $Permissions -IsDefault -WarningAction SilentlyContinue | Out-Null
            }

            setacerec -Account $Account -Path $Path -Permissions $Permissions -Id $EntityId -EntityType $EntityType -WarningAction SilentlyContinue | Out-Null

            Write-Host "Done."
        }
        catch
        {
            Write-Error "Set ACL operation failed with the following error: $($error[0])"
        } 
    }
}
