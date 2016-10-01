
using System;
using REST = Microsoft.Rest.Azure;

namespace AzureDataLake.Authentication
{
    public class AuthenticatedSession
    {
        public Microsoft.Rest.ServiceClientCredentials Credentials;

        public void Authenticate()
        {
            var domain = "common"; // Replace this string with the user's Azure Active Directory tenant ID or domain name, if needed.
            var nativeClientApp_clientId = "1950a258-227b-4e31-a9cf-717495945fc2";
            var clientRedirectUri = new System.Uri("urn:ietf:wg:oauth:2.0:oob");
            var activeDirectoryClientSettings = REST.Authentication.ActiveDirectoryClientSettings.UsePromptOnly(nativeClientApp_clientId, clientRedirectUri);

            // Load the token cache, if one exists.
            string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string basefname = "TokenCache.tc";
            var tokenCachePath = System.IO.Path.Combine(path, basefname);

            Microsoft.IdentityModel.Clients.ActiveDirectory.TokenCache token_cache;

            if (System.IO.File.Exists(tokenCachePath))
            {
                var bytes = System.IO.File.ReadAllBytes(tokenCachePath);
                token_cache = new Microsoft.IdentityModel.Clients.ActiveDirectory.TokenCache(bytes);
            }
            else
            {
                token_cache = new Microsoft.IdentityModel.Clients.ActiveDirectory.TokenCache();
            }

            // Now figure out the token business

            Microsoft.Rest.ServiceClientCredentials creds = null;

            // Get the cached token, if it exists and is not expired.
            foreach (var item in token_cache.ReadItems())
            {
                if (item.ExpiresOn > System.DateTime.Now)
                {
                    // Reuse token
                    creds = REST.Authentication.UserTokenProvider.CreateCredentialsFromCache(nativeClientApp_clientId, item.TenantId, item.DisplayableId, token_cache).Result;
                    break;
                }
            }

            if (creds == null)
            {
                // Did not find the token in the cache, show popup and save the token
                var sync_context = new System.Threading.SynchronizationContext();
                System.Threading.SynchronizationContext.SetSynchronizationContext(sync_context);
                creds = REST.Authentication.UserTokenProvider.LoginWithPromptAsync(domain, activeDirectoryClientSettings, token_cache).Result;
                System.IO.File.WriteAllBytes(tokenCachePath, token_cache.Serialize());
            }

            this.Credentials = creds;

        }
    }

}