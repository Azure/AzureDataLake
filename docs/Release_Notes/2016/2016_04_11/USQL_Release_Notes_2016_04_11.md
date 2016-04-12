# Azure Data Lake > U-SQL Release Notes 2016-04-11
--------------------------

# Breaking Changes
 
None.

## Azure Data Lake Tools for Visual Studio

### ADL Tools for Visual Studio is now Available in the Azure SDK

ADL Tools for Visual Studio is now available with the [Azure SDK](https://azure.microsoft.com/en-us/documentation/articles/azure-sdk-dotnet-release-notes-2-9/). 

### The "Data Lake" main menu item is no longer visible by default

To avoid cluttering up the main menu of Visual Studio, the "Data Lake" menu item is no longer visible by default. TO make it appear, simply click on any 
U-SQL or ADL Analytics features, such as viewing an ADL Account, U-SQL jobs, creating a U-SQL project, etc. 
 
### U-SQL Local Execution now supports parallelism

Now U-SQL scripts can run in parallel locally. You can specify the degree of parallelism in Data Lake > Options and Settings

### Viewing the outputs of U-SQL Local Execution

When you run a U-SQL script locally, you can now view the results in the output window by double clicking it.

### Cloud Explorer new shows ADL Store & Analytics accounts.

ADL Analytics & Store accounts are now visible in Cloud Explorer along with your other Azure resources.

NOTE: The Local execition account "(local)" is still only available in in Server Explorer. Soon, it will also be available in Cloud Explorer.

## U-SQL Big Fixes

### **MD5 Hash for WASB files is fixed**

Previously, when writing a file to WASB locations, the MD5 hash has was not set correctly and tools that checked the MD5 hash, such as Azure Data Explorer or Polybase, failed. This issue now has been fixed. 

### **CREATE TYPE is fixed**

U-SQL allows the creation of table types to be used in U-SQL function and procedure signatures. In this release, some meta data catalog issues have been fixed so the type should be usable. Example: 
 
    CREATE TYPE IF NOT EXISTS MyTableType AS TABLE (id int, data string); 

    CREATE FUNCTION IF NOT EXISTS MyTVF(@t MyTableType) RETURNS @r 
    AS
    BEGIN
        @r = SELECT * FROM @t;
    END;
        
