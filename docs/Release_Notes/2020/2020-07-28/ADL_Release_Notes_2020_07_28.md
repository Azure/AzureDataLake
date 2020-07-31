# ADL Release Notes 2020-07-28
--------------------------
## Table of Contents
[Latest component changes](#latest-component-changes)

1. [.NET Framework upgraded from 4.5.2 to 4.7.2](#.NET-Framework-upgraded-from-4.5.2-to-4.7.2)
    
2. [U-SQL runtime version updated to release-20200124live_adl_16283022_2](#U-SQL-runtime-version-updated-to-release-20200124live_adl_16283022_2)



--------------------------

## Latest Component Changes

Azure Data Lake Analytics (ADLA) is updated on an aperiodic basis. We continue to provide the support for this service with certain component update when needed, e.g.: certain shared component update along with other analytic service and so on. 

Check the below content for the component changes in recent update.

#### 1. .NET Framework upgraded from 4.5.2 to 4.7.2

The Azure Data Lake Analytics default runtime .Net Framework has been upgraded from .NET Framework v4.5.2 to .NET Framework v4.7.2. This upgrade from .NET Framework 4.5.2 to version 4.7.2 means that the .NET Framework deployed in a U-SQL runtime (the default runtime) will now always be 4.7.2. (Note: the actual upgrade happens several months ago. This is to record this change in ADL release note for history check.)

For more detailed information regarding this upgrade, refer to [Azure Data Lake Analytics is upgrading to the .NET Framework v4.7.2](https://docs.microsoft.com/en-gb/azure/data-lake-analytics/dotnet-upgrade-troubleshoot)

#### 2. U-SQL runtime version updated to release-20200124live_adl_16283022_2    

The Azure Data Lake U-SQL runtime, including the compiler, optimizer, and job manager, is what processes your U-SQL code.

The default runtime version in production environment has been updated to **release-20200124live_adl_16283022_2** on 6/24/2020. This runtime version update is a general update. It is mainly for other analytic services which have the shared runtime component. There should be no impact to Azure Data Lake Analytics service with this update.

  
