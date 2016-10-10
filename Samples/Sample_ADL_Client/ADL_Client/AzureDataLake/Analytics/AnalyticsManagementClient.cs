using System.Collections.Generic;
using System.Linq;
using Microsoft.Azure.Management.DataLake.Analytics;
using ADL=Microsoft.Azure.Management.DataLake;

namespace AzureDataLake.Analytics
{
    public class AnalyticsManagementClient: ClientBase
    {
        private ADL.Analytics.DataLakeAnalyticsAccountManagementClient _adla_mgmt_rest_client;
        private AzureDataLake.Subscription Sub;

        public AnalyticsManagementClient(Subscription sub, AzureDataLake.Authentication.AuthenticatedSession authSession) :
            base(authSession)
        {
            this.Sub = sub;
            this._adla_mgmt_rest_client = new ADL.Analytics.DataLakeAnalyticsAccountManagementClient(this.AuthenticatedSession.Credentials);
            this._adla_mgmt_rest_client.SubscriptionId = sub.ID;
        }

        public List<ADL.Analytics.Models.DataLakeAnalyticsAccount> ListAccounts()
        {
            var initial_page = this._adla_mgmt_rest_client.Account.List();
            var pages = AzureDataLake.RESTUtil.EnumPages(initial_page,
                p => this._adla_mgmt_rest_client.Account.ListNext(p.NextPageLink));

            var result = new List<ADL.Analytics.Models.DataLakeAnalyticsAccount>();
            foreach (var cur_page in pages)
            {
                result.AddRange(cur_page);
            }
            return result;
        }

        public List<ADL.Analytics.Models.DataLakeAnalyticsAccount> ListAccountsByResourceGroup(string resource_group)
        {
            var initial_page = this._adla_mgmt_rest_client.Account.ListByResourceGroup(resource_group);
            var pages = AzureDataLake.RESTUtil.EnumPages(initial_page, p => this._adla_mgmt_rest_client.Account.ListByResourceGroupNext(p.NextPageLink));

            var result = new List<ADL.Analytics.Models.DataLakeAnalyticsAccount>();
            foreach (var cur_page in pages)
            {
                result.AddRange(cur_page);
            }
            return result;
        }
    }
}