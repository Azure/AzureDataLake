using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security;

using Microsoft.Azure;
using Microsoft.Azure.Common.Authentication;
using Microsoft.Azure.Common.Authentication.Models;
using Microsoft.Azure.Management.DataLake;

namespace SDKSampleHelpers
{
    public static class AzureHelper
    {
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

        public static Dictionary<string, string> GetSubscriptions(ProfileClient pClient)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            foreach (var sub in pClient.Profile.Subscriptions.Values)
                dict[sub.Id.ToString()] = sub.Name;
            return dict;
        }

        public static Dictionary<string, string> GetSubscriptions(ProfileClient pClient, string accountName)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();

            foreach (var sub in GetSubscriptions(pClient))
            {
                List<string> accts;
                     DataLakeManagementClient dlClient = new DataLakeManagementClient(GetCloudCredentials(pClient, new Guid(sub.Key)));
                    accts = DataLakeHelper.ListAccounts(dlClient).Keys.ToList();

                if (accts.Contains(accountName, StringComparer.InvariantCultureIgnoreCase))
                    dict[sub.Key] = sub.Value;
            }
            return dict;
        }
        public static SubscriptionCloudCredentials GetCloudCredentials(ProfileClient pClient, Guid subId)
        {
            var sub = pClient.Profile.Subscriptions.Values.FirstOrDefault(s => s.Id.Equals(subId));

            Debug.Assert(sub != null, "subscription != null");
            pClient.SetSubscriptionAsDefault(sub.Id, sub.Account);

            var credentials = AzureSession.AuthenticationFactory.GetSubscriptionCloudCredentials(pClient.Profile.Context);

            return credentials;
        }
    }
}
