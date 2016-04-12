# Azure Data Lake > U-SQL Release Notes 2016-04-11
--------------------------

## BREAKING CHANGES
 
No known breaking changes.

## AZURE DATA LAKE TOOLS FOR VISUAL STUDIO UPDATE
 1. **Hiding "Data Lake" top menu by default**

    As Azure Data Lake Tools for Visual Studio is now shipping together with [Azure SDK](https://azure.microsoft.com/en-us/documentation/articles/azure-sdk-dotnet-release-notes-2-9/), which is the central place for .NET developers for Azure development, we decided to hide the "Data Lake" menu by default to not disturb users.

    The menu will still appear when you click any U-SQL or Data Lake Analytics features, such as viewing the jobs, creating a U-SQL project, etc. 
 
 2. **Improvements for U-SQL local run**

    We have made several improvements for U-SQL local run, including:
    - Now U-SQL scripts can run in parallel locally - you can specify the degree of parallelism in Data Lake > Options and Settings
    - U-SQL local run output view: you can now view the U-SQL results in the output window by double clicking it.

 3. **Cloud Explorer Integration for Data Lake Analytics and Data Lake Store**
   
    We have integrated Data Lake Analytics and Data Lake Store in Cloud Explorer and you can easily navigate all your Azure resources there.

    Note: Local run account (local) is still in Server Explorer and we will migrate that part soon.

## MAJOR BUG FIXES

 1. **MD5 Hash for WASB files is fixed**

   Previously, when writing a file to WASB locations, the MD5 hash has was not set correctly and tools that checked the MD5 hash, such as Azure Data Explorer or Polybase, failed. This issue now has been fixed. 

 2. **CREATE TYPE is fixed**

   U-SQL allows the creation of table types to be used in U-SQL function and procedure signatures. In this release, some meta data catalog issues have been fixed so the type should be usable. Example: 
 
        CREATE TYPE IF NOT EXISTS MyTableType AS TABLE (id int, data string); 
        CREATE FUNCTION IF NOT EXISTS MyTVF(@t MyTableType) RETURNS @r 
        AS
        BEGIN
          @r = SELECT * FROM @t;
        END;
        
