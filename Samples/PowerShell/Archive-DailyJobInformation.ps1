Set-StrictMode -Version 2
$ErrorActionPreference = "Stop"

$startdate = get-date 8/1/2017
$enddate = get-date 8/31/2017
$adla_accountname = "datainsights"
$output_folder = "d:\"

function get-daterange( [datetime] $start, [datetime] $end )
{
    $curdate = $staRT
    while ($curdate -le $end)
    {
        $curdate
        $curdate = $curdate.AddDays(1)        
    }
}

$dates = get-daterange $startdate $enddate

foreach ($date in $dates)
{
    $after = $date
    $before = $date.AddDays(1)
    $top = 10000
    $filename = Join-Path $output_folder ( "jobs_" + $adla_accountname + "_" + $date.ToString("yyyyMMdd") + ".json")
    Write-Host $date " => " $filename

    if ( Test-Path $filename )
    {
        # file exists; skip it
    }
    else
    {
        $jobs = Get-AdlJob -Account $adla_accountname -SubmittedAfter $after -SubmittedBefore $before -top $top
        if ($jobs -ne $null)
        {
            Write-Host "Num jobs" = $jobs.Count
            $json = $jobs | ConvertTo-Json
            $json | Out-File -FilePath $filename
        }
    }
}
