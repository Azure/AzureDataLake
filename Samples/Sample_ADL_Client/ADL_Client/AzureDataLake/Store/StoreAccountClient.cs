using System.Collections.Generic;
using System.Linq;
using ADL=Microsoft.Azure.Management.DataLake;
using Microsoft.Azure.Management.DataLake.Store;

namespace AzureDataLake.Store
{
    public class StoreAccountClient : ClientBase
    {
        private ADL.Store.DataLakeStoreAccountManagementClient store_mgmt_client;
        private AzureDataLake.Subscription Sub;

        public StoreAccountClient(Subscription sub, AzureDataLake.Authentication.AuthenticatedSession authSession) :
            base(authSession)
        {
            this.Sub = sub;
            this.store_mgmt_client = new ADL.Store.DataLakeStoreAccountManagementClient(this.AuthenticatedSession.Credentials);
            this.store_mgmt_client.SubscriptionId = sub.ID;
        }

        public List<ADL.Store.Models.DataLakeStoreAccount> ListStores(string subscription_id)
        {
            var page = this.store_mgmt_client.Account.List();
            var stores = page.ToList();
            return stores;
        }
    }
}