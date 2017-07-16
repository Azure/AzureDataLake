
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

#### List of jobs

```
$jobs = Get-AdlJob -Account $adla
```

#### List a specific number of jobs

By default the list of jobs is sorted on submit time. So the most recently submitted jobs will come first. By default, The ADLA account remembers jobs for 180 days, but the Ge-AdlJob  cmdlet by default will return only the first 500. Use -Top parameter to list a specific number of jobs.

```
$jobs = Get-AdlJob -Account $adla -Top 10
```

#### List jobs based on the value of job property

List  jobs submitted in the last day

```
$d = [DateTime]::Now.AddDays(-1)
Get-AdlJob -Account $adla -SubmittedAfter $d
```

List jobs submitted in the last 5 days and that successfully completed.

```
$d = (Get-Date).AddDays(-5)
Get-AdlJob -Account $adla -SubmittedAfter $d -State Ended -Result Succeeded
```

List Failed jobs

```
Get-AdlJob -Account $adla -State Ended -Result Succeeded
```

List Failed jobs 

```
Get-AdlJob -Account $adla -State Ended -Result Failed
```

## Filtering a list of jobs

Once you have a list of jobs in your current PowerShell session. You can use normal PowerShell cmdlets to filter that last.

Filter a list of jobs to those submitted in the last 24 hours

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

Filter a list of jobs tho those that actually started. A job might fail at compile time - and so it never starts. Let's look at the failed
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

# By Result
$jobs | Group-Object Result | Select -Property Count,Name

# By State
$jobs | Group-Object State | Select -Property Count,Name

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

$jobs = $jobs | %{ annotate_job( $_ ) }
```

### Example Scenarios

First get a list of jobs and annotate them with extra properties.

```
$jobs = Get-AdlJob -Account $adla -Top 10
$jobs = $jobs | %{ annotate_job( $_ ) }
```



## Account Management

#### Test if an account exists

```
Test-AdlAnalyticsAccount -Name $adls
Test-AdlStore -Name $adls
```

## Account configuration

#### List the linked ADLS Stores or Blob Storage accounts 

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

## Scenarios

### get basic information about at ADLA account

Given an acount name, the following code looks up some basic information about the account

```
$adla_acct = Get-AdlAnalyticsAccount -Name "saveenrdemoadla"
$adla_name = $adla_acct.Name
$adla_subid = $adla_acct.Id.Split("/")[2]
$adla_sub = Get-AzureRmSubscription -SubscriptionId $adla_subid
$adla_subname = $adla_sub.Name
$adla_defadls_datasource = Get-AdlAnalyticsDataSource -Account $adla_name  | ? { $_.IsDefault } 
$adla_defadlsname = $adla_defadls_datasource.Name

Write-Host "ADLA Account Name" $adla_name
Write-Host "Subscription Id" $adla_subid
Write-Host "Subscription Name" $adla_subname
Write-Host "Defautl ADLS Store" $adla_defadlsname
Write-Host 

Write-Host '$subname' " = ""$adla_subname"" "
Write-Host '$subid' " = ""$adla_subid"" "
Write-Host '$adla' " = ""$adla_name"" "
Write-Host '$adls' " = ""$adla_defadlsname"" "
```

## U-SQL Catalog operations

#### List items in the U-SQL catalog

```
# Listing databases
Get-AdlCatalogItem -Account $adla -ItemType Database

# Listing assemblies
Get-AdlCatalogItem -Account $adla -ItemType Assembly -Path "database"

# Listing Tables
Get-AdlCatalogItem -Account $adla -ItemType Table -Path "database.schema"
```


#### Scenarios

List all the assemblies in all the databases in an ADLA Account

```
$dbs = Get-AdlCatalogItem -Account $adla -ItemType Database

foreach ($db in $dbs)
{
    $asms = Get-AdlCatalogItem -Account $adla -ItemType Assembly -Path $db.Name

    foreach ($asm in $asms)
    {
        $asmname = "[" + $db.Name + "].[" + $asm.Name + "]"
        Write-Host $asmname
    }
}
```

## Useful snippets

#### Check if you are running as an administrator

```
function Test-Administrator  
{  
    $user = [Security.Principal.WindowsIdentity]::GetCurrent();
    $p = New-Object Security.Principal.WindowsPrincipal $user
    $p.IsInRole([Security.Principal.WindowsBuiltinRole]::Administrator)  
}
```

#### Find the TenantID for a subscription

```
# Using the Subscription Name
(Get-AzureRmSubscription -SubscriptionName "MySUbName").TenantID

# Using the Subscription ID
(Get-AzureRmSubscription -SubscriptionName "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx").TenantID
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

