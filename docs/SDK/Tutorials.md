# Get started with Azure Data Lake using the .NET SDK 

Learn how to use the Azure Data Lake .NET SDK and perform common operations.  

## Prerequisites

This guide assumes you have previously followed the steps in the main [Getting Started guide](../GettingStarted.md) and the [SDK First Steps guide](FirstSteps.md).

## 01 - Namespace declations
In order to programatically access Azure Data Lake, add the following namespace declarations:

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Security;
    
    using Microsoft.Azure;
    using Microsoft.Azure.Common.Authentication;
    using Microsoft.Azure.Common.Authentication.Models;
    using Microsoft.Azure.Common.Authentication.Factories;
    using Microsoft.Azure.Management.DataLake;
    using Microsoft.Azure.Management.DataLake.Models;
    using Microsoft.Azure.Management.DataLakeFileSystem;
    using Microsoft.Azure.Management.DataLakeFileSystem.Models;
    using Microsoft.Azure.Management.DataLakeFileSystem.Uploading;
    using Microsoft.IdentityModel.Clients.ActiveDirectory;

## 02 - Create a Data Lake client

There are two main clients:
* DataLakeManagementClient: allows you to manage account operations such as creating, deleting, or updating properties
* DataLakeFileSystemClient: allows you to browse, create, and delete files

To create any Data Lake client you need to provide your Azure credentials via a CloudCredentials object.  Here is an example application that creates two Data Lake clients.

    namespace DataLakeConsoleApp
    {
        class DataLakeConsoleApp
        {
          private static DataLakeManagementClient _dataLakeClient;
          private static DataLakeFileSystemManagementClient _dataLakeFileSystemClient;
          
	        static void Main(string[] args)
	        {
		    var subscriptionId = new Guid("<subID>");
		    var _credentials = GetAccessToken();

		    _credentials = GetCloudCredentials(_credentials, subscriptionId);
		    _dataLakeClient = new DataLakeManagementClient(_credentials);
		    _dataLakeFileSystemClient = new DataLakeFileSystemManagementClient(_credentials);
		}
   

        public static SubscriptionCloudCredentials GetAccessToken(string username = null, SecureString password = null)
        {
            var authFactory = new AuthenticationFactory();

            var account = new AzureAccount { Type = AzureAccount.AccountType.User };

            if (username != null && password != null)
                account.Id = username;

            var env = AzureEnvironment.PublicEnvironments[EnvironmentName.AzureCloud];
            return new TokenCloudCredentials(authFactory.Authenticate(account, env, AuthenticationFactory.CommonAdTenant, password, ShowDialog.Auto).AccessToken);
        }

        public static SubscriptionCloudCredentials GetCloudCredentials(SubscriptionCloudCredentials creds, Guid subId)
        {
            return new TokenCloudCredentials(subId.ToString(), ((TokenCloudCredentials)creds).Token);
        }

      }
   }

## 03 - Example Operations Using the Data Lake Clients 

### Create and/or Delete an Azure Data Lake account

In the Main() function above add the following lines:

	var parameters = new DataLakeAccountCreateOrUpdateParameters();
	parameters.DataLakeAccount = new DataLakeAccount
	{
		Name = "<accountName>",
		Location = "<Azure Region>"
	};
	
	_dataLakeClient.DataLakeAccount.Create("<resourceGroupName>", parameters);
	_dataLakeClient.DataLakeAccount.Delete("<resourceGroupName>", parameters);

### FileSystem Operations

A completed tutorial can be downloaded [here](src/) that will demonstrate how to perform common scenarios such as uploading, downloading, and browsing your files.

The file system client expects local paths to be given like the following, C:\\\folder\\\test.tsv and ADL paths to be given relative to the root directory (e.g. /thisFolder/foo.txt).
            
##### Uploading or Appending to Files

        public static bool UploadFile(DataLakeFileSystemManagementClient dataLakeFileSystemClient, string dlAccountName, string srcPath, string destPath, bool force = false)
        {
            var parameters = new UploadParameters(srcPath, destPath, dlAccountName, isOverwrite: true);
            var frontend = new DataLakeFrontEndAdapter(dlAccountName, dataLakeFileSystemClient);
            var uploader = new DataLakeUploader(parameters, frontend);

            uploader.Execute();

            return true;
        }

        public static bool AppendBytes(DataLakeFileSystemManagementClient dataLakeFileSystemClient, string dlAccountName, string path, Stream streamContents)
        {
            var response = dataLakeFileSystemClient.FileSystem.BeginAppend(path, dlAccountName, null);
            dataLakeFileSystemClient.FileSystem.Append(response.Location, streamContents);

            return true;
        }
    
##### Downloading a File

       public static void DownloadFile(DataLakeFileSystemManagementClient dataLakeFileSystemClient,
            string dataLakeAccountName, string srcPath, string destPath, bool force)
        {
            var beginOpenResponse = dataLakeFileSystemClient.FileSystem.BeginOpen(srcPath, dataLakeAccountName,
                new FileOpenParameters());
            var openResponse = dataLakeFileSystemClient.FileSystem.Open(beginOpenResponse.Location);

            if (force || !File.Exists(destPath))
                File.WriteAllBytes(destPath, openResponse.FileContents);
        }

##### Listing Files

        public static List<FileStatusProperties> ListItems(DataLakeFileSystemManagementClient dataLakeFileSystemClient, string dataLakeAccountName, string path)
        {
            var response = dataLakeFileSystemClient.FileSystem.ListFileStatus(path, dataLakeAccountName, new DataLakeFileSystemListParameters());
            return response.FileStatuses.FileStatus.ToList();
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
