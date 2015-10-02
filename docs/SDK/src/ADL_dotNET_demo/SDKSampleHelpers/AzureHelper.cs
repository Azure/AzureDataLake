using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security;

using Microsoft.Azure;
using Microsoft.Azure.Common.Authentication;
using Microsoft.Azure.Common.Authentication.Factories;
using Microsoft.Azure.Common.Authentication.Models;
using Microsoft.Azure.Management.DataLake.Store;
using Microsoft.Azure.Subscriptions;

namespace SDKSampleHelpers
{
    public static class AzureHelper
    {
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
                DataLakeStoreManagementClient dlClient = new DataLakeStoreManagementClient(GetCloudCredentials(creds, new Guid(sub.Key)));
                accts = DataLakeStoreHelper.ListAccounts(dlClient).Keys.ToList();
                
                if (accts.Contains(accountName, StringComparer.InvariantCultureIgnoreCase))
                    dict[sub.Key] = sub.Value;
            }
            return dict;
        }

    }
}
