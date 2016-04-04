
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
 
    Login-AzureRmAccount 

Specify which Subscription you want to work in

    Set-AzureRmContext -SubscriptionName $subname 

##  Getting a list of all the jobs

    $jobs = Get-AzureRmDataLakeAnalyticsJob -Account $adla


## How many jobs do we have?

    $jobs.Count

## Look at the first job

    $jobs[0]

That didn't print much let's get all the fields

    $jobs[0] | select *

This prints:

    DegreeOfParallelism : 1
    EndTime             : 1/14/2016 10:24:26 PM +00:00
    ErrorMessage        : 
    JobId               : e2fb6818-2393-44f7-91eb-0002b86724e6
    Name                : ADF-e2fb6818-2393-44f7-91eb-0002b86724e6
    Priority            : 100
    Properties          : 
    Result              : Succeeded
    StartTime           : 1/14/2016 10:23:32 PM +00:00
    State               : Ended
    StateAuditRecords   : {}
    Submitter           : saveenr@microsoft.com
    SubmitTime          : 1/14/2016 10:23:02 PM +00:00
    Type                : USql


## Find all the submitters and the numbers of jobs they have submitted

    $jobs | Group-Object Submitter | Select -Property Count,Name

## Analyze the jobs by Result

    $jobs | Group-Object Result | Select -Property Count,Name


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

## Find all the failed jbos that actually started 

    $failed_jobs | Where-Object { $_.StartTime -ne $null }
 
