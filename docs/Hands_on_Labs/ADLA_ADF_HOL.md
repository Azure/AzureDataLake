Hands-on Lab: Using Azure Data Factory to Schedule Jobs in Azure Data Lake Analytics
====================================================================================

Introduction
============

In this lab you will create a simple Azure Data Factory (ADF) pipeline that runs
an Azure Data Lake Analytics (ADLA) job every fifteen minutes.

Getting started
===============

Prerequisites
-------------

Before you can start the lab exercises, you will need several Azure services
created. Follow the instructions here: [Start](Start.md)

 

Important information
---------------------

This lab requires you to remember and reuse several pieces of information in
various places. These pieces of information are provided to you when you visit
the registration website. Keep track of the following items:

-   **\$subname** - the subscription name.

-   **\$subid** - the subscription ID.

-   **\$rg** - the resource group name.

-   **\$blobs** - an Azure Storage account.

-   **\$adla** - the ADLA account.

-   **\$adls** - the Azure Data Lake Store (ADLS) account.

-   **\$blobs\_access\_key** - the Azure Blob access key.

-   **\$adf** - the ADF account.

Later in this lab, you will be asked to copy and paste some text that contains
these variables (\$adla for example). You can either:

-   Declare these variables in PowerShell and set them to the literal values
    provided by the registration website (the website provides variable values
    in a PowerShell-friendly format).

-   Replace these variables in the text you copy and paste with the literal
    values provided by the registration website.

Prepare the Azure Storage Account
---------------------------------

 

The Azure storage account will be used to store a script that will be run as
part of an ADF pipeline.

To use this Storage Account you’ll need to two two things:

-   Link the Storage Account to your ADLA Account

-   Place a U-SQL Script file in the Storage Account

 

 

-   In the Azure Portal, locate your ADLA account.

-   Click **Add Data Source**

-   For **Storage Type** select **Azure Storage**

-   Enter the name of the Storage Account in **Storage Account Name**

-   Enter the access key for the Storage Account in **Account Key**

-   Click **Select Data Source**

-   Click **Add**

-   Navigate to your ADLA Account

-   Click **Data Explorer**.

-   Under **Storage accounts**, select your Azure Storage account.

-   Click **New Folder** and create a a folder called **scripts**

-   Save the U-SQL Script below as file named **SearchLog\_15min.usql** then
    upload the file into the scripts folder using the **Upload** button

 

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    @searchlog = 
        EXTRACT UserId          int, 
                Start           DateTime, 
                Region          string, 
                Query           string, 
                Duration        int, 
                Urls            string, 
                ClickedUrls     string
        FROM @in 
        USING Extractors.Tsv();
    
    OUTPUT @searchlog 
        TO @out 
        USING Outputters.Tsv();
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

Prepare the sample data
-----------------------

The script you will automate in this lab uses sample data that comes with the
ADL Analytics account.

Complete the following steps to copy the sample data into the default ADL Store:

1.  Browse to your ADLA account.

2.  Click **Essentials**.

3.  Click **Explore example jobs**.

4.  Wait a few seconds. If you see the message **samples not set up**, click
    **Copy Samples**. If you don't see any messages about the samples, you don't
    have to do anything.

To confirm that the sample data is in the ADL Store Account, open the Data
Explorer and look under **/Data/Samples**. You should see a file named
**SearchLog.tsv**.

Link services to your ADF account
---------------------------------

In this section, you will link your ADF account to three services:

-   Azure Data Lake Analytics

-   Azure Data Lake Store

-   Azure Storage Account

 

To get started, browse to http://portal.azure.com, open your ADF account, and
click **Author and Deploy**.

### Link your ADL Analytics account

1.  Under **Author and Deploy**, click **New Compute** and then select **Azure
    Data Lake Analytics**.

2.  Replace the existing JSON text with the following text:

    ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    {
        "name": "MyADLA",
        "properties": {
            "description": "",
            "type": "AzureDataLakeAnalytics",
            "typeProperties": {
                "accountName": "$adla",
                "authorization": "**********",
                "sessionId": "**********",
                "subscriptionId": "$subid",
                "resourceGroupName": "$rg"
            }
        }
    }
    ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

3.  Click **Authorize**. When prompted, enter your credentials.

4.  Click **Deploy**.

### Link your ADL Store Account

1.  Under **Author and Deploy**, click **New data store**, and then select
    **Azure Data Lake Store**.

