using System.Linq;
using AzureDataLake.Analytics;

namespace ADL_Client_Demo
{
    class Program
    {
        private static void Main(string[] args)
        {
            var auth_session = new AzureDataLake.Authentication.AuthenticatedSession("ADL_Demo_Client");
            auth_session.Authenticate();

            string store_account = "datainsightsadhoc";
            string analytics_account = "datainsightsadhoc";
            string subid = "045c28ea-c686-462f-9081-33c34e871ba3";
            var sub = new AzureDataLake.Subscription(subid);

            var StoreClient = new AzureDataLake.Store.StoreFileSystemClient(store_account, auth_session);
            var AnalyticsClient = new AzureDataLake.Analytics.AnalyticsJobClient(analytics_account, auth_session);
            var StoreAccountClient = new AzureDataLake.Store.StoreManagementClient(sub, auth_session);
            var AnalyticsAccountClient = new AzureDataLake.Analytics.AnalyticsManagementClient(sub, auth_session);

        }

    }
}
