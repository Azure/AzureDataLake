# Hands-on Lab: Using PowerShell to interact with Azure Data Lake

# Introduction

In this lab you will learn how to use Azure PowerShell cmdlets to interact with Azure Data Lake Analytics (ADLA) and Azure Data Lake Store (ADLS).

# Getting started


## Prerequisites

Before you can start the lab exercises, you will need various Azure services provisioned for you. Follow the instructions here: [Start](Start.md). 

This process only takes a few minutes. Once your services are configured you can proceed with the lab.


## Setting up your computer

To complete this lab, you will need a computer running Windows with the latest version of Windows PowerShell (v5) installed. Windows 10 ships with PowerShell v5.


# Exercise 0: Installing and configuring Azure PowerShell
In this exercise you will install the Azure Resource Manager cmdlets in Windows PowerShell. These cmdlets will enable you to interact with Azure Data Lake (ADL) and complete the tasks in this lab.

1. Open a new PowerShell window in administrator mode.
   - At the Start menu, type **PowerShell**.
   - Right-click **Windows PowerShell**, and then click **Run as administrator**.
      
2. At the PowerShell command prompt, run the following commands:
     
   	 Install-Module AzureRm
   	 Install-AzureRm
	 
3. Close the PowerShell window.

   > NOTE: If the installation fails, [click here to install the Azure Resource Manager cmdlets by using the Web Platform Installer](https://azure.microsoft.com/en-us/documentation/articles/powershell-install-configure/#step-1-install).

4. Open a new PowerShell window (do not run as administrator).
5. Run the following command to log in to Azure PowerShell:
 
        Login-AzureRmAccount
 
6. At the authentication prompt, use the credentials provided by the instructor to log in.

# Exercise 1: Exploring ADL PowerShell cmdlets
In this exercise, you will explore the ADL cmdlets that are available for you to use in Windows PowerShell.

1. At the PowerShell command prompt, run the following command to list the ADL Analytics accounts that are available to you:
 
        Get-AzureRmDataLakeAnalyticsAccount
 
2. Run the following command to list all the Data Lake cmdlets that you can use:
 
        Get-Command \*DataLake\*
 
3. Run the following command to get help on a specific cmdlet (in this case the ``Get-AzureRmDataLakeAnalyticsAccount`` cmdlet):
 
        help Get-AzureRmDataLakeStoreAccount
 
For more help and guidance, refer to the following resources.

Tutorials:

* [Get started with Azure Data Lake Analytics using Azure PowerShell](https://azure.microsoft.com/en-us/documentation/articles/data-lake-analytics-get-started-powershell/) 
* [Get started with Azure Data Lake Store using Azure PowerShell](https://azure.microsoft.com/en-us/documentation/articles/data-lake-store-get-started-powershell/)

Reference material (MSDN):

* [Azure Data Lake Analytics Cmdlets](https://msdn.microsoft.com/en-us/library/mt607124.aspx) 
* [Azure Data Lake Store Cmdlets](https://msdn.microsoft.com/en-us/library/mt607120.aspx)

# Exercise 2: Listing and submitting ADLA jobs
In this exercise you will list the jobs that have run on your ADLA account. You will also submit a new job and check its status.

1. List all jobs that have run on your ADLA account.
      * Use the cmdlet ``Get-AzureRmDataLakeAnalyticsJob``.
2. Submit a new job to your ADLA account. 
      * Use the cmdlet ``Submit-AzureRmDataLakeAnalyticsJob``.
      * Use the following job definition:

            @searchlog = EXTRACT UserId int, Start DateTime, Region string,
			Query string, Duration int, Urls string, ClickedUrls string FROM
			@"/Samples/Data/SearchLog.tsv" USING Extractors.Tsv(); OUTPUT
			@searchlog TO @"/Samples/Output/SearchLog_TestOutput.tsv" USING
			Outputters.Tsv();
			
3. Retrieve the status of the job that you submitted. 
      * Use the cmdlet ``Get-AzureRmDataLakeAnalyticsJob``.
      * Pass in the job ID returned by Step 2.

# Exercise 3: Exploring and downloading ADLS files
In this exercise you will explore the files in your ADLS account. You will also download the file created by the job you submitted in Exercise 2.

1. List all files in the **/Samples/Output/** folder. 
      * Use the cmdlet ``Get-AzureRmDataLakeStoreChildItem``.
2.  Download the output of the job from Part 2.
      * Use the cmdlet ``Export-AzureRmDataLakestoreItem``.
      * Use the ADLS file path **/Samples/Output/SearchLog_TestOutput.tsv**.

