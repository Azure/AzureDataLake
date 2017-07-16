
# Azure Data Lake PowerShell Quick Start

## Introduction

If you are familiar with very basic PowerShell and Azure Data Lake, this document will
walk you through some very common and interesting scenarios.

### Information you Need to use this scripts

The scripts reuse some values so set the following variables as you need

```
$subname = Your subscription name
$subid  = Your subscription id
$adla = the name of your ADL Analytics account (not its full domain name)
$adls = the name of your ADL Store account (not its full domain name)
```

## Logging in 

Use the Login-AzureRmAccount cmdlet
 
```
Login-AzureRmAccount -SubscriptionName $subname 
```

### Saving the login information

```
Save-AzureRmProfile -Path D:\profile.json  
```

### Loading the login information

```
Select-AzureRmProfile -Path D:\profile.json 
```

## Basic Job mnonitoring

####   Getting a list of jobs

```
$jobs = Get-AdlJob -Account $adla
```

#### Controlling how many jobs to list

By default the list of jobs issorted on submit time. So the most recently submitted jobs will come first. By default, The ADLA account remembers jobs for 180 days, but the Ge-AdlJob  cmdlet by default will return only the first 500. You can -Top parameter to have to retrieve any number of jobs.

```
$jobs = Get-AdlJob -Account $adla -Top 10
```

#### Listing jobs based on a condition

Get all the jobs submitted in the last day

```
$d = [DateTime]::Now.AddDays(-1)
Get-AdlJob -Account $adla -SubmittedAfter $d
```

Get all the jobs submitted in the last 5 days and that successfully completed.

```
Get-AdlJob -Account datainsightsadhoc -SubmittedAfter (Get-Date).AddDays(-5) -State Ended -Result Succeeded
```

Find all Failed jobs

```
Get-AdlJob -Account $adla -State Ended -Result Succeeded
```

Find all Failed jobs 

```
Get-AdlJob -Account $adla -State Ended -Result Failed
```

## Filtering a list of jobs

Filter a list of jobs to those  submitted in the last 24 hours

```
$upperdate = Get-Date
$lowerdate = $upperdate.AddHours(-24)
$jobs | Where-Object { $_.EndTime -ge $lowerdate }
```

Filter a list of jobs to those ended in the last 24 hours

```
$upperdate = Get-Date
$lowerdate = $upperdate.AddHours(-24)
$jobs | Where-Object { $_.SubmitTime -ge $lowerdate }
```

Filter a list of jobs tho those that actually started 

A job might fail at compile time - and so it never starts. Let's look at the failed
jobs that actually started running and then failed.

```
$jobs | Where-Object { $_.StartTime -ne $null }
```

## Analyzing a list of jobs

Once you have a list of jobs in your PowerShell session, you can further analyze the list. The `Group-Object` cmdlet is very useful in these cases.

Find all the submitters and the numbers of jobs they have submitted

# Analyzing jobs by Submitter, Result, State, and DegreeOfParallelism

```
# By Submitter
$jobs | Group-Object Submitter | Select -Property Count,Name
```

```
# By Result
$jobs | Group-Object Result | Select -Property Count,Name
```

```
# By State
$jobs | Group-Object State | Select -Property Count,Name
```

```
# By DegreeOfParallelism
$jobs | Group-Object DegreeOfParallelism | Select -Property Count,Name
```


### Additional Examples


```
# Analysis of  failed jobs
$jobs = Get-AdlJob -Account $adla -State Ended -Result Failed -Top 1000
$jobs | Group-Object Submitter | Select -Property Count,Name
```



### ProTip: Add useful calculated properties to job objects

Powershell can dynamically add calculated properties to job objects. The script below adds properties
you may find very useful

```
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
```

## Account Management

#### Test if Accounts Exist

```
Test-AdlAnalyticsAccount -Name $adls
Test-AdlStore -Name $adls
```

## Account Configuration

#### List the linked ADLS Stores or Blob Storage Accounts 

```
Get-AdlAnalyticsDataSource -Account $adla
```

#### Get the default ADLS Store for an ADLA account

```
Get-AdlAnalyticsDataSource -Account $adla  | ? { $_.IsDefault } 
```


## Getting data into and out of Data Lake Store

#### Download a folder recursively 

```
Export-AdlStoreItem -Account $adls -Path /sourcefolder -Destination D:\destinationfolder -Recurse
```

#### Import a folder recursively 

```
Import-AdlStoreItem -Account $adls -Path d:\sourcefolder -Destination /destinationfolder -Recurse
```

## U-SQL Catalog operations

#### List U-SQL Databases in an account

```
Get-AdlCatalogItem -Account $adla -ItemType Database
```

#### List Assemblies in an U-SQL Database

```
Get-AdlCatalogItem -Account $adla -ItemType Assembly -Path "database"
```

#### List Tables in a U-SQL Database and schema

```
Get-AdlCatalogItem -Account $adla -ItemType Table -Path "database.schema"
```


#### Check if you are running as an administrator

```
function Test-Administrator  
{  
    $user = [Security.Principal.WindowsIdentity]::GetCurrent();
    $p = New-Object Security.Principal.WindowsPrincipal $user
    $p.IsInRole([Security.Principal.WindowsBuiltinRole]::Administrator)  
}
```

## Azure 

#### Find the TenantID for a subscription

```
# Using the Subscription Name
(Get-AzureRmSubscription -SubscriptionName "MySUbName").TenantID

# Using the Subscription ID
(Get-AzureRmSubscription -SubscriptionName "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx").TenantID
```

## General PowerShell Tips

#### More strict error handling

Put this at the top of any scripts. 

```
Set-StrictMode -Version 2
$ErrorActionPreference = "Stop"
```

#### Check if you are running as an administrator

```
function Test-Administrator  
{  
    $user = [Security.Principal.WindowsIdentity]::GetCurrent();
    $p = New-Object Security.Principal.WindowsPrincipal $user
    $p.IsInRole([Security.Principal.WindowsBuiltinRole]::Administrator)  
}

Test-Administrator
```

#### Time a command

```
Measure-Command { Get-ChildItem C:\ }
```

## Tutorials

* [Tutorial: get started with Azure Data Lake Analytics using Azure PowerShell](https://docs.microsoft.com/en-us/azure/data-lake-analytics/data-lake-analytics-get-started-powershell)
* [Tutorial: get started with Azure Data Lake Store using Azure PowerShell](https://docs.microsoft.com/en-us/azure/data-lake-store/data-lake-store-get-started-powershell)

## Reference Materials

* [Azure Data Lake Analytics Cmdlets](https://docs.microsoft.com/en-us/powershell/resourcemanager/azurerm.datalakeanalytics/v3.1.0/azurerm.datalakeanalytics?redirectedfrom=msdn)
* [Azure Data Lake Store Cmdlets](https://docs.microsoft.com/en-us/powershell/resourcemanager/azurerm.datalakestore/v3.1.0/azurerm.datalakestore)

