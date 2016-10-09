namespace AzureDataLake
{
    public class ResourceManagementClient : AccountClientBase
    {
        private Microsoft.Azure.Management.Resources.ResourceManagementClient _azurerm_rest_client;

        public ResourceManagementClient(string account, AzureDataLake.Authentication.AuthenticatedSession authSession) :
            base(account, authSession)
        {
            //this._azurerm_rest_client = new Microsoft.Azure.Management.Resources.ResourceManagementClient( (TokenCredentials)this.AuthenticatedSession.Credentials);
        }

    }
}