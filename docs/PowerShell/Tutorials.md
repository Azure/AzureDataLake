# Tutorial: Using Azure PowerShell with Data Lake

This guide assumes you have previously followed the steps in the main [Getting Started guide](../GettingStarted.md) and the PowerShell [First Steps](FirstSteps.md) guide.

-------------

## Prerequisites
Before you begin this tutorial, you must download the sample data from the [AzureDataLake Git Repository](https://github.com/MicrosoftBigData/AzureDataLake/tree/master/Samples/SampleData/OlympicAthletes.tsv).

The data file used in this tutorial is a tab separated file with the following fields:

    Athlete              string,
    Age                  string,
    Country              string,
    Year                 string,
    ClosingCeremonyDate  string,
    Sport                string,
    GoldMedals           string,
    SilverMedals         string,
    BronzeMedals         string,
    TotalMedals          string,

This is the dataset that will be used for the examples in this document. 

## Exercise 1: 
Learn how to upload data into the Azure Data Lake, and download the results.

### Explore the ADL file system
Let’s first see what’s at the root of the file system.

    Get-AzureDataLakeChildItem -Path swebhdfs://<your ADL account name>.azuredatalake.net/

### Create a folder for storing the Sample Data

    New-AzureDataLakeItem -Folder -Path swebhdfs://<your ADL account name>.azuredatalake.net/ADLDemo

We can further verify that the folder was created:

    Get-AzureDataLakeChildItem -Path swebhdfs://<your ADL account name>.azuredatalake.net/

And of course, this directory should be empty which we can again confirm

    Get-AzureDataLakeChildItem -Path swebhdfs://<your ADL account name>.azuredatalake.net/ADLDemo

### Upload the Sample Data File
To upload the Sample data file we’ll use the following command

    cd C:\ADLDemo\
    Copy-AzureDataLakeItem -Path OlympicAthletes.tsv -Destination swebhdfs://<your ADL account name>.azuredatalake.net/ADLDemo/OlympicAthletes.tsv

Now, let’s verify that the file is there and that it has the size we expect.

    Get-AzureDataLakeChildItem -Path swebhdfs://<your ADL account name>.azuredatalake.net/ADLDemo

### Copy the Sample Data File
To copy the sample file, use the following command:

    Copy-AzureDataLakeItem -Path swebhdfs://<your ADL account name>.azuredatalake.net/ADLDemo/OlympicAthletes.tsv -Destination swebhdfs://<your ADL account name>.azuredatalake.net/ADLDemo/OlympicAthletes_Copy.tsv
    
### Download the Sample Data File
To download the sample file, use the following command:

    Copy-AzureDataLakeItem -Path swebhdfs://<your ADL account name>.azuredatalake.net/ADLDemo/OlympicAthletes.tsv -Destination ./OlympicAthletes_Copy.tsv

At this point you are done with your scenario and you have learned how to Upload, List, Copy, and Download files in Azure Data Lake.

### Useful links

Browse the following pages:

* [Getting Started](../GettingStarted.md)
* Tools
    * [Azure Portal](../AzurePortal/FirstSteps.md)
    * [PowerShell](../PowerShell/FirstSteps.md)
    * [SDK](../SDK/FirstSteps.md)
