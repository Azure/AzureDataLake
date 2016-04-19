# Azure Data Lake Tools for Visual Studio Release Notes 2016-04-11
--------------------------

## Breaking Changes
 
None.

## Updates

#### ADL Tools for Visual Studio is now Available in the Azure SDK

ADL Tools for Visual Studio is now available with the [Azure SDK](https://azure.microsoft.com/en-us/documentation/articles/azure-sdk-dotnet-release-notes-2-9/). 

#### The "Data Lake" main menu item is no longer visible by default

To avoid cluttering up the main menu of Visual Studio, the "Data Lake" menu item is no longer visible by default. TO make it appear, simply click on any 
U-SQL or ADL Analytics features, such as viewing an ADL Account, U-SQL jobs, creating a U-SQL project, etc. 
 
#### U-SQL Local Execution now supports parallelism

Now U-SQL scripts can run in parallel locally. You can specify the degree of parallelism in Data Lake > Options and Settings

#### Viewing the outputs of U-SQL Local Execution

When you run a U-SQL script locally, you can now view the results in the output window by double clicking it.

#### Cloud Explorer new shows ADL Store & Analytics accounts.

ADL Analytics & Store accounts are now visible in Cloud Explorer along with your other Azure resources.

NOTE: The Local execution account "(local)" is still only available in in Server Explorer. Soon, it will also be available in Cloud Explorer.

