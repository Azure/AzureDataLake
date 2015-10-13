# Tutorial: Using Azure PowerShell with Data Lake Store

This guide assumes you have previously followed the steps in the main [Getting Started guide](../GettingStarted.md) and the PowerShell [First Steps](FirstSteps.md) guide.

-------------

## Prerequisites
Before you begin this tutorial, you must download the sample data from the AzureDataLake Git Repository. Download the file. Store the OlympicAthletes.tsv in a local directory on your computer e.g. c:\adldemo\


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

For all the examples, you will need the Azure Data Lake Store account that was created. To get it open a PowerShell window and type the command "Get-AzureRmDataLakeStoreAccount". Note the "Name" property of the account, that is referred to as "<your ADLS account name>" in rest of this document.

### Setup your root directory for each reference 

    $myrootdir = "/"
    
### Explore the ADLS file system
Let’s first see what’s at the root of the file system for your account. In PowerShell, type the command below.  

    Get-AzureRmDataLakeStoreChildItem -Path $myrootdir

### Create a folder for storing the Sample Data

    New-AzureRmDataLakeStoreItem -Folder -Path $myrootdir/adldemo

We can further verify that the folder was created:

    Get-AzureRmDataLakeStoreChildItem -Path $myrootdir

And of course, this directory should be empty which we can again confirm

    Get-AzureRmDataLakeStoreChildItem -Path $myrootdir/adldemo

### Upload the Sample Data File
To upload the Sample data file we’ll use the following command

    cd C:\adldemo\
    Import-AzureRmDataLakeStoreItem -AccountName $adlStoreAccountName -Path OlympicAthletes.tsv `
    -Destination $myrootdir/adldemo/OlympicAthletes.tsv

Now, let’s verify that the file is there and that it has the size we expect.

    Get-AzureRmDataLakeStoreChildItem -Path $myrootdir/adldemo

### Rename the Sample Data File
To rename the sample file, use the following command:

    Move-AzureRmDataLakeStoreItem `
    -Path $myrootdir/adldemo/OlympicAthletes.tsv `
    -Destination $myrootdir/adldemo/OlympicAthletes_Copy.tsv
    
### Download the Sample Data File
To download the sample file, use the following command:

    Export-AzureRmDataLakeStoreItem -AccountName $adlStoreAccountName`
    -Destination c:\adldemo\OlympicAthletes_Copy.tsv `
    -Path $myrootdir/adldemo/OlympicAthletes_Copy.tsv
    
Compare the sizes of the original file OlympicAthletes.tsv and the downloaded file OlympicAthletes_Copy.tsv. They should be the same.

At this point you are done with your scenario and you have learned how to Upload, List, Rename, and Download files in Azure Data Lake.

### Useful links

Browse the following pages:

* [Getting Started](../GettingStarted.md)
* Tools
    * [Azure Portal](../AzurePortal/FirstSteps.md)
    * [PowerShell](../PowerShell/FirstSteps.md)
    * [SDK](../SDK/FirstSteps.md)
