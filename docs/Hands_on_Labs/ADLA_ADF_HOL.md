# Hands on  Lab: Using ADF to Schedule Jobs in ADL Analytics

# Introduction

In this lab you'll create a simple ADF pipeline that runs an ADL Analytics Job every fifteen minutes.

# Prerequisites

To perform this lab you'll need to create:

- An Azure DataFactory Account
- Have access to a ADL Analytics account (this is provided for you in the lab)
- Have access to a ADL Store account (this is provided for you in the lab)
- Have access to a Azure Blob Store account (this is provided for you in the lab)

NOTE: There is a single blob store account used for all this HOL.

# Part 0 - Preparation

This lab requires remembering quite a few pieces of information in a number of places. So, keep track of the following items

- The Subscription ID: ADLTrainingMS
- The Resource Group name: PostTechReady
- The name of an ADL Analytics account: msanalytics0
- The name of an ADL Store account: msstore0
- The name of an Azure Blob Store account: adltrainingblobs
- the accesss kwey for the blob store account (will be provided by the lab instructor)


Copy Sample Data
go to ADLA account
click on essentials
CLick on Exppor sample jobs
wait a few seconds
If it says samples not set up click "copy sample jobs"
CLick Copy Sample Jobs




# Part 1 - Create an ADF Account

Go to http://portal.azure.com and create a Data Factory Account

# Part 2 - Create Linked Services

You'll need to create a linked service for these accounts
- ADLA
- ADLS
- Blob

Go to the http://portal.azure.com open your ADF account and click **Author and Deploy**

## Create a Linked ADLA Account

In "Author and Deploy" click "New Compute" and select ADLA. Replace the JSON with the text below

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

Then Click **Authorize** and enter your credentials.

Click deploy.

## Create a Linked ADLS Account

In "Author and Deploy" click "New data store" and select ADLs. Replace the JSON with the text below

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

Then Click **Authorize**.

Click deploy.

## Create a Linked Azure Storage Account

In "Author and Deploy" click "New data store" and select "Azxure Storage". Replace the JSON with the text below


    {
        "name": "MyBlobStore",
        "properties": {
            "description": "",
            "type": "AzureStorage",
            "typeProperties": {
                "connectionString": "DefaultEndpointsProtocol=https;AccountName=$blobs;AccountKey=$blobs_access_key "
            }
        }
    }

Click Deploy


# Part 3 - Create an DataSets

You'll create two datasets to represent (1) the input file and (2) the output file

## Create the input datasets

In the ADF Portal click "New dataset" and select ADLS. Replace the JSON with this

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

## Create the output datasets

In the ADF Portal click "New dataset" and select ADLS. Replace the JSON with this

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

# Part 4 Create a Script

In you blob store there is a container called "scripts" inside there is a script called SearchLog_15min.usql


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
    
    
    # Part 5 Create a Pipeline

CLick **new pipeline**

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

