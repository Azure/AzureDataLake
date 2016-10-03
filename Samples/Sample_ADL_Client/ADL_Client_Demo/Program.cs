using System.Linq;
using AzureDataLake.Analytics;

namespace ADL_Client_Demo
{
    class Program
    {
        private static void Main(string[] args)
        {
            var auth_session = new AzureDataLake.Authentication.AuthenticatedSession("ADL_Demo_Client");
            auth_session.Clear();
            auth_session.Authenticate();
        }

    }
}
