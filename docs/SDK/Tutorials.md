# Get started with Azure Data Lake using the .NET SDK 

Learn how to use the Azure Data Lake Store .NET SDK and perform common operations.  

## Prerequisites

This guide assumes you have previously followed the steps in the main [Getting Started guide](../GettingStarted.md) and the [SDK First Steps guide](FirstSteps.md).

## 01 - Namespace declations
In order to programatically access Azure Data Lake Store, add the following namespace declarations:

    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Security;

    using Microsoft.Azure;
    using Microsoft.Azure.Common.Authentication;
    using Microsoft.Azure.Management.DataLake.Store;
    using Microsoft.Azure.Management.DataLake.StoreFileSystem;
    using Microsoft.Azure.Management.DataLake.StoreFileSystem.Models;

## 02 - Create a Data Lake Store client

There are two main clients:
* DataLakeStoreManagementClient: allows you to manage account operations such as creating, deleting, or updating properties
* DataLakeStoreFileSystemManagementClient: allows you to browse, create, and delete files

To create the Data Lake Store clients you need to provide your Azure credentials via a SubscriptionCloudCredentials object.  The SubscriptionCloudCredentials object requires a profile client obtained with your username, password and subscription ID.

Here is an example application that creates the two Data Lake clients.

    namespace DataLakeConsoleApp
    {
        class DataLakeConsoleApp
        {
            private static SubscriptionCloudCredentials _credentials;
            private static DataLakeStoreManagementClient _dataLakeStoreClient;
            private static DataLakeStoreFileSystemManagementClient _dataLakeStoreFileSystemClient;
            private static IAccessToken _accessToken;

            public static void Main(string[] args)
            {
                var subscriptionId = new Guid("<subID>");
                var accessToken = GetAccessToken();

                _credentials = GetCloudCredentials(accessToken);
                _credentials = GetCloudCredentials(_credentials, subscriptionId);

                _dataLakeStoreClient = new DataLakeStoreManagementClient(_credentials);
                _dataLakeStoreFileSystemClient = new DataLakeStoreFileSystemManagementClient(_credentials);
            }

            public static IAccessToken GetAccessToken(string username = null, SecureString password = null)
            {
                var authFactory = new AuthenticationFactory();

                var account = new AzureAccount { Type = AzureAccount.AccountType.User };

                if (username != null && password != null)
                    account.Id = username;

                var env = AzureEnvironment.PublicEnvironments[EnvironmentName.AzureCloud];

                return authFactory.Authenticate(account, env, AuthenticationFactory.CommonAdTenant, password, ShowDialog.Auto);
            }

            public static IAccessToken GetAccessToken(Guid applicationId, Guid tenantId, SecureString password)
            {
                var authFactory = new AuthenticationFactory();

                var account = new AzureAccount { Type = AzureAccount.AccountType.ServicePrincipal, Id = applicationId.ToString() };

                var env = AzureEnvironment.PublicEnvironments[EnvironmentName.AzureCloud];

                return authFactory.Authenticate(account, env, tenantId.ToString(), password, ShowDialog.Never);
            }

            public static SubscriptionCloudCredentials GetCloudCredentials(IAccessToken accessToken)
            {
                return new TokenCloudCredentials(accessToken.AccessToken);
            }

            public static SubscriptionCloudCredentials GetCloudCredentials(SubscriptionCloudCredentials creds, Guid subId)
            {
                return new TokenCloudCredentials(subId.ToString(), ((TokenCloudCredentials)creds).Token);
            }
        }
    }


## 03 - Example Operations Using the Data Lake Store Clients 

### Create and/or Delete an Azure Data Lake Store account

In the Main() function above add the following lines:

    var parameters = new DataLakeStoreAccountCreateOrUpdateParameters();
    parameters.DataLakeStoreAccount = new DataLakeStoreAccount
    {
        Name = "<Account Name>",
        Location = "<Azure Location>"
    };

    _dataLakeStoreClient.DataLakeStoreAccount.Create("<resourceGroupName>", parameters);
    _dataLakeStoreClient.DataLakeStoreAccount.Delete("<resourceGroupName>", "<accountName>");

### FileSystem Operations

A completed tutorial can be downloaded [here](src/) that will demonstrate how to perform common scenarios such as uploading, downloading, and browsing your files.

The file system client expects local paths to be given like the following, C:\\\folder\\\test.tsv and ADL paths to be given relative to the root directory (e.g. /thisFolder/foo.txt).
            
##### Uploading or Appending to Files

    public static bool UploadFile(DataLakeStoreFileSystemManagementClient dataLakeStoreFileSystemClient, string dlStoreAccountName, string srcPath, string destPath, bool force = false)
    {
        var parameters = new UploadParameters(srcPath, destPath, dlStoreAccountName, isOverwrite: true);
        var frontend = new DataLakeStoreFrontEndAdapter(dlStoreAccountName, dataLakeStoreFileSystemClient);
        var uploader = new DataLakeStoreUploader(parameters, frontend);

        uploader.Execute();

        return true;
    }

    public static bool AppendBytes(DataLakeStoreFileSystemManagementClient dataLakeStoreFileSystemClient, string dlStoreAccountName, string path, Stream streamContents)
    {
        var response = dataLakeStoreFileSystemClient.FileSystem.BeginAppend(path, dlStoreAccountName, null);
        dataLakeStoreFileSystemClient.FileSystem.Append(response.Location, streamContents);

        return true;
    }
    
##### Downloading a File

    public static void DownloadFile(DataLakeStoreFileSystemManagementClient dataLakeStoreFileSystemClient,
        string dataLakeStoreAccountName, string srcPath, string destPath, bool force)
    {
        var beginOpenResponse = dataLakeStoreFileSystemClient.FileSystem.BeginOpen(srcPath, dataLakeStoreAccountName,
            new FileOpenParameters());
        var openResponse = dataLakeStoreFileSystemClient.FileSystem.Open(beginOpenResponse.Location);

        if (force || !File.Exists(destPath))
            File.WriteAllBytes(destPath, openResponse.FileContents);
    }

##### Listing Files

    public static List<FileStatusProperties> ListItems(DataLakeStoreFileSystemManagementClient dataLakeStoreFileSystemClient, string dataLakeStoreAccountName, string path)
    {
        var response = dataLakeStoreFileSystemClient.FileSystem.ListFileStatus(path, dataLakeStoreAccountName, new DataLakeStoreFileSystemListParameters());
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
