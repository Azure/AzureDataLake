using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ADL_Client_Tests
{
    public class Base_Tests
    {
        private bool init;

        public AzureDataLake.Authentication.AuthenticatedSession auth_session;
        public AzureDataLake.Analytics.AnalyticsJobClient adla_job_client;
        public AzureDataLake.Analytics.AnalyticsManagementClient adla_mgmt_client;
        public AzureDataLake.Store.StoreFileSystemClient adls_fs_client;
        public AzureDataLake.Store.StoreManagementClient adls_mgmt_client;
        public AzureDataLake.Subscription sub;


        public void Initialize()
        {
            if (this.init == false)
            {
                this.auth_session = new AzureDataLake.Authentication.AuthenticatedSession("ADL_Demo_Client");
                auth_session.Authenticate();

                string store_account = "datainsightsadhoc";
                string analytics_account = "datainsightsadhoc";
                string subid = "045c28ea-c686-462f-9081-33c34e871ba3";
                this.sub = new AzureDataLake.Subscription(subid);
                this.init = true;

                this.adls_fs_client = new AzureDataLake.Store.StoreFileSystemClient(store_account, auth_session);
                this.adla_job_client = new AzureDataLake.Analytics.AnalyticsJobClient(analytics_account, auth_session);
                this.adls_mgmt_client = new AzureDataLake.Store.StoreManagementClient(sub, auth_session);
                this.adla_mgmt_client = new AzureDataLake.Analytics.AnalyticsManagementClient(sub, auth_session);

            }
        }

    }
}
