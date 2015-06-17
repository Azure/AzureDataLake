# Tutorials: Azure .NET SDK with Data Lake

This guide assumes you have previously followed the steps in the main [Getting Started guide](../GettingStarted.md) and the [SDK First Steps guide](FirstSteps.md).

------------

### Tutorials

#### 01 - Creating the DataLakeConsoleApp class

Now that you've set up your C# project and [added the necessary NuGet packages](FirstSteps.md) for Data Lake, let's create the DataLakeConsoleApp:

    using Microsoft.Azure;
    using Microsoft.Azure.Common.Authentication;
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
    
            private string subscriptionID;
            private string resourceGroupName;
            private const string locationStr = "East US 2";

            public DataLakeConsoleApp(string subID, string rgName)
            {
                this.subscriptionID = subID;
                this.resourceGroupName = rgName;
            }
        }
    }

#### 02 - Signing Into Azure and Instantiating the Management Clients

To authenticate each call to Azure, you need to have an object of type ``SubscriptionCloudCredentials``. Let's set this up now.


            ...

            public TokenCloudCredentials GetCloudCredentials()
            {
                string accessToken;
    
                lock (this)
                {
                    AuthenticationContext context = new AuthenticationContext("https://login.windows.net/common/");
    
                    AuthenticationResult authResult = context.AcquireToken("https://management.core.windows.net/",
                                                                           "1950a258-227b-4e31-a9cf-717495945fc2",
                                                                           new Uri("urn:ietf:wg:oauth:2.0:oob"));
                    if (authResult == null)
                        throw new InvalidOperationException("Failed to obtain the access token");
    
                    accessToken = authResult.AccessToken;
                }
    
                return new TokenCloudCredentials(subscriptionID, accessToken);
            }

            public DataLakeConsoleApp(string subID, string rgName)
            {
                this.subscriptionID = subID;
                this.resourceGroupName = rgName;

                TokenCloudCredentials cc = GetCloudCredentials();
                
                this.dlClient = new DataLakeManagementClient(cc);
                this.dlFileSystemClient = new DataLakeFileSystemManagementClient(cc);
            }

            ...
            
#### Uploading or Appending to Files

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
            
            ...

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
