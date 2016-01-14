# Hands on  Lab: Using ADF to Schedule Jobs in ADL Analytics

# Introduction

In this lab you'll create a simple ADF pipeline that runs an ADL Analytics Job every fifteen minutes.

# Part 0 - Preparation

To perform this lab you'll be given access to
- An ADL Analytics account (this is provided for you in the lab)
- An ADL Store account (this is provided for you in the lab)

If you don't know how to access these accounts from the portal, notify the instructor.

# Part 0 - A Common Blob Store Account

Your ADL Analytics account has already been given access to an Azure Storage account called adltrainingblobs

The only purpose for this Azure Storage account is to store a U-SQL script that is used by ADF.

From your ADL Analytics account, browse to the storage account, and look inside the "scripts" container. You'll see at least on U-SQL query there.

# Part 0 - Important pieces of information

This lab requires remembering quite a few pieces of information in a number of places. So, keep track of the following items

- The Subscription Name: ADLTrainingMS
- The Resource Group name: PostTechReady
- The name of an ADL Analytics account: (will be provided by the lab instructor)
- The name of an ADL Store account: (will be provided by the lab instructor)
- The name of an Azure Blob Store account: adltrainingblobs
- the access key for the blob store account (will be provided by the lab instructor)

# Part 0 - Preparing Sample Data

The script we will automate will use Sample data that comes with the ADL Analytics account.

When you create an ADL Analytics account, the sample data is not yet copied into the default ADL Store. To force the data to be copied.

*  Go to your ADLA account.
*  Click on "essentials".
*  Click on "export sample jobs".
*  Wait a few seconds,  if the portal says "samples not set up", click "Copy Samples". If it doesn't mention anything about the samples, you don't have to do anything.

To confirm that the sample data is in the ADL Store Account, open the Data Explorer and look under /Data/Samples you should see a file called SearchLog.tsv. If you do not see this file contact the instructor.

# Part 0 - Create an ADF Account

Go to http://portal.azure.com and create a Data Factory Account

# Part 0 - Create Linked Services

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

# Part 4 Look at the Script we will run

In you blob store there is a container called "scripts" inside there is a script called SearchLog_15min.usql

Notice that the input and output files are variables called @in and @out.

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

# Part 5 Create a PipeLine

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

CLick **Deploy**



# Monitor the Pipeline

In the ADF portal, look at the output dataset. You should now see the outputs being constructed.

Then look in you ADL Analytics account, you should see that jobs are being executed as part of "MyPipeline"

