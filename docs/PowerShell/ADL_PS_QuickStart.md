
# ADL PowerShell Quick Start

## Introduction

If you are familiar with very basic PowerShell and Azure Data Lake, this document will
walk you through some very common and interesting scenarios.

## Information you Need to use this scripts

The scripts reuse some values so set the following variables as you need

    $subname = Your subscription name
    $subid  = Your subscription id
    $tenant = Your tenantid. Example: contoso.onmicrosoft.com
    $adla = the name of your ADL Analytics account (not its full domain name)
    $adls = the name of your ADL Store account (not its full domain name)


## Logging in 

Use the Login-AzureRmAccount cmdlet
 
    Login-AzureRmAccount -SubscriptionName $subname -TenantId $tenant

## Getting a list of all the jobs suibmitted in the last day

    $jobs = Get-AdlJob -Account $adla -SubmittedAfter ([DateTime]::Now.AddDays(-1))

##  Getting a list of all the jobs 

    $jobs = Get-AdlJob -Account $adla

NOTE: If you have a lot of jobs submitted inthe last 30 days it may take a while for this cmdlet to finish'

## Getting a list of all the jobs suibmitted in the last day

    $jobs = Get-AdlJob -Account $adla -SubmittedAfter ([DateTime]::Now.AddDays(-1))

## How many jobs do we have?

    $jobs.Count

## Look at the first job

    $jobs[0]
    
    JobId               : d0fe8b13-623f-4562-8a98-00890bae4e21
    Name                : ADF-d0fe8b13-623f-4562-8a98-00890bae4e21
    Type                : USql
    Submitter           : saveenr@microsoft.com
    ErrorMessage        :
    DegreeOfParallelism : 3
    Priority            : 100
    SubmitTime          : 12/28/2016 2:50:02 AM +00:00
    StartTime           : 12/28/2016 2:50:44 AM +00:00
    EndTime             : 12/28/2016 2:52:15 AM +00:00
    State               : Ended
    Result              : Succeeded
    LogFolder           :
    LogFilePatterns     :
    StateAuditRecords   :
    Properties          :

## Find all the submitters and the numbers of jobs they have submitted

    $jobs | Group-Object Submitter | Select -Property Count,Name

## Analyze the jobs by Result

    $jobs | Group-Object Result | Select -Property Count,Name


## Analyze the jobs by Result

    $jobs | Group-Object State | Select -Property Count,Name

## Filter jobs to once submitted in the last 24 hours

    $upperdate = Get-Date
    $lowerdate = $upperdate.AddHours(-24)
    
    $jobs | Where-Object { $_.EndTime -ge $lowerdate }
    
## Filter jobs to once ended in the last 24 hours

    $upperdate = Get-Date
    $lowerdate = $upperdate.AddHours(-24)
    
    $jobs | Where-Object { $_.SubmitTime -ge $lowerdate }
    
## Find all Failed  jobs

    $jobs | Where-Object { $_.Result -eq "Failed" }

## Who had failed jobs

    $failed_jobs = $jobs | Where-Object { $_.Result -eq "Failed" }
    $failed_jobs | Group-Object Submitter | Select -Property Count,Name

## Find all the failed jobs that actually started 

A job might fail at compile time - and so it never starts. Let's look at the failed
jobs that actually started running and then failed.

    $failed_jobs | Where-Object { $_.StartTime -ne $null }

## Check if you are running as an administrator

    function Test-Administrator  
    {  
        $user = [Security.Principal.WindowsIdentity]::GetCurrent();
        $p = New-Object Security.Principal.WindowsPrincipal $user
        $p.IsInRole([Security.Principal.WindowsBuiltinRole]::Administrator)  
    }
