# Hands-on Lab: Using Azure Data Factory to Schedule Jobs in Azure Data Lake Analytics

# Introduction

In this lab you'll create a simple Azure Data Factory (ADF) pipeline that runs an Azure Data Lake (ADL) Analytics job every fifteen minutes.

# Prerequisites

To complete this lab you'll need:

- Access to an ADL Analytics (ADLA) account (this is provided for you in the classroom).
- Access to an ADL Store (ADLS) account (this is provided for you in the classroom).

If you don't know how to access these accounts from the Azure portal, notify the instructor.

# Getting started

This section provides information and preparatory steps that you will need in order to complete the lab.

## A common Azure Blob Store account

Your ADL Analytics account has already been given access to an Azure Storage account called **adltrainingblobs**. The purpose of this Azure Storage account is to store a U-SQL script that is used by ADF.

From your ADL Analytics account, browse to the storage account, and look inside the **scripts** container. You'll see at least one U-SQL query there.

NOTE: in this lab you will have *write* access to the blob store. Please be aware of that and do not modify or delete the data there. 

## Important information

This lab requires you to remember several pieces of information in various places. Keep track of the following items:

    $subname = The subscription name       =  **ADLTrainingMS**
    $subid   = The subscription ID         =  **ace74b35-b0de-428b-a1d9-55459d7a6e30**
    $rg      = The resource group name     =  **PostTechReady**
    $adla    = The ADLA account            =  *(this will be provided by the lab instructor)*
    $adls    = The ADLS account            =  *(this will be provided by the lab instructor)*
    $blobs   = Azure Storage accoun t      =  **adltrainingblobs** 
    $blobs_access_key   = blob access key  =  *(this will be provided by the lab instructor)*

## Prepare the sample data

The script you will automate in this lab will use sample data that comes with the ADL Analytics account. When you create an ADL Analytics account, the sample data is not yet copied into the default ADL Store. Complete the following steps to copy the sample data into the default ADL Store:

1.  Browse to your ADLA account.
2.  Click **Essentials**.
3.  Click **Export sample jobs**.
4.  Wait a few seconds. If you see the message **samples not set up**, click **Copy Samples**. If you don't see any messages about the samples, you don't have to do anything.

To confirm that the sample data is in the ADL Store Account, open the Data Explorer and look under /Data/Samples. You should see a file named **SearchLog.tsv**. If you do not see this file contact the instructor.

## Create an ADF Account

Browse to http://portal.azure.com and create a Data Factory Account.

## Create linked services

In this section, you will create a linked service for each of these accounts:

- ADLA
- ADLS
- Azure Blob Store

To get started, browse to http://portal.azure.com open your ADF account and click **Author and Deploy**.

### Create a linked ADLA account

1.  Under **Author and Deploy**, click **New Compute** and then select **ADLA**. 
2.  Replace the existing JSON text with the following text:

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
    
3.  Click **Authorize**. When prompted, enter your credentials.
4.  Click **Deploy**.

### Create a linked ADLS account

1.  Under **Author and Deploy**, click **New data store**, and then select **ADLS**. 
2.  Replace the existing JSON text with the following text:

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
    
3.  Click **Authorize**. If prompted, enter your credentials.
4.  Click **Deploy**.

### Create a linked Azure Storage account

1.  Under **Author and Deploy**, click **New data store**, and then select **Azure Storage**. Replace the existing JSON text with the following text:

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
    
3.  Click **Deploy**.

# Exercise 1: Create input and output datasets

In this exercise, you'll create two datasets: one to represent the input file, and one to represent the output file.

## Create the input dataset

1. Browse to the ADF Portal.
2. Click **New dataset**, and then select **ADLS**.
3. Replace the existing JSON text with the following text:

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
    	
## Create the output dataset

1. In the ADF Portal, click **New dataset**, and then select **ADLS**.
2. Replace the existing JSON text with the following text:

        {
            "name": "MyOutput",
            "properties": {
                "published": false,
                "type": "AzureDataLakeStore",
                "linkedServiceName": "MyADLS",
                "typeProperties": {
                    "fileName": "SearchLog_Output.tsv",
                    "folderPath": "/Samples/Output",
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
    
# Exercise 2: Examine the script you will automate
In this exercise, you will review the script that you will automate in the next exercise.

1. Browse to your Azure Blob Store account.
2. Within the **scripts** container, review the script named **SearchLog_15min.usql**. The script resembles the following:

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
        
3. Notice that the input and output files are represented by variables named @in and @out.

# Exercise 3: Create a pipeline
In this exercise, you will create a new pipeline that runs a script every 15 minutes.

1. Browse to the ADF portal, and then click **new pipeline**.
2. Copy the following JSON text into the new pipeline:

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
                                "out": "/Samples/Output/SearchLog_Output.tsv"
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
                "start": "2015-08-08T00:00:00Z",
                "end": "2016-08-08T01:00:00Z",
                "isPaused": false,
                "hubName": "adltrainingadf0_hub",
                "pipelineMode": "Scheduled"
            }
        }
    
3. Review the JSON you just added, and then click **Deploy**.

# Monitor the Pipeline
In this exercise, you will monitor the activity of the ADF pipeline you just created.

1. In the ADF portal, review the output dataset. You should now see the outputs being constructed.
2. Browse to your ADL Analytics account. You should see that jobs are being executed under the name **MyPipeline**.