2.  Replace the existing JSON text with the following text:

    ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    {
        "name": "MyADLS",
        "properties": {
            "description": "",
            "type": "AzureDataLakeStore",
            "typeProperties": {
                "dataLakeStoreUri": "https://$adls.azuredatalakestore.net/webhdfs/v1",
                "authorization": "**********",
                "sessionId": "**********",
                "subscriptionId": "$subid",
                "resourceGroupName": "$rg"
            }
        }
    }
    ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

3.  Click **Authorize**. If prompted, enter your credentials.

4.  Click **Deploy**.

### Linked the shared Azure Storage account for this lab

1.  Under **Author and Deploy**, click **New data store**, and then select
    **Azure Storage**. Replace the existing JSON text with the following text:

    ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    {
        "name": "MyBlobStore",
        "properties": {
            "description": "",
            "type": "AzureStorage",
            "typeProperties": {
                "connectionString": "DefaultEndpointsProtocol=https;AccountName=$blobs;AccountKey=$blobs_access_key"
            }
        }
    }
    ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

2.  Click **Deploy**.

Exercise 1: Create input and output datasets
============================================

In this exercise, you'll create two datasets: one to represent the input file,
and one to represent the output file.

Create the input dataset
------------------------

1.  Browse to the ADF Portal.

2.  Click **New dataset**, and then select **Azure Data Lake Store**.

3.  Replace the existing JSON text with the following text:

    ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    {
        "name": "MyInput",
        "properties": {
            "published": false,
            "type": "AzureDataLakeStore",
            "linkedServiceName": "MyADLS",
            "typeProperties": {
                "fileName": "SearchLog.tsv",
                "folderPath": "/Samples/Data",
                "format": {
                    "type": "TextFormat",
                    "columnDelimiter": "\t"
                }
            },
            "availability": {
                "frequency": "Minute",
                "interval": 15
            },
            "external": true,
            "policy": {}
        }
    }
    ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

Create the output dataset
-------------------------

1.  In the ADF Portal, click **New dataset**, and then select **Azure Data Lake
    Store**.

2.  Replace the existing JSON text with the following text:

    ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    {
        "name": "MyOutput",
        "properties": {
            "published": false,
            "type": "AzureDataLakeStore",
            "linkedServiceName": "MyADLS",
            "typeProperties": {
                "fileName": "SearchLog.tsv",
                "folderPath": "/Samples/Output/MyPipeline",
                "format": {
                    "type": "TextFormat",
                    "columnDelimiter": "\t"
                }
            },
            "availability": {
                "frequency": "Minute",
                "interval": 15
            }
        }
    }
    ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

Exercise 2: Create a pipeline
=============================

In this exercise, you will create a new pipeline that runs a script every 15
minutes.

1.  Browse to the ADF portal, and then click **new pipeline**.

2.  Copy the following JSON text into the new pipeline:

    ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    {
        "name": "MyPipeline",
        "properties": {
            "description": "",
            "activities": [
                {
                    "type": "DataLakeAnalyticsU-SQL",
                    "typeProperties": {
                        "scriptPath": "\\scripts\\SearchLog_15min.usql",
                        "scriptLinkedService": "MyBlobStore",
                        "degreeOfParallelism": 1,
                        "priority": 100,
                        "parameters": {
                            "in": "/Samples/Data/SearchLog.tsv",
                            "out": "/Samples/Output/MyPipeline/SearchLog.tsv"
                        }
                    },
                    "inputs": [
                        {
                            "name": "MyInput"
                        }
                    ],
                    "outputs": [
                        {
                            "name": "MyOutput"
                        }
                    ],
                    "policy": {
                        "timeout": "06:00:00",
                        "concurrency": 1,
                        "executionPriorityOrder": "NewestFirst",
                        "retry": 1
                    },
                    "scheduler": {
                        "frequency": "Minute",
                        "interval": 15
                    },
                    "name": "MyActivity",
                    "linkedServiceName": "MyADLA"
                }
            ],
            "start": "2016-01-08T00:00:00Z",
            "end": "2016-01-09T00:00:00Z",
            "isPaused": false,
            "hubName": "adltrainingadf0_hub",
            "pipelineMode": "Scheduled"
        }
    }
    ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

3.  Review the JSON you just added, and then click **Deploy**.

Exercise 3: Monitor the Pipeline
================================

In this exercise, you will monitor the activity of the ADF pipeline you just
created.

1.  In the Azure Data Factory portal, review the output dataset. You should now
    see the outputs being constructed.

2.  Browse to your ADL Analytics account. You should see that jobs are being
    executed under the name **MyPipeline**.
