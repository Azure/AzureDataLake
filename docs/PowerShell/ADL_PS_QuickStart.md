
# ADL PowerShell Quick Start

## Introduction

If you are familiar with very basic PowerShell and Azure Data Lake, this document will
walk you through some very common and interesting scenarios.

## Information you Need to use this scripts

The scripts reuse some values so set the following variables as you need

    $subname = Your subscription name
    $subid  = Your subscription id
    $adla = the name of your ADL Analytics account (not its full domain name)
    $adls = the name of your ADL Store account (not its full domain name)


## Logging in 

Use the Login-AzureRmAccount cmdlet
 
    Login-AzureRmAccount -SubscriptionName $subname 

## Getting a list of all the jobs submitted in the last day

The -SubmittedAfter parameter performs server-side filtering.

    Get-AdlJob -Account $adla -SubmittedAfter ([DateTime]::Now.AddDays(-1))

##  Getting a list of all the jobs 

NOTE: If you have a lot of jobs submitted inthe last 30 days it may take a while for this cmdlet to finish'
    
    $jobs = Get-AdlJob -Account $adla

## How many jobs were retrieved

    $jobs.Count

## Examine details on a single job

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

## Analyze the jobs by State

    $jobs | Group-Object State | Select -Property Count,Name

## Filter jobs to once submitted in the last 24 hours (Client-side filtering)

    $upperdate = Get-Date
    $lowerdate = $upperdate.AddHours(-24)
    
    $jobs | Where-Object { $_.EndTime -ge $lowerdate }
    
## Filter jobs to once ended in the last 24 hours (Client-side filtering)

    $upperdate = Get-Date
    $lowerdate = $upperdate.AddHours(-24)
    
    $jobs | Where-Object { $_.SubmitTime -ge $lowerdate }

## Find all Failed  jobs (Server-side filtering)

    Get-AdlJob -Account $adla -State Ended -Result Succeeded


## Find all Failed  jobs (Server-side filtering)

    Get-AdlJob -Account $adla -State Ended -Result Failed

# Analyze jobs by the DegreeOfParallelism

    $jobs | Group-Object DegreeOfParallelism | Select -Property Count,Name

## Who had failed jobs

    $failed_jobs = Get-AdlJob -Account $adla -State Ended -Result Failed
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

## Find the TenantID for a subscription

    # Using the Subscription Name
    (Get-AzureRmSubscription -SubscriptionName "MySUbName").TenantID

    # Using the Subscription ID
    (Get-AzureRmSubscription -SubscriptionName "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx").TenantID
    
## List the linked ADLS Stores or Blob Storage Accounts 

    Get-AdlAnalyticsDataSource -Account $adla

## Get the default ADLS Store

    Get-AdlAnalyticsDataSource -Account $adla  | ? { $_.IsDefault } 

## Test if Accounts Exist

    Test-AdlStore -Name $adls

## List databases in an account

    Get-AdlCatalogItem -Account $adla -ItemType Database

# More Information

## Tutorials

* [Tutorial: get started with Azure Data Lake Analytics using Azure PowerShell](https://docs.microsoft.com/en-us/azure/data-lake-analytics/data-lake-analytics-get-started-powershell)
* [Tutorial: get started with Azure Data Lake Store using Azure PowerShell](https://docs.microsoft.com/en-us/azure/data-lake-store/data-lake-store-get-started-powershell)

## Reference Materials

* [Azure Data Lake Analytics Cmdlets](https://docs.microsoft.com/en-us/powershell/resourcemanager/azurerm.datalakeanalytics/v3.1.0/azurerm.datalakeanalytics?redirectedfrom=msdn)
* [Azure Data Lake Store Cmdlets](https://docs.microsoft.com/en-us/powershell/resourcemanager/azurerm.datalakestore/v3.1.0/azurerm.datalakestore)

