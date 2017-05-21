
# Azure Data Lake PowerShell Quick Start

#### Introduction

If you are familiar with very basic PowerShell and Azure Data Lake, this document will
walk you through some very common and interesting scenarios.

#### Information you Need to use this scripts

The scripts reuse some values so set the following variables as you need

    $subname = Your subscription name
    $subid  = Your subscription id
    $adla = the name of your ADL Analytics account (not its full domain name)
    $adls = the name of your ADL Store account (not its full domain name)

#### Logging in 

Use the Login-AzureRmAccount cmdlet
 
    Login-AzureRmAccount -SubscriptionName $subname 

#### Saving the login information

    Save-AzureRmProfile -Path D:\profile.json  

#### Loading the login information

    Select-AzureRmProfile -Path D:\profile.json 

## Job Monitoring

####   Getting a list of recent jobs (that are active or have ended)

    $jobs = Get-AdlJob -Account $adla

By default the list of jobs issorted on submit time. So the most recently submitted jobs will come first.

The ADLA account remember jobs for 180 days, but this cmdlet by default will return only the first 500. You can -Top parameter to have to retrieve any number of jobs.

    $jobs = Get-AdlJob -Account $adla -Top 10


#### Get all the jobs submitted in the last day (Server-side filtering)

    $d = [DateTime]::Now.AddDays(-1)
    Get-AdlJob -Account $adla -SubmittedAfter $d

####  Get all the jobs submitted in the last 5 days and that successfully completed (Server-side filtering)

    Get-AdlJob -Account datainsightsadhoc -SubmittedAfter (Get-Date).AddDays(-5) -State Ended -Result Succeeded
    
#### Examine details on a single job

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

## Job Analysis

#### ProTip: Add useful calculated properties to job objects

Powershell can dynamically add calculated properties to job objects. The script below adds properties
you may find very useful

    function annotate_job( $j )
    {
        $dic1 = @{
            Label='AUHours';
            Expression={ ($_.DegreeOfParallelism * ($_.EndTime-$_.StartTime).TotalHours)}}
        $dic2 = @{
            Label='DurationSeconds';
            Expression={ ($_.EndTime-$_.StartTime).TotalSeconds}}
        $dic3 = @{
            Label='DidRun';
            Expression={ ($_.StartTime -ne $null)}}

        $j2 = $j | select *, $dic1, $dic2, $dic3
        $j2
    }

    $jobs_annotated = $jobs | %{ annotate_job( $_ ) }


#### Find all the submitters and the numbers of jobs they have submitted

    $jobs | Group-Object Submitter | Select -Property Count,Name

#### Analyze the jobs by Result

    $jobs | Group-Object Result | Select -Property Count,Name

#### Analyze the jobs by State

    $jobs | Group-Object State | Select -Property Count,Name

#### Filter jobs to once submitted in the last 24 hours (Client-side filtering)

    $upperdate = Get-Date
    $lowerdate = $upperdate.AddHours(-24)
    
    $jobs | Where-Object { $_.EndTime -ge $lowerdate }
    
#### Filter jobs to once ended in the last 24 hours (Client-side filtering)

    $upperdate = Get-Date
    $lowerdate = $upperdate.AddHours(-24)
    
    $jobs | Where-Object { $_.SubmitTime -ge $lowerdate }

#### Find all Failed jobs (Server-side filtering)

    Get-AdlJob -Account $adla -State Ended -Result Succeeded


#### Find all Failed jobs (Server-side filtering)

    Get-AdlJob -Account $adla -State Ended -Result Failed

#### Analyze jobs by the DegreeOfParallelism

    $jobs | Group-Object DegreeOfParallelism | Select -Property Count,Name

#### Who had failed jobs

    $failed_jobs = Get-AdlJob -Account $adla -State Ended -Result Failed
    $failed_jobs | Group-Object Submitter | Select -Property Count,Name

#### Find all the failed jobs that actually started 

A job might fail at compile time - and so it never starts. Let's look at the failed
jobs that actually started running and then failed.

    $failed_jobs | Where-Object { $_.StartTime -ne $null }


## Account Management

#### Test if Accounts Exist

    Test-AdlAnalyticsAccount -Name $adls
    Test-AdlStore -Name $adls


## Account Configuration

#### List the linked ADLS Stores or Blob Storage Accounts 

    Get-AdlAnalyticsDataSource -Account $adla

#### Get the default ADLS Store for an ADLA account

    Get-AdlAnalyticsDataSource -Account $adla  | ? { $_.IsDefault } 


## Getting data into and out of Data Lake Store

#### Download a folder recursively 

    Export-AdlStoreItem -Account $adls -Path /sourcefolder -Destination D:\destinationfolder -Recurse

#### Import a folder recursively 

    Import-AdlStoreItem -Account $adls -Path d:\sourcefolder -Destination /destinationfolder -Recurse

## U-SQL Catalog operations

#### List U-SQL Databases in an account

    Get-AdlCatalogItem -Account $adla -ItemType Database

#### List Assemblies in an U-SQL Database

    Get-AdlCatalogItem -Account $adla -ItemType Assembly -Path "database"

#### List Tables in a U-SQL Database and schema

    Get-AdlCatalogItem -Account $adla -ItemType Table -Path "database.schema"


#### Check if you are running as an administrator

    function Test-Administrator  
    {  
        $user = [Security.Principal.WindowsIdentity]::GetCurrent();
        $p = New-Object Security.Principal.WindowsPrincipal $user
        $p.IsInRole([Security.Principal.WindowsBuiltinRole]::Administrator)  
    }

## Azure 

#### Find the TenantID for a subscription

    # Using the Subscription Name
    (Get-AzureRmSubscription -SubscriptionName "MySUbName").TenantID

    # Using the Subscription ID
    (Get-AzureRmSubscription -SubscriptionName "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx").TenantID

## General PowerShell Tips

#### More strict error handling

Put this at the top of any scripts. 

    Set-StrictMode -Version 2
    $ErrorActionPreference = "Stop"

#### Check if you are running as an administrator

    function Test-Administrator  
    {  
        $user = [Security.Principal.WindowsIdentity]::GetCurrent();
        $p = New-Object Security.Principal.WindowsPrincipal $user
        $p.IsInRole([Security.Principal.WindowsBuiltinRole]::Administrator)  
    }

#### Time a command

    Measure-Command { Get-ChildItem C:\ }

    Days              : 0
    Hours             : 0
    Minutes           : 0
    Seconds           : 0
    Milliseconds      : 980
    Ticks             : 9807034
    TotalDays         : 1.13507337962963E-05
    TotalHours        : 0.000272417611111111
    TotalMinutes      : 0.0163450566666667
    TotalSeconds      : 0.9807034
    TotalMilliseconds : 980.7034

## Tutorials

* [Tutorial: get started with Azure Data Lake Analytics using Azure PowerShell](https://docs.microsoft.com/en-us/azure/data-lake-analytics/data-lake-analytics-get-started-powershell)
* [Tutorial: get started with Azure Data Lake Store using Azure PowerShell](https://docs.microsoft.com/en-us/azure/data-lake-store/data-lake-store-get-started-powershell)

## Reference Materials

* [Azure Data Lake Analytics Cmdlets](https://docs.microsoft.com/en-us/powershell/resourcemanager/azurerm.datalakeanalytics/v3.1.0/azurerm.datalakeanalytics?redirectedfrom=msdn)
* [Azure Data Lake Store Cmdlets](https://docs.microsoft.com/en-us/powershell/resourcemanager/azurerm.datalakestore/v3.1.0/azurerm.datalakestore)

