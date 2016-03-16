# Hands-on Lab: Using Azure Data Factory to Schedule Jobs in Azure Data Lake Analytics

# Introduction

In this lab you'll create a simple Azure Data Factory (ADF) pipeline that runs an Azure Data Lake (ADL) Analytics job every fifteen minutes.

# Prerequisites

To complete this lab you'll need:

- Owner Access to an ADL Analytics (ADLA) account
- Owner Access to an ADL Store (ADLS) account
- Owner Access to an Azure Storage account
- Owner Access to an Azure Data Factory Account 

# Getting started

This section provides information and preparatory steps that you will need in order to complete the lab.


## Important information

This lab requires you to remember several pieces of information in various places. Keep track of the following items:

    $subname = The subscription name       =  **ADLTrainingMS**
    $subid   = The subscription ID         =  **ace74b35-b0de-428b-a1d9-55459d7a6e30**
    $rg      = The resource group name. NOTE: To simplify this lab it's best if all the Azure services used are in the same resource group
    $blobs   = Azure Storage account          
    $adla    = The ADLA account            
    $adls    = The ADLS account            
    $blobs_access_key   = blob access key  
    $adf     = Your ADF account            

## The Azure Storage Account

If you don't already have an Azure Storage account, create in on the Azure Portal in the EAST US 2 Region. Please note the name of the account as $blobs and an access key as $blobs_access_key.

## The ADLA and ADLS Accounts

If you don't already have these accounts. Then go to the Azure Portal to create them.

- In the Azure Portal
- CLick Browse
- Select "Data Lake Analytics"
- Click Add (the plus sign)
- Enter a name for the account
- Click on "Data Lake Store"
- Click "Create New Data Lake Store"
- The Name will be prepopulated. Change it if you want.
- CLick OK
- Make sure the Subscription is correct
- Make sure the resource group name is correct 
- Click Create

These steps will create both an ADLA and ADLS account.

## Add the Azure Storage account as a datasource to the ADLA Account

- In the Azure Portal, locate your ADLA account
- Click "Add Data Source"
- For **Storage Type** select **Azure Storage**
- For **Storage Account name** enter the value for $blobs
- For **Account Key** enter the value for $blobs_access_key 


## Prepare the Azure Storage Account

- In the Azure Portal, locate your ADLA account
- Click "Data Explorer"
- Under **Storage accounts** Select the Azure Storage account 
- Click *New Folder" and give the name "scripts"
- Navigate to the "scripts" folder
- Upload the following text content below as a file called **SearchLog_15min.usql** (Save the text below into a file on your local machine, then use the "Upload" button in the Data Explorer to upload)

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

  
## Prepare the sample data

The script you will automate in this lab will use sample data that comes with the ADL Analytics account. When you create an ADL Analytics account, the sample data is not yet copied into the default ADL Store. Complete the following steps to copy the sample data into the default ADL Store:

1.  Browse to your ADLA account.
2.  Click **Essentials**.
3.  Click **Export sample jobs**.
4.  Wait a few seconds. If you see the message **samples not set up**, click **Copy Samples**. If you don't see any messages about the samples, you don't have to do anything.

To confirm that the sample data is in the ADL Store Account, open the Data Explorer and look under /Data/Samples. You should see a file named **SearchLog.tsv**. If you do not see this file contact the instructor.

## Link Services to your ADF Account

In this section, you will linked your ADF account to three services:
- Azure Data Lake Analytics
- Azure Data Lake Store
- Azure Blob Store

To get started, browse to http://portal.azure.com open your ADF account and click **Author and Deploy**.

### Link your ADL Analytics account

1.  Under **Author and Deploy**, click **New Compute** and then select **Axure Data Lake Analytics**. 
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

### Link your ADL Store Account

1.  Under **Author and Deploy**, click **New data store**, and then select **Azure Data Lake Store**. 
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

### Linked the shared Azure Storage account for this lab

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
2. Click **New dataset**, and then select **Azure Data Lake Store**.
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

1. In the ADF Portal, click **New dataset**, and then select **Azure Data Lake Store**.
2. Replace the existing JSON text with the following text:

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
    

# Exercise 2: Create a pipeline

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
                "start": "2015-08-08T00:00:00Z",
                "end": "2016-08-08T01:00:00Z",
                "isPaused": false,
                "hubName": "adltrainingadf0_hub",
                "pipelineMode": "Scheduled"
            }
        }
    
3. Review the JSON you just added, and then click **Deploy**.

# Excercise 3: Monitor the Pipeline
In this exercise, you will monitor the activity of the ADF pipeline you just created.

1. In the ADF portal, review the output dataset. You should now see the outputs being constructed.
2. Browse to your ADL Analytics account. You should see that jobs are being executed under the name **MyPipeline**.

# Excercise 4: Modify the Pipeline to produce one output per timeslice

As it currently exists, the pipeline keeps overrwiting the same output file. Many pipelines however, need to create a
new output file for every time slice.

Below is an EXAMPLE of how a ADF pipeline can create a new OUTPUT file for each slice.

**THIS IS AN EXAMPLE. DO NOT COPY PASTE IT INTO YOUR PIPELINE DEFINITION.**

    "typeProperties": {
          "scriptPath": "usql-scripts\\GetMcgSfAccountStandardFields.usql",
          "scriptLinkedService": "LinkedService_AS_EPTStorage",
          "degreeOfParallelism": 3,
          "priority": 100,
          "parameters": {
            "in": "/Someinputfile.txt",
            "out": "$$Text.Format('/Standard/Salesforce/Account/{0:yyyy}/{0:MM}/{0:dd}/MCG/Account.csv',SliceStart)"
          }
        },  

As you can see from the example, the C# string.Format() method is used to generate the output filename.

Using this information modify your pipeline so that the output files are placed in:

    /Samples/Output/MyPipeline/YEAR/MONTH/DAY/HOUR/MINUTE/SearchLog.tsv
    