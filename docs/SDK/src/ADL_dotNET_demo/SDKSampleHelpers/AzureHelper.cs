using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security;

using Microsoft.Azure;
using Microsoft.Azure.Common.Authentication;
using Microsoft.Azure.Common.Authentication.Factories;
using Microsoft.Azure.Common.Authentication.Models;
using Microsoft.Azure.Management.DataLake;
using Microsoft.Azure.Subscriptions;

namespace SDKSampleHelpers
{
    public static class AzureHelper
    {
        public static SubscriptionCloudCredentials GetAccessToken(string username = null, SecureString password = null)
        {
            var authFactory = new AuthenticationFactory();

            var account = new AzureAccount { Type = AzureAccount.AccountType.User };

            if (username != null && password != null)
                account.Id = username;

            var env = AzureEnvironment.PublicEnvironments[EnvironmentName.AzureCloud];
            return new TokenCloudCredentials(authFactory.Authenticate(account, env, AuthenticationFactory.CommonAdTenant, password, ShowDialog.Auto).AccessToken);
        }

        public static Dictionary<string, string> GetSubscriptions(SubscriptionCloudCredentials creds)
        {
            var subClient = new SubscriptionClient(creds);

            var response = subClient.Subscriptions.List();

            Dictionary<string, string> dict = new Dictionary<string, string>();
            foreach (var sub in response.Subscriptions)
                dict[sub.SubscriptionId] = sub.DisplayName;

            return dict;
        }

        public static Dictionary<string, string> GetSubscriptions(SubscriptionCloudCredentials creds, string accountName)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();

            foreach (var sub in GetSubscriptions(creds))
            {
                List<string> accts;
                DataLakeManagementClient dlClient = new DataLakeManagementClient(GetCloudCredentials(creds, new Guid(sub.Key)));
                accts = DataLakeHelper.ListAccounts(dlClient).Keys.ToList();

                if (accts.Contains(accountName, StringComparer.InvariantCultureIgnoreCase))
                    dict[sub.Key] = sub.Value;
            }
            return dict;
        }
        public static SubscriptionCloudCredentials GetCloudCredentials(SubscriptionCloudCredentials creds, Guid subId)
        {
            return new TokenCloudCredentials(subId.ToString(), ((TokenCloudCredentials)creds).Token);
        }
    }
}
