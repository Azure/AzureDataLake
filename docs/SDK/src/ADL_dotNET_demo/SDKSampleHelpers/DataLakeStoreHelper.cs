using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

using Hyak.Common;
using Microsoft.Azure.Management.DataLake.Store;
using Microsoft.Azure.Management.DataLake.Store.Models;
using Microsoft.Azure.Management.DataLake.StoreFileSystem;
using Microsoft.Azure.Management.DataLake.StoreFileSystem.Models;
using Microsoft.Azure.Management.DataLake.StoreUploader;

namespace SDKSampleHelpers
{   
    public static class DataLakeStoreHelper
    {
        public static Dictionary<string, string> ListAccounts(DataLakeStoreManagementClient dataLakeStoreClient)
        {
            Dictionary<string, string> accountList = new Dictionary<string, string>();

            DataLakeStoreAccountListResponse response;
            try
            {
                response = dataLakeStoreClient.DataLakeStoreAccount.List(null, new DataLakeStoreAccountListParameters());
            }
            catch (CloudException)
            {
                return accountList;
            }

            IEnumerable<DataLakeStoreAccount> accountObjects = new List<DataLakeStoreAccount>(response.Value);
            while (!String.IsNullOrWhiteSpace(response.NextLink))
            {
                response = dataLakeStoreClient.DataLakeStoreAccount.ListNext(response.NextLink);
                accountObjects = accountObjects.Concat(response.Value);
            }
            foreach (var acct in accountObjects)
                accountList[acct.Name] = acct.Location;
            return accountList;
        }
        
        public static bool UploadFile(DataLakeStoreFileSystemManagementClient dataLakeStoreFileSystemClient, string dlAccountName, string srcPath, string destPath, bool force = false)
        {
            var parameters = new UploadParameters(srcPath, destPath, dlAccountName, isOverwrite:true);
            var frontend = new DataLakeStoreFrontEndAdapter(dlAccountName, dataLakeStoreFileSystemClient);
            var uploader = new DataLakeStoreUploader(parameters, frontend);

            uploader.Execute();

            return true;
        }

        public static bool AppendBytes(DataLakeStoreFileSystemManagementClient dataLakeStoreFileSystemClient, string dlAccountName, string path, Stream streamContents)
        {
            var response = dataLakeStoreFileSystemClient.FileSystem.BeginAppend(path, dlAccountName, null);
            dataLakeStoreFileSystemClient.FileSystem.Append(response.Location, streamContents);

            return true;
        }

        public static string GetResourceGroupName(DataLakeStoreManagementClient dataLakeStoreClient, string dataLakeStoreAccountName)
        {
            var response = dataLakeStoreClient.DataLakeStoreAccount.List(null, new DataLakeStoreAccountListParameters());

            IEnumerable<DataLakeStoreAccount> accountObjects = new List<DataLakeStoreAccount>(response.Value);
            while (!String.IsNullOrWhiteSpace(response.NextLink))
            {
                response = dataLakeStoreClient.DataLakeStoreAccount.ListNext(response.NextLink);
                accountObjects = accountObjects.Concat(response.Value);
            }

            var acct = accountObjects
                .FirstOrDefault(a => a.Name.Equals(dataLakeStoreAccountName, StringComparison.InvariantCultureIgnoreCase));
            
            Debug.Assert(acct != null, "acct != null");
            var match = Regex.Match(acct.Id, @"resourceGroups/([^/]+)/");

            return match.Groups[1].Value;
        }

        public static List<FileStatusProperties> ListItems(DataLakeStoreFileSystemManagementClient dataLakeStoreFileSystemClient, string dataLakeStoreAccountName, string path)
        {
            var response = dataLakeStoreFileSystemClient.FileSystem.ListFileStatus(path, dataLakeStoreAccountName, new DataLakeStoreFileSystemListParameters());
            return response.FileStatuses.FileStatus.ToList();
        }

        public static void DownloadFile(DataLakeStoreFileSystemManagementClient dataLakeStoreFileSystemClient, 
            string dataLakeStoreAccountName, string srcPath, string destPath, bool force)
        {
            var beginOpenResponse = dataLakeStoreFileSystemClient.FileSystem.BeginOpen(srcPath, dataLakeStoreAccountName,
                new FileOpenParameters());
            var openResponse = dataLakeStoreFileSystemClient.FileSystem.Open(beginOpenResponse.Location);

            if(force || !File.Exists(destPath))
                File.WriteAllBytes(destPath, openResponse.FileContents);
        }

        public static void NewFile(DataLakeStoreFileSystemManagementClient dataLakeStoreFileSystemClient,
            string dataLakeStoreAccountName, string path)
        {
            var beginCreateResponse = dataLakeStoreFileSystemClient.FileSystem.BeginCreate(path, dataLakeStoreAccountName,
                new FileCreateParameters());
            var createResponse = dataLakeStoreFileSystemClient.FileSystem.Create(beginCreateResponse.Location, new MemoryStream());
        }

        public static void Concat(DataLakeStoreFileSystemManagementClient dataLakeStoreFileSystemClient,
            string dataLakeStoreAccountName, string[] srcPaths, string destPath, bool force)
        {
            var concatResponse = dataLakeStoreFileSystemClient.FileSystem.MsConcat(destPath, dataLakeStoreAccountName,
                new MemoryStream());
        }
    }
}