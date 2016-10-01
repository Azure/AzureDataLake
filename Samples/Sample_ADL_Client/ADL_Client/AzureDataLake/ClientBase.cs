namespace AzureDataLake
{
    public class ClientBase
    {
        public string Account;
        public AzureDataLake.Authentication.AuthenticatedSession AuthenticatedSession;

        public ClientBase(string account, AzureDataLake.Authentication.AuthenticatedSession auth_session)
        {
            this.Account = account;
            this.AuthenticatedSession = auth_session;
        }
    }
}