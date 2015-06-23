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
