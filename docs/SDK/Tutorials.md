# Tutorials: Azure .NET SDK with Data Lake

This guide assumes you have previously followed the steps in the main [Getting Started guide](../GettingStarted.md) and the [SDK First Steps guide](FirstSteps.md).

------------

### Tutorials

#### Tutorial 1 - DataLakeConsoleApp

This tutorial will focus on a class we're creating called ``DataLakeConsoleApp``.

You can download the file with the completed tutorial [here](src/DataLakeConsoleApp.cs).

##### 01 - Creating the DataLakeConsoleApp class

Now that you've set up your C# project and [added the necessary NuGet packages](FirstSteps.md) for Data Lake, let's create ``DataLakeConsoleApp``:

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    
    using Microsoft.Azure;
    using Microsoft.Azure.Common.Authentication;
    using Microsoft.Azure.Common.Authentication.Models;
    using Microsoft.Azure.Management.DataLake;
    using Microsoft.Azure.Management.DataLake.Models;
    using Microsoft.Azure.Management.DataLakeFileSystem;
    using Microsoft.Azure.Management.DataLakeFileSystem.Models;
    using Microsoft.Azure.Management.DataLakeFileSystem.Uploading;
    using Microsoft.IdentityModel.Clients.ActiveDirectory;
    
    namespace DataLakeConsoleApp
    {
        class DataLakeConsoleApp
        {
            private DataLakeManagementClient dlClient;
            private DataLakeFileSystemManagementClient dlFileSystemClient;
    
            public DataLakeConsoleApp(Guid subID)
            {
            }
    
            static void Main(string[] args)
            {
                DataLakeConsoleApp app = new DataLakeConsoleApp(new Guid("12345678-1234-1234-1234-123456789012"));
    
                System.Console.ReadKey();
            }
        }
    }

##### 02 - Signing Into Azure and Instantiating the Management Clients

Modify the ``DataLakeConsoleApp`` constructor and add the ``InitializeClients`` method.

            ...

            public DataLakeConsoleApp(Guid subID)
            {
                InitializeClients(subID);
            }
    
            public void InitializeClients(Guid subID)
            {
                ProfileClient profileClient = new ProfileClient(new AzureProfile());
                AzureEnvironment env = profileClient.GetEnvironmentOrDefault(EnvironmentName.AzureCloud);
                AzureAccount acct = new AzureAccount { Type = AzureAccount.AccountType.User };
    
                profileClient.InitializeProfile(env, subID, acct, null, "");
    
                AzureSubscription subscription = profileClient.Profile.Subscriptions.Values.FirstOrDefault(s => s.Id.Equals(subID));
    
                profileClient.SetSubscriptionAsDefault(subscription.Id, subscription.Account);
    
                SubscriptionCloudCredentials credentials = AzureSession.AuthenticationFactory.GetSubscriptionCloudCredentials(profileClient.Profile.Context);
    
                // Instantiate clients
                this.dlClient = new DataLakeManagementClient(credentials, profileClient.Profile.Context.Environment.GetEndpointAsUri(AzureEnvironment.Endpoint.ResourceManager));
                this.dlFileSystemClient = new DataLakeFileSystemManagementClient(credentials, profileClient.Profile.Context.Environment.GetEndpoint(AzureEnvironment.Endpoint.AzureDataLakeFileSystemEndpointSuffix));
            }

            ...
            
##### 03 - Uploading or Appending to Files

Let's add the ``UploadFile`` and ``AppendBytes`` methods and modify our Main method.

            ...
            
            public bool UploadFile(string dlAccountName, string srcPath, string destPath)
            {
                UploadParameters parameters = new UploadParameters(srcPath, destPath, dlAccountName);
                DataLakeFrontEndAdapter frontend = new DataLakeFrontEndAdapter(dlAccountName, dlFileSystemClient);
                DataLakeUploader uploader = new DataLakeUploader(parameters, frontend);
                uploader.Execute();
    
                return true;
            }
    
            public bool AppendBytes(string dlAccountName, string path, System.IO.Stream streamContents)
            {
                var response = dlFileSystemClient.FileSystem.BeginAppend(path, dlAccountName, null);
                dlFileSystemClient.FileSystem.Append(response.Location, streamContents);
                return true;
            }
    
            static void Main(string[] args)
            {
                DataLakeConsoleApp app = new DataLakeConsoleApp(new Guid("12345678-1234-1234-1234-123456789012"));
    
                app.UploadFile("mydatalakeaccount", @"C:\foo.txt", @"foo_uploaded.txt");
    
                System.Console.ReadKey();
            }
        }
    }

#### Learn more
* [SDK User Manual](UserManual.md) - View some basic documentation for the Azure Data Lake .NET SDK.

------------

### Useful links

Browse the following pages:

* [Getting Started](../GettingStarted.md)
* Tools
    * [Azure Portal](../AzurePortal/FirstSteps.md)
    * [PowerShell](../PowerShell/FirstSteps.md)
    * [SDK](../SDK/FirstSteps.md)
