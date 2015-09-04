using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

using Hyak.Common;
using Microsoft.Azure;
using Microsoft.Azure.Management.DataLake;
using Microsoft.Azure.Management.DataLake.Models;
using Microsoft.Azure.Management.DataLakeFileSystem;
using Microsoft.Azure.Management.DataLakeFileSystem.Models;
using Microsoft.Azure.Management.DataLakeFileSystem.Uploading;

namespace SDKSampleHelpers
{
    public static class DataLakeHelper
    {
        public static Dictionary<string, string> ListAccounts(DataLakeManagementClient dataLakeClient)
        {
            Dictionary<string, string> accountList = new Dictionary<string, string>();

            DataLakeAccountListResponse response;
            try
            {
                response = dataLakeClient.DataLakeAccount.List(null, new DataLakeAccountListParameters());
            }
            catch (CloudException)
            {
                return accountList;
            }

            IEnumerable<DataLakeAccount> accountObjects = new List<DataLakeAccount>(response.Value);
            while (!String.IsNullOrWhiteSpace(response.NextLink))
            {
                response = dataLakeClient.DataLakeAccount.ListNext(response.NextLink);
                accountObjects = accountObjects.Concat(response.Value);
            }
            foreach (var acct in accountObjects)
                accountList[acct.Name] = acct.Location;
            return accountList;
        }

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

        public static string GetResourceGroupName(DataLakeManagementClient dataLakeClient, string dataLakeAccountName)
        {
            var response = dataLakeClient.DataLakeAccount.List(null, new DataLakeAccountListParameters());

            IEnumerable<DataLakeAccount> accountObjects = new List<DataLakeAccount>(response.Value);
            while (!String.IsNullOrWhiteSpace(response.NextLink))
            {
                response = dataLakeClient.DataLakeAccount.ListNext(response.NextLink);
                accountObjects = accountObjects.Concat(response.Value);
            }

            var acct = accountObjects
                .FirstOrDefault(a => a.Name.Equals(dataLakeAccountName, StringComparison.InvariantCultureIgnoreCase));

            Debug.Assert(acct != null, "acct != null");
            var match = Regex.Match(acct.Id, @"resourceGroups/([^/]+)/");

            return match.Groups[1].Value;
        }

        public static List<FileStatusProperties> ListItems(DataLakeFileSystemManagementClient dataLakeFileSystemClient, string dataLakeAccountName, string path)
        {
            var response = dataLakeFileSystemClient.FileSystem.ListFileStatus(path, dataLakeAccountName, new DataLakeFileSystemListParameters());
            return response.FileStatuses.FileStatus.ToList();
        }

        public static void DownloadFile(DataLakeFileSystemManagementClient dataLakeFileSystemClient,
            string dataLakeAccountName, string srcPath, string destPath, bool force)
        {
            var beginOpenResponse = dataLakeFileSystemClient.FileSystem.BeginOpen(srcPath, dataLakeAccountName,
                new FileOpenParameters());
            var openResponse = dataLakeFileSystemClient.FileSystem.Open(beginOpenResponse.Location);

            if (force || !File.Exists(destPath))
                File.WriteAllBytes(destPath, openResponse.FileContents);
        }

        public static void NewFile(DataLakeFileSystemManagementClient dataLakeFileSystemClient,
            string dataLakeAccountName, string path)
        {
            var beginCreateResponse = dataLakeFileSystemClient.FileSystem.BeginCreate(path, dataLakeAccountName,
                new FileCreateParameters());
            var createResponse = dataLakeFileSystemClient.FileSystem.Create(beginCreateResponse.Location, new MemoryStream());
        }

        public static void Concat(DataLakeFileSystemManagementClient dataLakeFileSystemClient,
            string dataLakeAccountName, string[] srcPaths, string destPath, bool force)
        {
            var concatResponse = dataLakeFileSystemClient.FileSystem.MsConcat(destPath, dataLakeAccountName,
                new MemoryStream());
        }
    }
}