# Tutorial: Using Azure PowerShell with Data Lake

This guide assumes you have previously followed the steps in the main [Getting Started guide](../GettingStarted.md) and the PowerShell [First Steps](FirstSteps.md) guide.

## Prerequisites
Before you begin this tutorial, you must download the data.

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

Download them from the [AzureDataLake Git Repository](https://github.com/MicrosoftBigData/AzureDataLake)

In the Repo in the path /Samples/SampleData you’ find the file called OlympicAthletes.tsv. This is the dataset that will be used for the examples in this document. 

## Exercise 1: Manage ADL users
### Setting up your ADL Session
1.	Open a new PowerShell window.
2.	Select your subscription by entering the following: Select-AzureSubscription -SubscriptionId $subscriptionId

### Managing Users 
See which users have access and what roles:

### Add users
Add a user to your ADL Account
New-ADLUser –UserEmail user1@aadtenant.onmicrosoft.com –UserRole User 

### List users
Use the following command to list the existing ADL users:
Get-ADLUser 

The output is similar to:

CreationTime                LastModifiedTime            Name                        Roles                      
------------                ----------------            ----                        -----                      
Mon, 13 Oct 2014 18:42:3... Mon, 13 Oct 2014 18:42:3... PublicTest@abc.          .. {Admin}                    
Tue, 14 Oct 2014 02:24:3... Tue, 14 Oct 2014 02:24:3... adluser2@test               {Admin}                    
Tue, 14 Oct 2014 02:26:0... Tue, 14 Oct 2014 02:26:0... adluser1@onboardflow.on. .. {Admin}                    

### Remove users
Use the following cmdlet to remove a user from your ADL account
Remove-ADLUser –UserEmail user1@aadtenant.onmicrosoft.com 

## Exercise 2: 
Learn how to upload data into the Azure Data Lake, and download the results.

### Define utility functions
To simplify your experience run the following script. It will create some utility functions that will simplify these exercises. In the future these helper functions will not be needed.
function kdir( [string] $Path )
{
    $result = @()
    $items = (Get-AzureDataLakeItem -Path $Path)
    $files = $items.Files | Select-Object -Property Name,Length
    $folders = $items.SubFolders| Select-Object -Property Name,Length
    $result += $files 
    $result += $folders
    $result
}

function kmkdir ( [string] $Path )
{
    New-AzureDataLakeItem -Path $Path -Folder 
}


function kupload ( [string] $Local, [string] $Remote )
{
    Copy-AzureDataLakeItem -Path $local -Destination $Remote
}

### Explore the ADL file system
Let’s first see what’s at the root of the file system.

kdir /

Name                                                                          Length
----                                                                          ------
mafs://accounts/<Your ADL Account name>/fs/foo.txt                           526136
mafs://accounts/<Your ADL Account name>/fs/fs/                                                          
mafs://accounts/<Your ADL Account name>/fs/system/                                                      
mafs://accounts/<Your ADL Account name>/fs/test/                                                        
mafs://accounts/<Your ADL Account name>/fs/users/                                                       

Naturally, depending on how you have been using your ADL account you may see different things but at least you should notice that you see folders and perhaps some files.

### Create a folder for storing the Sample Data

kmkdir /ADLDemo

AccessedSizeInBiMonth     : 
AccessedSizeInMonth       : 
AccessedSizeInQuarter     : 
AccessedSizeInWeek        : 
Created                   : 1/1/0001 12:00:00 AM
CreatedBy                 : 
Description               : 
DirectorySizeRefreshTime  : 1/1/0001 12:00:00 AM
Files                     : {mafs://accounts/<Your ADL Account name>/fs/ADLDemo/__placeholder__}
HotDataSizeRatio          : 
HotDataSizeRatioInBiMonth : 
HotDataSizeRatioInMonth   : 
HotDataSizeRatioInQuarter : 
HotDataSizeRatioInWeek    : 
LastModified              : 1/1/0001 12:00:00 AM
Name                      : mafs://accounts/<Your ADL Account name>/fs/ADLDemo/
Permission                : 
PhysicalSizeOwned         : 0
SubFolders                : {}

We can further verify that the folder was created

kdir ./

Name                                                                          Length
----                                                                          ------
swebhdfs://<Your ADL Account name>.azuredatalake.com /foo.txt                                                526136
mafs://accounts/<Your ADL Account name>/fs/ADLDemo/  
mafs://accounts/<Your ADL Account name>/fs/fs/                                                          
mafs://accounts/<Your ADL Account name>/fs/system/                                                      
mafs://accounts/<Your ADL Account name>/fs/test/                                                        
mafs://accounts/<Your ADL Account name>/fs/users/                                                       
And of course, this directory should be empty which we can again confirm
kdir /ADLDemo

Name                                                                          Length
----                                                                          ------
mafs://accounts/<Your ADL Account name>/fs/ADLDemo/__place...                                         0
NOTE: Currently newly-created directories contain a __placeholder file which you can see above. This is mistake in our system, and in the future no _placeholder file will be seen .

### Upload the Sample Data File
To upload the Sample data file we’ll use the following command

kupload D:\ADLDemo\OlympicAthletes.tsv /ADLDemo/OlympicAthletes.tsv


CreationTime           : 0001-01-01T00:00:00
ExpirationTime         : 9999-12-31T23:59:59.9999999
Extents                : 
Flags                  : 256
Id                     : 12326eef-99a5-4b3c-beea-823c67c4e968
Length                 : 526136
ModificationTime       : 2014-10-24T20:21:29.6714891Z
Name                   : mafs://accounts/<Your ADL Account name>/fs/ADLDemo/OlympicAthletes.tsv
CosmosPath             : mafs://accounts/<Your ADL Account name>/fs/ADLDemo/OlympicAthletes.tsv
OwnerVc                : 
Permission             : 
NumberOfPendingExtents : 0
Path                   : 
StartExtentIndex       : 0
StartExtentOffset      : 0

Now, let’s verify that the file is there and that it has the size we expect.

kdir ./ADLDemo

Name                                                                          Length
----                                                                          ------
mafs://accounts/<Your ADL Account name>/fs/ADLDemo/Olympic...                                    526136
mafs://accounts/<Your ADL Account name>/fs/ADLDemo/__place...                                         0

### Look at Files in ADL

kdir ./ADLDemo

Name                                                                          Length
----                                                                          ------
mafs://accounts/<Your ADL Account name>/fs/ADLDemo/Olympic...               526136
mafs://accounts/<Your ADL Account name>/fs/ADLDemo/Olympic...               526136
mafs://accounts/<Your ADL Account name>/fs/ADLDemo/__place...                    0

As you can see the output file exists.

Now let’s download the output to examine it.
Export-ADLStorageItem mafs://accounts/<Your ADL Account name>/fs/ADLDemo/OlympicAthletes_Copy.tsv d:\ADLDemo\OlympicAtheletes_Copy.tsv

At this point you are done with your scenario and you have learned Uploading, Listing, and Downloading files in ADL.

