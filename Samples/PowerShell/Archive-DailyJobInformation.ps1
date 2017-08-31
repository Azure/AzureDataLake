Set-StrictMode -Version 2
$ErrorActionPreference = "Stop"

$startdate = get-date 8/1/2017
$enddate = get-date 8/2/2017
$adla_accountname = "datainsights"
$output_folder = "D:\jobhistory"


function get-daterange( [datetime] $start, [datetime] $end )
{
    $curdate = $staRT
    while ($curdate -le $end)
    {
        $curdate
        $curdate = $curdate.AddDays(1)        
    }
}

function get-dailyfilename( [string] $account, [string] $folder, [datetime] $date )
{
    $date = $date.Date
    $filename = Join-Path $folder ( "jobs_" + $account + "_" + $date.ToString("yyyyMMdd") + ".clixml")
    $filename
}

function Export-AdlJobHistory( [string] $account, [datetime] $start, [datetime] $end, [string] $folder, [bool] $overwrite=$false)
{
    $dates = get-daterange $startdate $enddate

    foreach ($date in $dates)
    {
        $after = $date
        $before = $date.AddDays(1)
        $top = 10000
        $filename = get-dailyfilename $account $folder $date
        Write-Host $date " => " $filename

        if ( Test-Path $filename )
        {
            if ($overwrite)
            {
                Remove-Item $filename
            }
        }


        if ( !(Test-Path $filename) )
        {
            $jobs = Get-AdlJob -Account $account -SubmittedAfter $after -SubmittedBefore $before -top $top
            if ($jobs -ne $null)
            {
                Write-Host "Num jobs" = $jobs.Count
                $jobs | Export-Clixml -Path $filename
            }
        }
    }

}


Export-AdlJobHistory -account $adla_accountname -start $startdate -end $enddate -folder $output_folder -overwrite $false


function Import-AdlJobHistory( [string] $account, [datetime] $start, [datetime] $end, [string] $folder)
{
    $items = New-Object System.Collections.Generic.List[System.Object]

    $dates = get-daterange $startdate $enddate
    foreach ($date in $dates)
    {
        
        $filename = get-dailyfilename $account $folder $date

        Write-Host $date " => " $filename

        if ( Test-Path $filename )
        {
            $jobs = Import-Clixml $filename
            $items.AddRange($jobs)
        }

    }

    $items

}

$jobs = Import-AdlJobHistory -account $adla_accountname -start $startdate -end $enddate -folder $output_folder 

