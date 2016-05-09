# Hands-on  Lab: Automating Microsoft Azure Data Lake tasks with Windows PowerShell

# Introduction

In this lab you will learn how to use Azure PowerShell cmdlets to interact with Azure Data Lake Analytics (ADLA) and Azure Data Lake Store (ADLS).

# Getting started


## Prerequisites

Before you can start the lab exercises, you will need various Azure services provisioned for you. Follow the instructions here: [Start](Start.md). 

This process only takes a few minutes. Once your services are configured you can proceed with the lab.


## Setting up your computer

To complete this lab, you will need a computer running Windows with the latest version of Windows PowerShell (v5) installed and Azure PowerShell. Windows 10 ships with PowerShell v5.

## More information

For more help and guidance, refer to the following resources.

Tutorials:

* [Get started with Azure Data Lake Analytics using Azure PowerShell](https://azure.microsoft.com/en-us/documentation/articles/data-lake-analytics-get-started-powershell/) 
* [Get started with Azure Data Lake Store using Azure PowerShell](https://azure.microsoft.com/en-us/documentation/articles/data-lake-store-get-started-powershell/)

Reference material (MSDN):

* [Azure Data Lake Analytics Cmdlets](https://msdn.microsoft.com/en-us/library/mt607124.aspx) 
* [Azure Data Lake Store Cmdlets](https://msdn.microsoft.com/en-us/library/mt607120.aspx)

# Exercise 0: Installing and configuring Azure PowerShell
In this exercise you will install the Azure Resource Manager cmdlets in Windows PowerShell. These cmdlets will enable you to interact with Azure Data Lake (ADL) and complete the tasks in this lab.

1. Open a new PowerShell window.

2. Run the following cmdlet to log in to Azure PowerShell:
 
        Login-AzureRmAccount
 
3. At the authentication prompt, use the credentials provided by the instructor to log in.

4. (Optional) If you think you have more than one subscription, you can specify the subscription by running the following cmdlet:

        Select-AzureRmSubscription -SubscriptionName "<your subscription name>"

5. (Optional) To save your login session and restore it later, you can run the following cmdlets:

        # Save the login session after logging in.
        Save-AzureRmProfile -Path C:\adldemo\profile
        
        # Restore the login session.
        #   (Put this at the top of scripts that you'll run later.)
        Select-AzureRmProfile -Path C:\adldemo\profile

# Exercise 1: Exploring ADL PowerShell cmdlets
In this exercise, you will explore the ADL cmdlets that are available for you to use in Azure PowerShell.

1. At the PowerShell command prompt, run the following command to list the ADL Analytics accounts that are available to you:
 
        Get-AzureRmDataLakeAnalyticsAccount
 
2. Run the following cmdlet to list all the Data Lake cmdlets that you can use:
 
        Get-Command *DataLake*
 
3. Run the following cmdlet to get help on a specific cmdlet (in this case the ``Get-AzureRmDataLakeAnalyticsAccount`` cmdlet):
 
        Get-Help Get-AzureRmDataLakeStoreAccount

4. Run the following cmdlets to list all Data Lake Analytics and Store accounts in the currently-selected subscription:

        Get-AzureRmDataLakeAnalyticsAccount
        Get-AzureRmDataLakeStoreAccount


# Exercise 2: Listing and submitting ADLA jobs
In this exercise you will list the jobs that have run on your ADLA account. You will also submit a new job and check its status.

1. List all jobs that have run on your ADLA account.
      * Use the cmdlet ``Get-AzureRmDataLakeAnalyticsJob``.
      * Use the ``-State`` parameter to filter by job state(s), like Running or Ended.

2. Submit a new job to your ADLA account. 
      * First,  save the following script to a file on your local machine, being sure to *change* the "UserName" part to something specific to you:
                            
                        @searchlog = EXTRACT UserId int, Start DateTime, Region string,
                        Query string, Duration int, Urls string, ClickedUrls string FROM
                        @"/Samples/Data/SearchLog.tsv" USING Extractors.Tsv();
                        OUTPUT @searchlog TO @"/Samples/Output/UserName/SearchLog_TestOutput.tsv" USING
                        Outputters.Tsv();

      * Then, use the cmdlet ``Submit-AzureRmDataLakeAnalyticsJob`` with the ``-ScriptPath`` parameter.
			
3. Retrieve the status of the job that you submitted. 
      * Use the cmdlet ``Get-AzureRmDataLakeAnalyticsJob``.
      * Pass in the job ID returned by Step 2 to the ``-JobId`` parameter.

# Exercise 3: Exploring and downloading ADLS files
In this exercise you will explore the files in your ADLS account. You will also download the file created by the job you submitted in Exercise 2.

1. List all files in the **/Samples/Output/UserName/** folder. 
      * Use the cmdlet ``Get-AzureRmDataLakeStoreChildItem``.
2.  Download the output of the job from Part 2.
      * Use the cmdlet ``Export-AzureRmDataLakestoreItem``.
      * Use the ADLS file path **"/Samples/Output/UserName/SearchLog_TestOutput.tsv"**.

# PowerShell Quick Start

Once you've gone through this HOL you can try exploring the [ADL PS QuickStart](/docs/PowerShell/ADL_PS_QuickStart.md)
