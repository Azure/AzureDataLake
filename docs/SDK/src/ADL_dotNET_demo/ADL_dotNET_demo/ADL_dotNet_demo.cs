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
using SDKSampleHelpers;

namespace DataLakeConsoleApp
{
    class DataLakeConsoleApp
    {
        private static ProfileClient _profileClient;
        private static SubscriptionCloudCredentials _credentials;

        private static DataLakeManagementClient _dataLakeClient;
        private static DataLakeFileSystemManagementClient _dataLakeFileSystemClient;

        private static string _dataLakeAccountName;
        private static string _dataLakeResourceGroupName;

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
            System.Security.SecureString password = null; 
            if (username != null)
                password = ConsolePrompts.SecurePrompt("Enter your password.", true);
            else
            Console.WriteLine("Showing an OAuth popup.\r\n");

            Console.WriteLine("Logging you in... please wait.");
            _profileClient = AzureHelper.GetProfile(username, password);
        }

        private static void GetAccounts()
        {
           var subId =
                ConsolePrompts.MenuPrompt("Select a subscription.\r\nTo create a new subscription, visit:"
                                          + "\r\n   https://account.windowsazure.com/Subscriptions"
                                          + "\r\nIf you're not sure which subscription to pick, leave this blank.",
                    AzureHelper.GetSubscriptions(_profileClient));

           if (string.IsNullOrWhiteSpace(subId))
           {
               _dataLakeAccountName = ConsolePrompts.Prompt("Select a subscription by providing a Data Lake account name.", true);
               subId = AzureHelper.GetSubscriptions(_profileClient, _dataLakeAccountName).Keys.FirstOrDefault();
           }

            _credentials = AzureHelper.GetCloudCredentials(_profileClient, new Guid(subId));
            _dataLakeClient = new DataLakeManagementClient(_credentials);
            _dataLakeFileSystemClient = new DataLakeFileSystemManagementClient(_credentials);

            if (string.IsNullOrWhiteSpace(_dataLakeAccountName))
            {
               _dataLakeAccountName =
                   ConsolePrompts.MenuPrompt("Select your Data Lake account.",
                       DataLakeHelper.ListAccounts(_dataLakeClient),
                       true);
            }

            _dataLakeResourceGroupName = DataLakeHelper.GetResourceGroupName(_dataLakeClient, _dataLakeAccountName);
        }

        private enum TopMenuOptionsEnum
        {
            BrowseData,
            UploadFile,
            DownloadFile,
            Done
        }

        private static void MainMenu()
        {
            var topMenuOptions = new[]
            {
                "Data Lake - Browse my data",
                "Data Lake - Upload a file",
                "Data Lake - Download a file",
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
            do
            {
                var currentPath = String.Join("", Enumerable.Reverse(breadcrumbs.ToList()));
                var fileList = DataLakeHelper.ListItems(_dataLakeFileSystemClient, _dataLakeAccountName, currentPath);
                var fileMenuItems = fileList.Select(a => String.Format("{0,15} {1}", a.Type, a.PathSuffix))
                    .Concat(new[] { "Navigate up", "Refresh list", "Return to main menu" })
                    .ToList();
                var input = ConsolePrompts.MenuPrompt(String.Format("Current path: {0}"
                    + "\r\nChoose an action or directory.", currentPath), fileMenuItems, true, true);
                var inputInt = Convert.ToInt32(input);

                if (inputInt >= 0 && inputInt < (fileMenuItems.Count() - 3))
                {
                    if (fileList[inputInt].Type == FileType.Directory)
                        breadcrumbs.Push(fileList[inputInt].PathSuffix + "/");
                }
                else if (inputInt == (fileMenuItems.Count() - 3))
                {
                    breadcrumbs.Pop();
                    Console.WriteLine("Moving up.");
                }
                else if (inputInt == (fileMenuItems.Count() - 1))
                {
                    done = true;
                }
            } while (!done);
        }

        private static void UploadFileMenu()
        {
            var localPath = ConsolePrompts.Prompt("Enter the local path of a file you wish to upload. e.g. C:\\folder\\test.tsv");
            var remotePath = ConsolePrompts.Prompt(String.Format("Enter the path where you'd like to place the file in Data Lake '{0}'."
                + "\r\ne.g. /thisFolder/foo.txt", _dataLakeAccountName));
            bool force = ConsolePrompts.MenuPrompt("Force overwrite?", new[] { "No", "Yes" }).Equals("1", StringComparison.InvariantCultureIgnoreCase);
            DataLakeHelper.UploadFile(_dataLakeFileSystemClient, _dataLakeAccountName, localPath, remotePath, force);
        }

        private static void DownloadFileMenu()
        {
            var remotePath = ConsolePrompts.Prompt(String.Format("Enter the path of the file you want to download from Data Lake '{0}'."
                + "\r\ne.g. /thisFolder/foo.txt", _dataLakeAccountName));
            var localPath = ConsolePrompts.Prompt("Enter the local path where you want the file saved. e.g. C:\\folder\\test.tsv");
            bool force = ConsolePrompts.MenuPrompt("Force overwrite?", new[] { "No", "Yes" }).Equals("1", StringComparison.InvariantCultureIgnoreCase);
            DataLakeHelper.DownloadFile(_dataLakeFileSystemClient, _dataLakeAccountName, remotePath, localPath, force);
        }

        private static void Done()
        {
            Console.WriteLine("Done. Press any key to exit.");
            Console.ReadKey(true);
        }
    }
}