using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            DataLakeConsoleApp app = new DataLakeConsoleApp("00000000-1111-2222-3333-444444444444", "myresourcegroup");

            app.UploadFile("mydatalake", @"C:\foo.txt", @"foo_uploaded.txt");

            System.Console.ReadKey();
        }
    }
}