# Hands on  Lab: Using ADF to Schedule Jobs in ADL Analytics

# Introduction

In this lab you'll create a simple ADF pipeline that runs an ADL Analytics Job every fifteen minutes.

# Prerequisites

To perform this lab you'll need to create:

- An Azure DataFactory Account
- Have access to a ADL Analytics account (this is provided for you in the lab)
- Have access to a ADL Store account (this is provided for you in the lab)
- Have access to a Azure Blob Store account (this is provided for you in the lab)


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
            "hubName": "my_hub",
            "type": "AzureDataLakeAnalytics",
            "typeProperties": {
                "accountName": "<NAME OF YOUR ADLA ACCOUNT>",
                "authorization": "**********",
                "sessionId": "**********",
                "subscriptionId": "<YOUR SUBSCRIPTION ID>",
                "resourceGroupName": "<YOUR RESOURCE GROUP>"
            }
        }
    }

Then Click **Authorize**.

## Create a Linked ADLS Account

In "Author and Deploy" click "New data store" and select ADLs. Replace the JSON with the text below

    {
        "name": "MyADLS",
        "properties": {
            "description": "",
            "hubName": "my_hub",
            "type": "AzureDataLakeStore",
            "typeProperties": {
                "dataLakeStoreUri": "https://<<YOU ADLS ACCOUNT>>.azuredatalakestore.net/webhdfs/v1",
                "authorization": "**********",
                "sessionId": "**********",
                "subscriptionId": "<YOUR SUBSCRIPTION ID>",
                "resourceGroupName": "<YOUR RESOURCE GROUP>"
            }
        }
    }

Then Click **Authorize**.

## Create a Linked Azure Storage Account

In "Author and Deploy" click "New data store" and select "Azxure Storage". Replace the JSON with the text below


    {
        "name": "MyBlobStore",
        "properties": {
            "description": "",
            "hubName": "my_hub",
            "type": "AzureStorage",
            "typeProperties": {
                "connectionString": "DefaultEndpointsProtocol=https;AccountName=<<BLOB ACCOUNT NAME>;AccountKey=<<THE ACCOUNT KEY>>"
            }
        }
    }




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

In you blob store there is a container called "scripts" inside there is a script called searchlog.usql


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
                        "scriptPath": "\\scripts\\SearchLogProcessing.usql",
                        "scriptLinkedService": "MyBlobStore",
                        "degreeOfParallelism": 2,
                        "priority": 100,
                        "parameters": {
                            "in": "/Data/Input/SearchLog.tsv",
                            "out": "/Data/Output/Result.tsv"
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
            "hubName": "my_hub",
            "pipelineMode": "Scheduled"
        }
    }

