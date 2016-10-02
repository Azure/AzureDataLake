using System.Collections.Generic;
using System.Linq;
using ADL=Microsoft.Azure.Management.DataLake;
using Microsoft.Azure.Management.DataLake.Store;

namespace AzureDataLake.Store
{
    public class StoreManagementClient : ClientBase
    {
        private ADL.Store.DataLakeStoreAccountManagementClient _adls_mgmt_rest_client;
        private AzureDataLake.Subscription Sub;

        public StoreManagementClient(Subscription sub, AzureDataLake.Authentication.AuthenticatedSession authSession) :
            base(authSession)
        {
            this.Sub = sub;
            this._adls_mgmt_rest_client = new ADL.Store.DataLakeStoreAccountManagementClient(this.AuthenticatedSession.Credentials);
            this._adls_mgmt_rest_client.SubscriptionId = sub.ID;
        }

        public List<ADL.Store.Models.DataLakeStoreAccount> ListAccounts()
        {
            var page = this._adls_mgmt_rest_client.Account.List();
            var pages = AzureDataLake.RESTUtil.EnumPages(page,
                p => this._adls_mgmt_rest_client.Account.ListNext(p.NextPageLink));

            var result = new List<ADL.Store.Models.DataLakeStoreAccount>();
            foreach (var p in pages)
            {
                result.AddRange(p);
            }
            return result;
        }
    }
}