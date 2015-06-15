# First steps: Azure Data Lake .NET SDK

This guide assumes you have previously followed the steps in the main [Getting Started guide](../GettingStarted.md).

------------

### First steps

#### Download and extract the Azure Data Lake .NET SDK NuGet packages
1. Download the [Azure Data Lake .NET SDK NuGet packages here](https://github.com/MicrosoftBigData/AzureDataLake/releases/download/PowerShellSDK/AzureSDK_DataLake_Only.zip).
1. Extract the contents of **AzureSDK_DataLake_Only.zip**.
1. Place the contents in a directory, e.g. ``C:\KonaDataLake_NuGet\...``

#### Add NuGet packages to your .NET project in Visual Studio
1. Either open or create a .NET project in Visual Studio.

1. Right-click on your project in the Solution Explorer and select "Manage NuGet Packages...":

    ![](img/firststeps_managenugetpackages.png)

1. Click "Settings":

    ![](img/firststeps_settings.png)

1. Click the "+" button to add a new NuGet source:

    ![](img/firststeps_settings_plus.png)

1. Enter the information below for the new source:

    * Name: Data Lake SDK
    * Source: C:\KonaDataLake_NuGet\

        Note: Use the path where you placed the NuGet packages initially.

    ![](img/firststeps_settings_enterinfo.png)

1. Click "Update".
 
       You should now see the new source in the source list:
    
    ![](img/firststeps_settings_updated.png)

1. Click OK.
    
       You should now be able to select Azure Data Lake from your list of NuGet sources.
    
    ![](img/firststeps_manage_list.png)

1. Select "Include Prerelease" from the dropdown.

1. Click "Install" on any/all Azure Data Lake packages that you would like to use with your project.

    You're done!

#### Learn more
* [SDK Tutorials](about:blank) - (COMING SOON) Learn how to write some basic code using the Azure Data Lake .NET SDK.
* [SDK User Manual](UserManual.md) - View some basic documentation for the Azure Data Lake .NET SDK.

------------

### Useful links

Browse the following pages:

* [Getting Started](../GettingStarted.md)
* Tools
    * [Azure Portal](../AzurePortal/FirstSteps.md)
    * [PowerShell](../PowerShell/FirstSteps.md)
    * [SDK](../SDK/FirstSteps.md)
