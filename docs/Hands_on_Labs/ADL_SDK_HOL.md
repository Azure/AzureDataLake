# Hands-on  Lab: Using the C# .NET SDK to interact with Azure Data Lake

# Introduction

In this lab you'll learn how to use the Data Lake .NET SDK to interact with Azure Data Lake in C#.

# Prerequisites

To complete this lab you'll need:

- Access to an ADL Analytics account name and resource group name (this is provided for you in the classroom)
- Access to an ADL Store account name and resource group name (this is provided for you in the classroom)
- A Windows machine with the Visual Studio 2013 or 2015 installed.

# Exercise 0: Creating the base project

1.  Open Visual Studio and create a new C# Console project.
2.  In Visual Studio, use the NuGet Package Manager to install the following packages:

   -  Microsoft.Azure.Common.Authentication
   -  Microsoft.Azure.Management.DataLake.Store
   -  Microsoft.Azure.Management.DataLake.StoreFilesystem
   -  Microsoft.Azure.Management.DataLake.StoreUploader
   -  Microsoft.Azure.Management.DataLake.Analytics
   -  Microsoft.Azure.Management.DataLake.AnalyticsJob
   -  Microsoft.Azure.Management.DataLake.AnalyticsCatalog

> NOTE: Be sure to check the "Include prerelease" checkbox when searching. If you encounter errors when installing packages, you can [follow the instructions on NuGet.org](http://www.nuget.org/packages?q=Microsoft.Azure.Management.DataLake) for each of the required packages.

3.   Delete any existing code in the Program.cs file and copy in the following code. The code defines a base class that handles some initialization steps, including user authentication:

    ```
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
    ```
4.   Review the code you just added. In particular, note:
     - The constants declared at the start of the class. You will need to update these with values provided by your instructor.
	 - The **Write your code here** comment. This is where you should add code in the exercises that follow.

# Guidance
In the exercises that follow, you will perform various tasks using the C# SDK for Azure Data Lake. This section provides examples and information that may help you to get started.

As an example, here is how you could list all the jobs in your Data Lake Analytics account:

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

For more help and guidance, refer to the following resources.

Tutorials:

* [Get started with Azure Data Lake Analytics using .NET SDK](https://azure.microsoft.com/documentation/articles/data-lake-analytics-get-started-net-sdk/) 
* [Get started with Azure Data Lake Store using .NET SDK](https://azure.microsoft.com/documentation/articles/data-lake-store-get-started-net-sdk/)

Reference material (MSDN):

* [Azure Data Lake Analytics .NET SDK Reference](https://msdn.microsoft.com/library/azure/mt572197.aspx) 

# Exercise 1: List jobs and submit a job
In this exercise you will retrieve a list of all the jobs that have run on your ADLA account. You will then submit a new job and check the job status programmatically.

1.  List all jobs that have run on your ADLA account. Use the method **_dataLakeAnalyticsJobClient.Jobs.List**.
      > Note: You can use the code example presented in the **Guidance** section. 
2.  Submit a new job to your ADLA account. Use the method **_dataLakeAnalyticsJobClient.Jobs.Create**.
      * Use the job type **Usql**.
	  * Use the following script to submit with your job:

            @searchlog = EXTRACT UserId int, Start DateTime, Region string,
			Query string, Duration int, Urls string, ClickedUrls string FROM
			@"/Samples/Data/SearchLog.tsv" USING Extractors.Ts(); OUTPUT
			@searchlog TO @"/Samples/Output/SearchLog_TestOutput.tsv" USING
			Outputters.Tsv();
			
3.  Get the status of the job that you submitted, referencing the job ID you gave in Step 2. Use the method **_dataLakeAnalyticsJobClient.Jobs.Get**.

# Exercise 2: List files and download a file
In this exercise you will retrieve a list of all the files in an output folder. You will also download the output of the job you created in Exercise 1.

1.  List all files in the /Samples/Output/ folder. Use the method **_dataLakeStoreFileSystemClient.FileSystem.ListFileStatus**.
2.  Download the output of the job from Part 2. Use the cmdlet **_dataLakeStoreFileSystemClient.FileSystem.DirectOpen**.
      * Use the ADLS path **/Samples/Output/SearchLog_TestOutput.tsv**.
