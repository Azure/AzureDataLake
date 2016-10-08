namespace AzureDataLake.Store
{

    public class FsUri
    {
        public readonly string Account;
        public readonly string Path;

        public FsUri(string uri):
            this( new System.Uri(uri))
        {                       
        }

        public FsUri(string account, string path)
        {
            if (account == null)
            {
                throw new System.ArgumentNullException(nameof(account));
            }

            if (account.Length==0)
            {
                throw new System.ArgumentOutOfRangeException(nameof(account));
            }

            this.Account = account.ToLower();


            if (path == null || path == "" || path == "/" || path == "\\" )
            {
                this.Path = "/";
            }
            else
            {
                if (path.StartsWith("/") || path.StartsWith("\\"))
                {
                    this.Path = path;
                }
                else
                {
                    this.Path = "/" + path;
                }
            }

        }
        public FsUri(System.Uri uri)
        {
            // adl://usqlteamdemosadls.azuredatalakestore.net/users/mwinkle
            // swebhdfs://usqlteamdemosadls.azuredatalakestore.net/users/mwinkle
            string scheme = uri.Scheme;
            scheme = scheme.ToLowerInvariant();

            // validate the URI scheme

            if (!(scheme == "adl" || scheme == "swebhdfs"))
            {
                throw new System.ArgumentException("Unsupported URI scheme");
            }

            // Figure out account name from URI host

            string lowerhost = uri.Host.ToLowerInvariant();
            var tokens = lowerhost.Split('.');
            if (tokens.Length == 1)
            {
                string account = tokens[0];
                this.Account = account;
            }
            else if (tokens.Length == 3)
            {
                if (tokens[1] != "azuredatalakestore")
                {
                    throw new System.ArgumentException("Invalid hostname");
                }

                if (tokens[2] != "net")
                {
                    throw new System.ArgumentException("Invalid hostname");
                }

                string account = tokens[0];
                this.Account = account;
            }
            else
            {
                throw new System.ArgumentException("Invalid hostname");
            }

            string localpath = uri.LocalPath;
            this.Path = localpath;
        }

    }
    public class FsPath
    {
        string s;

        public FsPath(string s)
        {
            if (s == null)
            {
                throw new System.ArgumentNullException(nameof(s));
            }

            if (s.Length == 0)
            {
                throw new System.ArgumentOutOfRangeException(nameof(s));

            }
            this.s=s;
        }

        public override string ToString()
        {
            return this.s;
        }

        public bool IsRooted
        {
            get { return this.s[0] == '/'; }
        }


        public static FsPath Root
        {
            get { return FsPath.root; }
        }

        private static readonly FsPath root = new FsPath("/");

        public static FsPath Combine(FsPath left, FsPath right)
        {
            if (right.IsRooted)
            {
                throw new System.ArgumentException(nameof(right));
            }


            if (left.EndsWithSeparator)
            {
                var p = new FsPath(left.ToString() + right.ToString());
                return p;

            }
            else
            {
                var p = new FsPath(left.ToString() + "/" + right.ToString());
                return p;
            }

        }

        public FsPath Append(FsPath p)
        {
            return FsPath.Combine(this, p);
        }

        public FsPath Append(string p)
        {
            return FsPath.Combine(this, new FsPath(p));
        }

        public bool EndsWithSeparator
        {
            get { return this.s[this.s.Length - 1] == '/'; }
        }

    }
}