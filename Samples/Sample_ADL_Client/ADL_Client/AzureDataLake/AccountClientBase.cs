namespace AzureDataLake
{

    public class ClientBase
    {
        public AzureDataLake.Authentication.AuthenticatedSession AuthenticatedSession;

        public ClientBase(AzureDataLake.Authentication.AuthenticatedSession auth_session)
        {
            this.AuthenticatedSession = auth_session;
        }
    }

    public class AccountClientBase : ClientBase
    {
        public string Account;

        public AccountClientBase(string account, AzureDataLake.Authentication.AuthenticatedSession auth_session) :
            base(auth_session)
        {
            this.Account = account;
        }
    }

    public class Subscription
    {
        public string ID;

        public Subscription(string id)
        {
            this.ID = id;
        }
    }
}