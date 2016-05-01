param([string]$account) #Must be the first statement in your script

# First, make sure you are logged into the correct Azure Subscription and Tenant with Login-AzureRmAccount first
#
#     > Login-AzureRmAccount -SubscriptionId "..."
#
# Then, call it like this from the command line
#     
#     > .\RegisterAssemblies.ps1 -account "sandbox"
#



$folder = join-path $PSScriptRoot "..\AzureDiagnosticsExtractors\bin\Debug"
$folder = [System.IO.Path]::GetFullPath($folder)
$dlls = @( 
    "Newtonsoft.Json.dll", 
    "AzureDiagnosticsExtractors.dll", 
    "AzureDiagnostics.dll"
)

$dlls = $dlls | % { join-path $folder $_ }


$dlls

foreach ($dll in $dlls)
{
    if (!(test-path $dll))
    {
        Write-Host "Does not exist: $dll"
    }
    else
    {
        Write-Host "File Exists: $dll"
    }
}

$remote_temp = "/dll_temp";

$adla_account = Get-AzureRmDataLakeAnalyticsAccount -Name $account

Write-Host $adla_account

$adls_name = $adla_account.Properties.DefaultDataLakeStoreAccount

Write-Host $adls_name

$script = ""
$script = $script + "CREATE DATABASE IF NOT EXISTS Diagnostics;`n"


foreach ($dll in $dlls)
{
    $basename = [System.IO.Path]::GetFileName($dll)
    $basename_noext = [System.IO.Path]::GetFileNameWithoutExtension($dll)

    $asm_name = $basename_noext 
    $dest_path = "/dll_temp/" + $basename 
    Import-AzureRmDataLakeStoreItem -AccountName $adls_name -Path $dll -Destination $dest_path -Force

    $full_dest_path = "adl://" + $adls_name + ".azuredatalakestore.net" + $dest_path
    $script = $script + "DROP ASSEMBLY IF EXISTS Diagnostics.[" + $asm_name + "];`n"
    $script = $script + "CREATE ASSEMBLY Diagnostics.[" + $asm_name  + "] FROM `"$full_dest_path`" ;`n"

}



Submit-AzureRmDataLakeAnalyticsJob -Name "RegisterDiagnosticsAssemblies" -AccountName $account -Script $script
