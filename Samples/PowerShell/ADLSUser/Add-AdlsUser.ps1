<#
.SYNOPSIS
Script used to add or remove permissions for a Data Lake Store user or group

.DESCRIPTION
This script adds or removes permissions for the specified user, service principal, or group to access 
files and directories in the specified Data Lake Store account, with the specified level of access.

.PARAMETER Account
The name of the Data Lake Store account to which to add or remove the user or group.

.PARAMETER EntityIdToAdd
The Azure AD Object ID of the user or group to add.
The recommendation to ensure the right user or group is added is to run Get-AzureRMAdUser or 
Get-AzureRMAdGroup and pass in the Object ID returned by that cmdlet.

.PARAMETER EntityType
Indicates if the entity to be added is a user or group.

.PARAMETER Path
The path to start giving the specified entity permissions. This will also recursively propagate those permissions.

.PARAMETER Permissions
The permissions to give the user or group. This can be "All" or "None".

.EXAMPLE
$objectId = (Get-AzureRmAdUser -Mail john@contoso.com).ObjectId
Add-AdlsUser.ps1 -Account myadlsaccount -EntityIdToAdd $objectId -EntityType User
#>
param
(
    [Parameter(Mandatory=$true)]
    [string] $Account,
    [Parameter(Mandatory=$true)]
    [Guid] $EntityIdToAdd,
    [ValidateSet("User", "Group")]
    [Parameter(Mandatory=$true)]
    [string] $EntityType,
    [Parameter(Mandatory=$true)]
    [string] $Path,
    [ValidateSet("All", "None")]
    [Parameter(Mandatory=$true)]
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
        [Parameter(Mandatory=$true)]
        [Guid] $IdToAdd,
        [Parameter(Mandatory=$true)]
        [string] $entityType,
        [Parameter(Mandatory=$true)]
        [string] $permissionToAdd,
        [Parameter(Mandatory=$false)]
        [switch] $isDefault = $false,
        [Parameter(Mandatory=$true)]
        [string] $loginProfilePath
    )
    
    # There is not an easy way to check if the user is part of an existing security group with permissions, so we are going to need to just add the ACE

    $aceToAdd = "$entityType`:$idToAdd`:$permissionToAdd"
    if($isDefault)
    {
        $aceToAdd = "default:$aceToAdd,$aceToAdd"
    }
    
    return Start-Job -ScriptBlock {param ($loginProfilePath, $Account, $Path, $aceToAdd) Select-AzureRMProfile -Path $loginProfilePath | Out-Null; Set-AzureRmDataLakeStoreItemAclEntry -Account $Account -Path $Path -Acl "$aceToAdd"} -ArgumentList $loginProfilePath, $Account, $Path, $aceToAdd
}

function copyacls
{
    param
    (
        [Parameter(Mandatory=$true)]
        [string] $Account,
        [Parameter(Mandatory=$true)]
        [string] $Path,
        [Parameter(Mandatory=$true)]
        [string] $Permissions,
        [Parameter(Mandatory=$true)]
        [Guid] $IdToAdd,
        [Parameter(Mandatory=$true)]
        [string] $entityType,
        [Parameter(Mandatory=$true)]
        [string] $loginProfilePath
    )
    
    $itemList = Get-AzureRMDataLakeStoreChildItem -Account $Account -Path $Path;
    foreach($item in $itemList)
    {
        $pathToSet = Join-Path -Path $Path -ChildPath $item.PathSuffix;
        $pathToSet = $pathToSet.Replace("\", "/");
        
        if ($Permissions -ieq "All")
        {
            $perms = "rwx";
        }
        else 
        {
            $perms = "---";
        }
        
        if ($item.Type -ieq "FILE")
        {
            # set the ACL without default
            giveaccess -Account $Account -Path $pathToSet -IdToAdd $IdToAdd -entityType $entityType -permissionToAdd $perms -loginProfilePath $loginProfilePath | Out-Null
        }
        elseif ($item.Type -ieq "DIRECTORY")
        {
            # set permission and recurse on the directory
            giveaccess -Account $Account -Path $pathToSet -IdToAdd $IdToAdd -entityType $entityType -permissionToAdd $perms -isDefault -loginProfilePath $loginProfilePath  | Out-Null
            copyacls -Account $Account -Path $pathToSet -Permissions $Permissions -IdToAdd $IdToAdd -entityType $entityType -loginProfilePath $loginProfilePath  | Out-Null
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
try
{    
    $executingDir = Split-Path -parent $MyInvocation.MyCommand.Definition
    $executingFile = Split-Path -Leaf $MyInvocation.MyCommand.Definition
    
    $profilePath = Join-Path $env:TEMP "jobprofilesession.tmp"
    if(Test-Path $profilePath)
    {
        Remove-Item $profilePath -Force -Confirm:$false
    }
    
    Save-AzureRMProfile -path $profilePath | Out-Null
    
    Write-Host "Request to add entity: $EntityIdToAdd successfully submitted and will propagate over time depending on the size of the folder."
    Write-Host "Please do not close this PowerShell window; otherwise, the propagation will be cancelled."
    
    Set-AzureRmDataLakeStoreItemAclEntry -Account $Account -Path $Path -AceType $EntityType -Id $EntityIdToAdd -Permissions $Permissions
    Set-AzureRmDataLakeStoreItemAclEntry -Account $Account -Path $Path -AceType $EntityType -Id $EntityIdToAdd -Permissions $Permissions -Default
    copyacls -Account $Account -Path $Path -Permissions $Permissions -IdToAdd $EntityIdToAdd -entityType $EntityType -loginProfilePath $profilePath | Out-Null
}
catch
{
    Write-Error "ACL propagation failed with the following error: $($error[0])"
}
