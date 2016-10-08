using System;

namespace AzureDataLake.Store
{
    public class FsUri
    {
        public string Account { get; private set; }
        public string Path { get; private set; }

        public FsUri(string uri)
        {
            string s = uri.Replace("\\", "/");
            var u= new System.Uri(s);
            this.InitFromUri(u);
        }

        public FsUri(string account, string path)
        {
            InitFromAccountAndPath(account, path);
        }

        private void InitFromAccountAndPath(string account, string path)
        {
            if (account == null)
            {
                throw new System.ArgumentNullException(nameof(account));
            }

            if (account.Length == 0)
            {
                throw new System.ArgumentOutOfRangeException(nameof(account));
            }

            this.Account = account.ToLower();


            if (path != null && path.Length > 0)
            {
                if (path.Contains("\\"))
                {
                    path = path.Replace("\\", "/");
                }
            }


            if (path == null || path == "" || path == "/" || path == "\\")
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
            InitFromUri(uri);
        }

        private void InitFromUri(Uri uri)
        {
            string scheme = uri.Scheme;
            scheme = scheme.ToLowerInvariant();

            // validate the URI scheme

            if (!(scheme == "adl" || scheme == "swebhdfs" || scheme == "webhdfs"))
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

        public string ToUriString()
        {
            string s = string.Format("adl://{0}.azuredatalakestore.net{1}", this.Account, this.Path);
            return s;
        }

    }
}