# Tutorials: Azure .NET SDK with Data Lake

This guide assumes you have previously followed the steps in the main [Getting Started guide](../GettingStarted.md) and the [SDK First Steps guide](FirstSteps.md).

------------

### Tutorial - How to use Data Lake from .NET

A completed tutorial can be downloaded [here](src/ADL_dotNET_demo.zip) that will demonstrate how to perform common scenarios using the Azure Data Lake service .NET SDK.  The scenarios covered include uploading, downloading, and browsing your files.
The guide below will highlight snippets of the code that enables these scenarios.   

##### 02 - Namespace declarations
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
 

##### 03 - Create a DataLakeConsoleApp 

    namespace DataLakeConsoleApp
    {
        class DataLakeConsoleApp
        {
            private DataLakeManagementClient dlClient;
            private DataLakeFileSystemManagementClient dlFileSystemClient;
    
            static void Main(string[] args)
            {


            }
        }
    }

##### 04 - Sign Into Azure

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

##### 05 - Instantiate management clients

            subId = "12345678-1234-1234-1234-123456789012";
            _credentials = AzureHelper.GetCloudCredentials(_profileClient, new Guid(subId));
            _dataLakeClient = new DataLakeManagementClient(_credentials);
            _dataLakeFileSystemClient = new DataLakeFileSystemManagementClient(_credentials);
            _dataLakeResourceGroupName = DataLakeHelper.GetResourceGroupName(_dataLakeClient, _dataLakeAccountName);
            
##### 06 - Uploading or Appending to Files

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
    
##### 07 - Download File

       public static void DownloadFile(DataLakeFileSystemManagementClient dataLakeFileSystemClient,
            string dataLakeAccountName, string srcPath, string destPath, bool force)
        {
            var beginOpenResponse = dataLakeFileSystemClient.FileSystem.BeginOpen(srcPath, dataLakeAccountName,
                new FileOpenParameters());
            var openResponse = dataLakeFileSystemClient.FileSystem.Open(beginOpenResponse.Location);

            if (force || !File.Exists(destPath))
                File.WriteAllBytes(destPath, openResponse.FileContents);
        }

##### 08 - List Files

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
