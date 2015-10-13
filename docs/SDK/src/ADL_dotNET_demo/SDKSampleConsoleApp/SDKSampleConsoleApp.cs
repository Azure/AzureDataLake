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
using SDKSampleHelpers;

namespace SDKSampleConsoleApp
{
    static class SDKSampleConsoleApp
    {
        private static SubscriptionCloudCredentials _credentials;

        private static DataLakeStoreManagementClient _dataLakeStoreClient;
        private static DataLakeStoreFileSystemManagementClient _dataLakeStoreFileSystemClient;

        private static string _dataLakeStoreAccountName;
        private static string _dataLakeStoreResourceGroupName;
        private static IAccessToken _accessToken;

        private static void Main(string[] args)
        {
            Login();

            GetAccounts();

            MainMenu();

            Done();
        }

        private static void Login()
        {
            var username =
                ConsolePrompts.Prompt("Enter your username. To show the OAuth popup instead, leave this blank.");
            SecureString password = null;
            if (username != null)
                password = ConsolePrompts.SecurePrompt("Enter your password.", true);
            else
                Console.WriteLine("Showing an OAuth popup.\r\n");

            Console.WriteLine("Logging you in... please wait.");
            _accessToken = AzureHelper.GetAccessToken(username, password);
            _credentials = AzureHelper.GetCloudCredentials(_accessToken);
        }

        private static void GetAccounts()
        {
            var subId =
                ConsolePrompts.MenuPrompt("Select a subscription.\r\nTo create a new subscription, visit:"
                                          + "\r\n   https://account.windowsazure.com/Subscriptions"
                                          + "\r\nIf you're not sure which subscription to pick, leave this blank.",
                    AzureHelper.GetSubscriptions(_credentials));

            _credentials = AzureHelper.GetCloudCredentials(_credentials, new Guid(subId));
            _dataLakeStoreClient = new DataLakeStoreManagementClient(_credentials);
            _dataLakeStoreFileSystemClient = new DataLakeStoreFileSystemManagementClient(_credentials);

            if (!string.IsNullOrWhiteSpace(subId))
            {
                _dataLakeStoreAccountName =
                    ConsolePrompts.MenuPrompt("Select your Data Lake Store account.",
                        DataLakeStoreHelper.ListAccounts(_dataLakeStoreClient),
                        true);
            }
            else
            {
                _dataLakeStoreAccountName = ConsolePrompts.Prompt("Enter your Data Lake Store account name.", true);
                subId = AzureHelper.GetSubscriptions(_credentials, _dataLakeStoreAccountName).Keys.FirstOrDefault();
            }
            
            _dataLakeStoreResourceGroupName = DataLakeStoreHelper.GetResourceGroupName(_dataLakeStoreClient, _dataLakeStoreAccountName);
        }

        private enum TopMenuOptionsEnum
        {
            BrowseData=0,
            UploadFile,
            DownloadFile,
            Done
        }

        private static void MainMenu()
        {
            var topMenuOptions = new[]
            {
                "Data Lake Store - Browse my data",
                "Data Lake Store - Upload a file",
                "Data Lake Store - Download a file",
                "I'm done!"
            }.ToList();
            ushort topMenuChoice;
            do
            {
                topMenuChoice = Convert.ToUInt16(ConsolePrompts.MenuPrompt(
                    "Main Menu"
                    + "\r\n---------"
                    + "\r\nSelect an action.",
                    topMenuOptions,
                    true,
                    true));
                switch (topMenuChoice)
                {
                    case (int)TopMenuOptionsEnum.BrowseData:
                        BrowseDataMenu();
                        break;
                    case (int)TopMenuOptionsEnum.UploadFile:
                        UploadFileMenu();
                        break;
                    case (int)TopMenuOptionsEnum.DownloadFile:
                        DownloadFileMenu();
                        break;
                }
            }
            while (topMenuChoice != (int)TopMenuOptionsEnum.Done);
        }

        private static void BrowseDataMenu()
        {
            var breadcrumbs = new Stack<string>();
            breadcrumbs.Push("/");
            var done = false;
            do {
                var currentPath = String.Join("", Enumerable.Reverse(breadcrumbs.ToList()));
                var fileList = DataLakeStoreHelper.ListItems(_dataLakeStoreFileSystemClient, _dataLakeStoreAccountName, currentPath);
                var fileMenuItems = fileList.Select(a => String.Format("{0,15} {1}", a.Type, a.PathSuffix))
                    .Concat(new[]{"Navigate up", "Refresh list", "Return to main menu"})
                    .ToList();
                var input = ConsolePrompts.MenuPrompt(String.Format("Current path: {0}"
                    + "\r\nChoose an action or directory.", currentPath), fileMenuItems, true, true);
                var inputInt = Convert.ToInt32(input);

                if (inputInt >= 0 && inputInt < (fileMenuItems.Count() - 3)){
                    if (fileList[inputInt].Type == FileType.Directory)
                        breadcrumbs.Push(fileList[inputInt].PathSuffix + "/");
                }
                else if (inputInt == (fileMenuItems.Count() - 3)){
                    breadcrumbs.Pop();
                        Console.WriteLine("Moving up.");
                }
                else if (inputInt == (fileMenuItems.Count() - 1)){
                    done = true;
                }
            } while (!done);
        }

        private static void UploadFileMenu()
        {
            var localPath = ConsolePrompts.Prompt("Enter the local path of a file you wish to upload. e.g. C:\\folder\\test.tsv");
            var remotePath = ConsolePrompts.Prompt(String.Format("Enter the path where you'd like to place the file in Data Lake Store '{0}'."
                + "\r\ne.g. /thisFolder/foo.txt",_dataLakeStoreAccountName));
            bool force = ConsolePrompts.MenuPrompt("Force overwrite?", new[] { "No", "Yes" }).Equals("1", StringComparison.InvariantCultureIgnoreCase);
            DataLakeStoreHelper.UploadFile(_dataLakeStoreFileSystemClient, _dataLakeStoreAccountName, localPath, remotePath, force);
        }

        private static void DownloadFileMenu()
        {
            var remotePath = ConsolePrompts.Prompt(String.Format("Enter the path of the file you want to download from Data Lake Store '{0}'."
                + "\r\ne.g. /thisFolder/foo.txt", _dataLakeStoreAccountName));
            var localPath = ConsolePrompts.Prompt("Enter the local path where you want the file saved. e.g. C:\\folder\\test.tsv");
            bool force = ConsolePrompts.MenuPrompt("Force overwrite?", new[] { "No", "Yes" }).Equals("1", StringComparison.InvariantCultureIgnoreCase);
            DataLakeStoreHelper.DownloadFile(_dataLakeStoreFileSystemClient, _dataLakeStoreAccountName, remotePath, localPath, force);
        }

        private static void Done()
        {
            Console.WriteLine("Done. Press any key to exit.");
            Console.ReadKey(true);
        }
    }
}