using System.Collections.Generic;
using System.Linq;
using Microsoft.Azure.Management.DataLake.Analytics;
using ADL=Microsoft.Azure.Management.DataLake;
using Microsoft.Azure.Management.DataLake.Store;

namespace AzureDataLake.Analytics
{
    public class AnalyticsAccountClient: ClientBase
    {
        private ADL.Analytics.DataLakeAnalyticsAccountManagementClient analytics_mgmt_client;
        private AzureDataLake.Subscription Sub;

        public AnalyticsAccountClient(Subscription sub, AzureDataLake.Authentication.AuthenticatedSession authSession) :
            base(authSession)
        {
            this.Sub = sub;
            this.analytics_mgmt_client = new ADL.Analytics.DataLakeAnalyticsAccountManagementClient(this.AuthenticatedSession.Credentials);
            this.analytics_mgmt_client.SubscriptionId = sub.ID;
        }

        public List<ADL.Analytics.Models.DataLakeAnalyticsAccount> ListStores(string subscription_id)
        {
            var page = this.analytics_mgmt_client.Account.List();
            var accounts = page.ToList();
            return accounts;
        }
    }
}