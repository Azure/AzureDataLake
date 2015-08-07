# Get started with Azure Data Lake using the .NET SDK 

Learn how to use the Azure Data Lake .NET SDK and perform common operations.  

## Prerequisites

This guide assumes you have previously followed the steps in the main [Getting Started guide](../GettingStarted.md) and the [SDK First Steps guide](FirstSteps.md).

## Create an Azure Data Lake account

The following parameters are needed to create an Azure Data Lake account.

resourceGroupName:
    The name of the resource group the account will be associated with.
parameters:
     Parameters supplied to the create DataLake account operation.
cancellationToken:
     Cancellation token.

Task<AzureAsyncOperationResponse> BeginCreateAsync(string resourceGroupName, DataLakeAccountCreateOrUpdateParameters parameters, CancellationToken cancellationToken);

[Section incomplete]

## Tutorial

A completed tutorial can be downloaded [here](src/) that will demonstrate how to perform common scenarios such as uploading, downloading, and browsing your files.

The guide below will highlight snippets of the code that enables these scenarios.   

##### 01 - Namespace declarations
Add the following namespace declarations to the top of any C# file in which you wish to programmatically access Azure Data Lake:

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
 

##### 02 - Creating a DataLakeConsoleApp class 
The framework to create the class and initialize static variables and constructors.

    namespace DataLakeConsoleApp
    {
        static class DataLakeConsoleApp
        {
		private static ProfileClient _profileClient;
	        private static SubscriptionCloudCredentials _credentials;

	        private static DataLakeManagementClient _dataLakeClient;
        	private static DataLakeFileSystemManagementClient _dataLakeFileSystemClient;
    
    	        private static void Main(string[] args)
       	        {
			

                }
        }
    }

##### 03 - Signing Into Azure
Demonstrates how to log in with either a username and password or, if blank, an OAuth popup. 

	_profileClient = AzureHelper.GetProfile(username, password);

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

##### 04 - Instantiating management clients

            subId = "12345678-1234-1234-1234-123456789012";
            _credentials = AzureHelper.GetCloudCredentials(_profileClient, new Guid(subId));
            _dataLakeClient = new DataLakeManagementClient(_credentials);
            _dataLakeFileSystemClient = new DataLakeFileSystemManagementClient(_credentials);
            _dataLakeResourceGroupName = DataLakeHelper.GetResourceGroupName(_dataLakeClient, _dataLakeAccountName);
            
##### 05 - Uploading or Appending to Files

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
    
##### 06 - Downloading a File

       public static void DownloadFile(DataLakeFileSystemManagementClient dataLakeFileSystemClient,
            string dataLakeAccountName, string srcPath, string destPath, bool force)
        {
            var beginOpenResponse = dataLakeFileSystemClient.FileSystem.BeginOpen(srcPath, dataLakeAccountName,
                new FileOpenParameters());
            var openResponse = dataLakeFileSystemClient.FileSystem.Open(beginOpenResponse.Location);

            if (force || !File.Exists(destPath))
                File.WriteAllBytes(destPath, openResponse.FileContents);
        }

##### 07 - Listing Files

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
