# Get started with Azure Data Lake using the .NET SDK 

Learn how to use the Azure Data Lake Store .NET SDK and perform common operations.  

## Prerequisites

This guide assumes you have previously followed the steps in the main [Getting Started guide](../GettingStarted.md) and the [SDK First Steps guide](FirstSteps.md).

## 01 - Namespace declations
In order to programatically access Azure Data Lake Store, add the following namespace declarations:

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    
    using Microsoft.Azure;
    using Microsoft.Azure.Common.Authentication;
    using Microsoft.Azure.Common.Authentication.Models;
    using Microsoft.Azure.Management.DataLake.Store;
    using Microsoft.Azure.Management.DataLake.Store.Models;
    using Microsoft.Azure.Management.DataLake.StoreFileSystem;
    using Microsoft.Azure.Management.DataLake.StoreFileSystem.Models;
    using Microsoft.Azure.Management.DataLake.StoreUploader;
    using Microsoft.IdentityModel.Clients.ActiveDirectory;

## 02 - Create a Data Lake Store client

There are two main clients:
* DataLakeStoreManagementClient: allows you to manage account operations such as creating, deleting, or updating properties
* DataLakeStoreFileSystemClient: allows you to browse, create, and delete files

To create the Data Lake Store clients you need to provide your Azure credentials via a SubscriptionCloudCredentials object.  The SubscriptionCloudCredentials object requires a profile client obtained with your username, password and subscription ID.

Here is an example application that creates the two Data Lake clients.

    namespace DataLakeConsoleApp
    {
        class DataLakeConsoleApp
        {
          private static DataLakeManagementClient _dataLakeClient;
          private static DataLakeFileSystemManagementClient _dataLakeFileSystemClient;
          
	        static void Main(string[] args)
	        {
	            var profileClient = GetProfile();
         	    var _credentials = GetCloudCredentials(profileClient, subscriptionId);
		    
				_dataLakeClient = new DataLakeManagementClient(_credentials);
				_dataLakeFileSystemClient = new DataLakeFileSystemManagementClient(_credentials);
			}
   

        public static ProfileClient GetProfile(string username = null, SecureString password = null)
        {
            var pClient = new ProfileClient(new AzureProfile());
            var env = pClient.GetEnvironmentOrDefault(EnvironmentName.AzureCloud);
            var acct = new AzureAccount { Type = AzureAccount.AccountType.User };

            if (username != null && password != null)
                acct.Id = username;

            pClient.AddAccountAndLoadSubscriptions(acct, env, password);

            return pClient;
        }

        private static SubscriptionCloudCredentials GetCloudCredentials(ProfileClient profileClient, Guid subscriptionId)
        {
            var sub = profileClient.Profile.Subscriptions.Values.FirstOrDefault(s => s.Id.Equals(subscriptionId));

            Debug.Assert(sub != null, "subscription != null");
            profileClient.SetSubscriptionAsDefault(sub.Id, sub.Account);

            return AzureSession.AuthenticationFactory.GetSubscriptionCloudCredentials(profileClient.Profile.Context);
        }
    }

## 03 - Example Operations Using the Data Lake Store Clients 

### Create and/or Delete an Azure Data Lake Store account

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

------------

### Useful links

Browse the following pages:

* [Getting Started](../GettingStarted.md)
* Tools
    * [Azure Portal](../AzurePortal/FirstSteps.md)
    * [PowerShell](../PowerShell/FirstSteps.md)
    * [SDK](../SDK/FirstSteps.md)
