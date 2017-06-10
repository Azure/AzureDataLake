param
(
    [Parameter(Mandatory=$true)]
    [string] $Account,
    [Parameter(Mandatory=$true)]
    [string] $Path,
    [switch]$GetNames
)

$ObjectIdToFriendlyNameMap = @{}

function getObjectIdFriendlyName
{
    param
    (
        [Parameter(Mandatory=$true)]
        [string] $ObjectId
    )

    if ($ObjectIdToFriendlyNameMap.ContainsKey($ObjectId))
    {
        return $ObjectIdToFriendlyNameMap[$ObjectId]
    }
    else
    {
        $displayName = $null;
        
        try
        {
            $displayName = (Get-AzureRmADUser -ObjectId $ObjectId)[0].DisplayName;
        }
        catch {}

        if ($displayName -eq $null)
        {
            try
            {
                $displayName = (Get-AzureRmADGroup -ObjectId $ObjectId)[0].DisplayName;
            }
            catch {}
        }

        if ($displayName -eq $null)
        {
            try
            {
                $displayName = (Get-AzureRmADServicePrincipal -ObjectId $ObjectId)[0].DisplayName;
            }
            catch {}
        }

        if ($displayName -eq $null)
        {
            $displayName = $ObjectId;
        }

        $ObjectIdToFriendlyNameMap[$ObjectId] = $displayName;

        return $displayName
    }
}

function getACL
{
    param
    (
        [Parameter(Mandatory=$true)]
        [string] $Account,
        [Parameter(Mandatory=$true)]
        [string] $Path,
        [Parameter(Mandatory=$true)]
        [bool] $GetNames = $false
    )

    Write-Verbose -Message "Checking $Path"
    
    $aclEntries = Get-AzureRmDataLakeStoreItemAclEntry -Account $Account -Path $Path
    
    $accessAclEntries = $aclEntries | ? { $_.Scope -eq "Access" -and ![String]::IsNullOrEmpty($_.Id) }
    $defaultAclEntries = $aclEntries | ? { $_.Scope -eq "Default" -and ![String]::IsNullOrEmpty($_.Id) }
    
    if ($GetNames)
    {
        $accessAclEntryDisplayNames = $accessAclEntries | % { getObjectIdFriendlyName -ObjectId $_.Id }
        $defaultAclEntryDisplayNames = $defaultAclEntries | % { getObjectIdFriendlyName -ObjectId $_.Id }

        return @([pscustomobject]@{Path=$Path;Type="Access";Count=$accessAclEntries.Count;DisplayNames=$accessAclEntryDisplayNames}, `
                 [pscustomobject]@{Path=$Path;Type="Default";Count=$defaultAclEntries.Count;DisplayNames=$defaultAclEntryDisplayNames})
    }
    else
    {
        return @([pscustomobject]@{Path=$Path;Type="Access";Count=$accessAclEntries.Count;DisplayNames=$null}, `
                 [pscustomobject]@{Path=$Path;Type="Default";Count=$defaultAclEntries.Count;DisplayNames=$null})
    }

    
    
}

function enumerate
{
    param
    (
        [Parameter(Mandatory=$true)]
        [string] $Account,
        [Parameter(Mandatory=$true)]
        [string] $Path,
        [Parameter(Mandatory=$true)]
        [bool] $GetNames = $false
    )
    
    $returnList = @()

    $itemList = Get-AzureRMDataLakeStoreChildItem -Account $Account -Path $Path

    foreach($item in $itemList)
    {
        $pathToSet = Join-Path -Path $Path -ChildPath $item.PathSuffix
        $pathToSet = $pathToSet.Replace("\", "/");

        $returnList += getACL -Account $Account -Path $pathToSet -GetNames $GetNames
        
        if ($item.Type -ieq "DIRECTORY")
        {
            enumerate -Account $Account -Path $pathToSet -GetNames $GetNames
        }
    }

    return $returnList
}

# This script assumes the following:
# 1. The Azure PowerShell environment is installed
# 2. The current session has already run "Login-AzureRMAccount" with a user account that has permissions to the specified ADLS account
try
{   
    Write-Warning "Please do not close this PowerShell window. Output will be provided at the end."
    Write-Warning "To see progress, run this cmdlet with the -Verbose flag."
    Write-Warning "To see friendly names for your ACLs, run this cmdlet with the -GetNames flag. This will slow down the script."

    $returnList = getACL -Account $Account -Path $Path -GetNames $GetNames

    $returnList += enumerate -Account $Account -Path $Path -GetNames $GetNames

    return ($returnList | Sort-Object -Property Count -Descending)
}
catch
{
    Write-Error "Operation failed with the following error: $($error[0])"
}