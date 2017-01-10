Set-StrictMode -Version 2
$ErrorActionPreference = "Stop"

# Login-AzureRmAccount -SubscriptionName <YOURSUBSCRIPTION>
$account = "YOURADLSACCOUNT"

# This script will create three levels of folders each hass a hundred subfolders
$level1 = 1..100
$level2 = 1..100
$level3 = 1..100

# Keep a count of the files
$count = 1

foreach ($a in $level1) 
{
    foreach ($b in $level2) 
    {
        foreach ($c in $level3) 
        {
            $remote_path = "/ManyFiles/" + $a + "/" + $b + "/" + $c + "/" + "data_" + $count + ".csv"

            Write-Host $count, $remote_path

            $value = $a.ToString() + "," + $b.ToString() + "," + $c.ToString() + "," + $count.ToString() 
            New-AdlStoreItem -Account $account -Path $remote_path -Force -Value $value

            $count++
        }
    }
}
