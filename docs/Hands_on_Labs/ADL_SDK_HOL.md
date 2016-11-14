# Hands-on Lab: Building a Microsoft Azure Data Lake application with Visual C♯

# Introduction

In this lab you will learn how to use the Data Lake .NET SDK to interact with Azure Data Lake in C#.


# Getting started


## Prerequisites

Before you can start the lab exercises, you will need various Azure services provisioned for you. Follow the instructions here: [Start](Start.md). 

This process only takes a few minutes. Once your services are configured you can proceed with the lab.

# More info

For more help and guidance, refer to the following resources.

Tutorials:

* [Get started with Azure Data Lake Analytics using .NET SDK](https://azure.microsoft.com/documentation/articles/data-lake-analytics-get-started-net-sdk/) 
* [Get started with Azure Data Lake Store using .NET SDK](https://azure.microsoft.com/documentation/articles/data-lake-store-get-started-net-sdk/)

Reference material (MSDN):

* [Azure Data Lake Analytics .NET SDK Reference](https://msdn.microsoft.com/library/azure/mt572197.aspx) 

# Exercise 0: Creating the base project

1.  Open Visual Studio and create a new C# Console Application.
    - On the **File** menu, point to **New**, and then click **Project**.
    - In the New Project dialog box, in the navigation pane, expand **Templates**, and then click **Visual C#**.
    - In the center pane, click **Console Application**.
    - Enter any **Name** you want, and then click **OK**.
    
2.  In Visual Studio, go to Tools -> NuGet Package Manager -> Package Manager Console to install some NuGet packages.

     Enter the following, one by one, to install the required packages:
   
            Install-Package Microsoft.Azure.Management.DataLake.Analytics –Pre
            
            Install-Package Microsoft.Azure.Management.DataLake.Store –Pre
            
            Install-Package Microsoft.Azure.Management.DataLake.StoreUploader –Pre
            
            Install-Package Microsoft.Rest.ClientRuntime.Azure.Authentication -Pre
       
      > Note: If you encounter errors when installing packages, you can [follow the instructions on NuGet.org](http://www.nuget.org/packages?q=Microsoft.Azure.Management.DataLake) for each of the required packages.

3.   Delete any existing code in the Program.cs file and copy in the following code. The code defines a base class that handles some initialization steps, including user authentication:

    ```
    using System;
    using System.IO;
    using System.Security;
    using System.Text;
    using System.Collections.Generic;
    using System.Linq;
    
    using Microsoft.Azure.Management.DataLake.Store;
    using Microsoft.Azure.Management.DataLake.Store.Models;
    using Microsoft.Azure.Management.DataLake.StoreUploader;
    using Microsoft.Azure.Management.DataLake.Analytics;
    using Microsoft.Azure.Management.DataLake.Analytics.Models;
    using Microsoft.IdentityModel.Clients.ActiveDirectory;
    using Microsoft.Rest;
    
    namespace SdkSample
    {
        class Program
        {
            private static void Main(string[] args)
            {
                ////////////// Initial values //////////////
                string subscriptionId = "<YOUR-SUB-GUID>";
    
                var tenantId = "common"; //Replace this if you know your Tenant ID.
                var appClientId = "fbc174f6-bfd2-423a-b72a-e9393e789c21"; // Replace this if you have a registered Azure AD Application.
                var appRedirectUri = new Uri("https://localhost"); // Replace this if you have a registered Azure AD Application.
    
                var adlsAccountName = "adlanalyticsaccount";
                var adlaAccountName = "adlstoreaccount";
                var location = "East US 2";
    
                // TODO: Make sure these local paths exist and can be overwritten.
                var localFolderPath = @"C:\adldemo\";
                var localFilePath = @"C:\adldemo\file.txt";
    
                var remoteFolderPath = "/data_lake_path/";
                var remoteFilePath = remoteFolderPath + "file.txt";
    
                ////////////// Authentication //////////////
                Console.Write("Authenticating the end-user... ");
                var resource = "https://management.core.windows.net/";
                var authContext = new AuthenticationContext("https://login.microsoftonline.com/" + tenantId);
                authContext.AcquireToken(resource, appClientId, appRedirectUri, PromptBehavior.Auto);
                var tokenProvider = new UserTokenProvider(authContext, appClientId, new Uri(resource), UserIdentifier.AnyUser);
                var tokenCreds = new TokenCredentials(tokenProvider);
                Console.WriteLine("done. Press any key to continue.");
                Console.ReadKey(true);

                //////////////////  Service-to-service (alternative) //////////////
                /* 
                    Console.Write("Authenticating the application... ");
                    var clientSecret = GetSecureStringClientSecret();
                    var authContext = new AuthenticationContext("https://login.microsoftonline.com/" + tenantId);
                    var credential = new ClientCredential(appClientId, clientSecret);
                    var tokenAuthResult = authContext.AcquireToken(resource, credential);
                    var tokenProvider = new ApplicationTokenProvider(authContext, resource, credential, tokenAuthResult);
                    var tokenCreds = TokenCredentials(tokenProvider);
                    Console.WriteLine("done. Press any key to continue.");
                    Console.ReadKey(true);;
                */
    
                ////////////// Create clients //////////////
                Console.Write("Creating clients... ");
                var adlsClient = new DataLakeStoreAccountManagementClient(tokenCreds);
                var adlsFileSystemClient = new DataLakeStoreFileSystemManagementClient(tokenCreds);
                var adlaClient = new DataLakeAnalyticsAccountManagementClient(tokenCreds);
                var adlaJobClient = new DataLakeAnalyticsJobManagementClient(tokenCreds);
                var adlaCatalogClient = new DataLakeAnalyticsCatalogManagementClient(tokenCreds);
                adlsClient.SubscriptionId = subscriptionId;
                adlaClient.SubscriptionId = subscriptionId;
                Console.WriteLine("done. Press any key to continue.");
                Console.ReadKey(true);
    
                // Write your code here
    
                Console.WriteLine("Done. Press any key.");
                Console.ReadKey(true);
            }
        }
    }
    ```
    
4.   Review the code you just added. In particular, note:
     - The variables declared at the start of the class. You will need to update these with values provided by the registration website.
	 - The **Write your code here** comment. This is where you should add code in the exercises that follow.

# Guidance
In the exercises that follow, you will perform various tasks using the C# SDK for Azure Data Lake. This section provides examples and information that may help you to get started.

As an example, here is how you could list all the jobs in your Data Lake Analytics account:

     var response = adlaJobClient.Job.List(adlaAccountName);
     var jobs = new List<JobInformation>(response);
     
     foreach (var job in response)
     {
          Console.WriteLine(job.JobId + job.Name);
     }
     
     // If there are more results, fetch them
     while (response.NextPageLink != null)
     {
          response = adlaJobClient.Job.ListNext(response.NextPageLink);
          jobs.AddRange(response);
          
          foreach (var job in response)
          {
               Console.WriteLine(job.JobId + job.Name);
          }
     }

# Exercise 1: List jobs and submit a job

In this exercise you will retrieve a list of all the jobs that have run on your ADLA account. You will then submit a new job and check the job status programmatically.

1.  List all jobs that have run on your ADLA account. Use the method **_dataLakeAnalyticsJobClient.Jobs.List**.
      > Note: You can use the code example presented in the **Guidance** section. 
2.  Submit a new job to your ADLA account. Use the method **_dataLakeAnalyticsJobClient.Jobs.Create**.
      * Save the following script in a new file.

                EXTRACT UserId int, Start DateTime, Region string,
                Query string, Duration int, Urls string, ClickedUrls string FROM
                @"/Samples/Data/SearchLog.tsv" USING Extractors.Tsv(); OUTPUT
                @searchlog TO @"/Samples/Output/UserName/SearchLog_TestOutput.tsv" USING
                Outputters.Tsv();
		
      * Use the job type **Usql**:

                var script = File.ReadAllText(scriptPath);
                var jobId = Guid.NewGuid();
                var properties = new USqlJobProperties(script);
                var parameters = new JobInformation("New Job", JobType.USql, properties, priority: 1000, degreeOfParallelism: 1);
                
                var jobInfo = adlaJobClient.Job.Create(jobId, parameters, _adlaAccountName);
			
3.  Get the status of the job that you submitted, referencing the job ID you gave in Step 2. Use the method **_dataLakeAnalyticsJobClient.Jobs.Get**.

# Exercise 2: List files and download a file
In this exercise you will retrieve a list of all the files in an output folder. You will also download the output of the job you created in Exercise 1.

1.  List all files in the /Samples/Output/ folder. Use the method **_dataLakeStoreFileSystemClient.FileSystem.ListFileStatus**.
2.  Download the output of the job from Part 2. Use the cmdlet **_dataLakeStoreFileSystemClient.FileSystem.DirectOpen**.
      * Use the ADLS path **/Samples/Output/UserName/SearchLog_TestOutput.tsv**.
