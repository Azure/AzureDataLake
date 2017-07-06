param
(
    [Parameter(Mandatory=$true)]
    [string] $Account,
    [Parameter(Mandatory=$true)]
    [string] $Path
)

function enumerate
{
    param
    (
        [Parameter(Mandatory=$true)]
        [string] $Account,
        [Parameter(Mandatory=$true)]
        [string] $Path
    )
    
    $returnList = @()

    $itemList = Get-AdlStoreChildItem -Account $Account -Path $Path

    foreach($item in $itemList)
    {
        $childPath = Join-Path -Path $Path -ChildPath $item.PathSuffix
        $childPath = $childPath.Replace("\", "/");

        $returnList += $childPath
        
        if ($item.Type -ieq "DIRECTORY")
        {
            enumerate -Account $Account -Path $childPath
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

    return (enumerate -Account $Account -Path $Path)
}
catch
{
    Write-Error "Operation failed with the following error: $($error[0])"
}
