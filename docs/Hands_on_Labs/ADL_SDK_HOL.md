# Hands on  Lab: Using the C# .NET SDK to interact with Azure Data Lake

# Introduction

In this lab you'll learn how to use the Data Lake .NET SDK to interact with Azure Data Lake in C#.

# Prerequisites

To perform this lab you'll need:

- Access to an ADL Analytics account name and resource group name (this is provided for you in the lab)
- Access to an ADL Store account name and resource group name (this is provided for you in the lab)
- A Windows machine with the Visual Studio 2013 or 2015 installed.

# Part 0 - Creating the base project

1.  Create a new C# Console project.
2.  Install the following packages using the NuGet package manager in Visual Studio:

   *  Microsoft.Azure.Common.Authentication
   *  Microsoft.Azure.Management.DataLake.Store
   *  Microsoft.Azure.Management.DataLake.StoreFilesystem
   *  Microsoft.Azure.Management.DataLake.StoreUploader
   *  Microsoft.Azure.Management.DataLake.Analytics
   *  Microsoft.Azure.Management.DataLake.AnalyticsJob
   *  Microsoft.Azure.Management.DataLake.AnalyticsCatalog

> NOTE: Be sure to check the "Include prerelease" checkbox when searching. If the above does not work for you, you can [follow the instructions on NuGet.org](http://www.nuget.org/packages?q=Microsoft.Azure.Management.DataLake) for each of the needed packages.

Here's a base class that handles the initialization steps, including user authentication:

    using System;
    using System.Security;
    using System.IO;
    
    using Microsoft.Azure;
    using Microsoft.Azure.Common.Authentication;
    using Microsoft.Azure.Common.Authentication.Factories;
    using Microsoft.Azure.Common.Authentication.Models;

    using Microsoft.Azure.Management.DataLake.Store;
    using Microsoft.Azure.Management.DataLake.Store.Models;
    using Microsoft.Azure.Management.DataLake.StoreFileSystem;
    using Microsoft.Azure.Management.DataLake.StoreFileSystem.Models;
    using Microsoft.Azure.Management.DataLake.StoreUploader;

    using Microsoft.Azure.Management.DataLake.Analytics;
    using Microsoft.Azure.Management.DataLake.Analytics.Models;
    using Microsoft.Azure.Management.DataLake.AnalyticsJob;
    using Microsoft.Azure.Management.DataLake.AnalyticsJob.Models;
    using Microsoft.Azure.Management.DataLake.AnalyticsCatalog;
    using Microsoft.Azure.Management.DataLake.AnalyticsCatalog.Models;
    
    namespace data_lake_analytics_get_started
    {
        class Program
        {
            private const string AzureSubscriptionID = "<Your Azure Subscription ID>";
            private const string ResourceGroupName = "<Your Azure Resource Group Name>";
            private const string Location = "East US 2";

            private const string DataLakeStoreAccountName = "<Data Lake Store Account Name>";
            private const string DataLakeAnalyticsAccountName = "<Data Lake Analytics Account Name>";
    
            private const string LocalFolder = @"C:\tutorials\downloads\";  //local folder with write permission for file transferring.
    
            private static DataLakeStoreManagementClient _dataLakeStoreClient;
            private static DataLakeStoreFileSystemManagementClient _dataLakeStoreFileSystemClient;
    
            private static DataLakeAnalyticsManagementClient _dataLakeAnalyticsClient;
            private static DataLakeAnalyticsJobManagementClient _dataLakeAnalyticsJobClient;
    
            static void Main(string[] args)
            {
                var subscriptionId = new Guid(AzureSubscriptionID);
                var _credentials = GetAccessToken();
    
                _credentials = GetCloudCredentials(_credentials, subscriptionId);

                _dataLakeStoreClient = new DataLakeStoreManagementClient(_credentials);
                _dataLakeStoreFileSystemClient = new DataLakeStoreFileSystemManagementClient(_credentials);

                _dataLakeAnalyticsClient = new DataLakeAnalyticsManagementClient(_credentials);
                _dataLakeAnalyticsJobClient = new DataLakeAnalyticsJobManagementClient(_credentials);
    
                // Write your code here
    
                Console.WriteLine("Done. Press any key.");
                Console.ReadKey(true);
            }
    
            // Authenticate
            public static SubscriptionCloudCredentials GetAccessToken()
            {
                var authFactory = new AuthenticationFactory();
                var account = new AzureAccount { Type = AzureAccount.AccountType.User };    
                var env = AzureEnvironment.PublicEnvironments[EnvironmentName.AzureCloud];

                return new TokenCloudCredentials(authFactory.Authenticate(account, env, AuthenticationFactory.CommonAdTenant, null, ShowDialog.Auto).AccessToken);
            }
    
            // Add subscription ID to the token object
            public static SubscriptionCloudCredentials GetCloudCredentials(SubscriptionCloudCredentials creds, Guid subId)
            {
                return new TokenCloudCredentials(subId.ToString(), ((TokenCloudCredentials)creds).Token);
            }
        }
    }

# Part 1 - Learning about the .NET SDK and an example

For an example, here is how you could list all jobs in your Data Lake Analytics account:

     var parameters = new JobListParameters{};
     var response = _dataLakeAnalyticsJobClient.Jobs.List(ResourceGroupName, DataLakeAnalyticsAccountName, parameters);
     
     // Fetch first page of results
     foreach (var job in response.Value)
     {
        Console.WriteLine(job.JobId + job.Name);
     }

     // If there are more results, fetch them
     while (!String.IsNullOrWhiteSpace(response.NextLink))
     {
        response = _dataLakeAnalyticsJobClient.Jobs.ListNext(response.NextLink, ResourceGroupName);

        foreach (var job in response.Value)
        {
            Console.WriteLine(job.JobId + job.Name);
        }
     }

To get detailed help, with examples:

* Documentation articles: [ADLA](https://azure.microsoft.com/en-us/documentation/articles/data-lake-analytics-get-started-net-sdk/) [ADLS](https://azure.microsoft.com/en-us/documentation/articles/data-lake-store-get-started-net-sdk/)
* MSDN reference material: [ADLA](https://msdn.microsoft.com/en-US/library/azure/mt572197(Azure.100).aspx) [ADLS](https://msdn.microsoft.com/library/azure/mt581387.aspx)

# Part 2 - List jobs and submit a job

1.  List all jobs that have run on your ADLA account. Use the method ``_dataLakeAnalyticsJobClient.Jobs.List``.
      *  Note: This step is already covered for you above. 
2.  Submit a new job to your ADLA account. Use the method ``_dataLakeAnalyticsJobClient.Jobs.Create``.
      * Here's the contents of the script you should submit with your job. The job type is "USql".

                @searchlog = EXTRACT UserId int, Start DateTime, Region string, Query string, Duration int, Urls string, ClickedUrls string FROM @"/Samples/Data/SearchLog.tsv" USING Extractors.Ts(); OUTPUT @searchlog TO @"/Samples/Output/SearchLog_TestOutput.tsv" USING Outputters.Tsv();

3.  Get the status of the job that you submitted, referencing the job ID you gave in Step 2. Use the method ``_dataLakeAnalyticsJobClient.Jobs.Get``.

# Part 3 - List files and download a file

1.  List all files in the /Samples/Output/ folder. Use the method ``_dataLakeStoreFileSystemClient.FileSystem.ListFileStatus``.
2.  Download the output of the job from Part 2. Use the cmdlet ``_dataLakeStoreFileSystemClient.FileSystem.DirectOpen``.
      * The ADLS path to use is:   /Samples/Output/SearchLog_TestOutput.tsv
