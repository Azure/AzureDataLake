using System.Collections.Generic;
using Microsoft.Azure.Management.DataLake.Analytics;
using Microsoft.Azure.Management.DataLake.Analytics.Models;
using Microsoft.Rest;
using ADL=Microsoft.Azure.Management.DataLake;

namespace AzureDataLake
{
    public class RMClient : AccountClientBase
    {
        private Microsoft.Azure.Management.Resources.ResourceManagementClient _azurerm_rest_client;

        public RMClient(string account, AzureDataLake.Authentication.AuthenticatedSession authSession) :
            base(account, authSession)
        {
            //this._azurerm_rest_client = new Microsoft.Azure.Management.Resources.ResourceManagementClient( (TokenCredentials)this.AuthenticatedSession.Credentials);
        }

    }
}