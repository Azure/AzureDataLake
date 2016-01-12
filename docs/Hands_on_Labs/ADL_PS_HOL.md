# Hands on  Lab: Using PowerShell to interact with Azure Data Lake

# Introduction

In this lab you'll learn how to use the Azure PowerShell cmdlets.

# Prerequisites

To perform this lab you'll need:

- Access to a ADL Analytics account (this is provided for you in the lab)
- Access to a ADL Store account (this is provided for you in the lab)
- A Windows machine with the latest version of Windows PowerShell installed

# Part 0 - Installing and setting up Azure PowerShell

Open a new PowerShell session in administrator mode. Enter the following:

     Install-Module AzureRM
     Install-AzureRM

Close the PowerShell session.

> NOTE: If the above does not work for you, [click here to install through the Web Platform Installer](https://azure.microsoft.com/en-us/documentation/articles/powershell-install-configure/#step-1-install).

Log into Azure PowerShell:

     Login-AzureRmAccount

# Part 1 - Learning about ADL PowerShell cmdlets

For an example, list all Data Lake Analytics accounts available to you:

     Get-AzureRmDataLakeAnalyticsAccount

To list all Data Lake cmdlets available to you:

     Get-Command *DataLake*

To get help on a specific cmdlet:

     help Get-AzureRmDataLakeStoreAccount

To get detailed help, with examples:

* Documentation articles: [ADLA](https://azure.microsoft.com/en-us/documentation/articles/data-lake-analytics-get-started-powershell/) [ADLS](https://azure.microsoft.com/en-us/documentation/articles/data-lake-store-get-started-powershell/)
* MSDN reference material: [ADLA](https://msdn.microsoft.com/en-us/library/mt607124.aspx) [ADLS](https://msdn.microsoft.com/en-us/library/mt607120.aspx)

# Part 2 - List jobs and submit a job

1.  List all jobs that have run on your ADLA account. Use the cmdlet ``Get-AzureRmDataLakeAnalyticsJob``.
2.  Submit a new job to your ADLA account. Use the cmdlet ``Submit-AzureRmDataLakeAnalyticsJob``.
      * Here's the job to submit.

                @searchlog = EXTRACT UserId int, Start DateTime, Region string, Query string, Duration int, Urls string, ClickedUrls string FROM @"/Samples/Data/SearchLog.tsv" USING Extractors.Ts(); OUTPUT @searchlog TO @"/Samples/Output/SearchLog_TestOutput.tsv" USING Outputters.Tsv();

3.  Get the status of the job that you submitted, referencing the job ID returned in Step 2. Use the cmdlet ``Get-AzureRmDataLakeAnalyticsJob``.

# Part 3 - List files and download a file

1.  List all files in the /Samples/Output/ folder. Use the cmdlet ``Get-AzureRmDataLakeStoreChildItem``.
2.  Download the output of the job from Part 2. Use the cmdlet ``Export-AzureRmDataLakestoreItem``.
      * The ADLS path to use is:   /Samples/Output/SearchLog_TestOutput.tsv
