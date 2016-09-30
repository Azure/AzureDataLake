using ADL = Microsoft.Azure.Management.DataLake;
using RESTAUTH = Microsoft.Rest.Azure.Authentication;

namespace Sample_Upload_Submit
{
    class Program
    {

        static void Main(string[] args)
        {
            // -------------------------- 
            var creds = get_creds();

            var adls_acct = "mahiadlsdemo";
            var fs_client = new ADL.Store.DataLakeStoreFileSystemManagementClient(creds);
            var upload_params = new ADL.StoreUploader.UploadParameters(@"C:\mva\localinput.txt",
                                                        "/mva2/input1.csv",
                                                        adls_acct,
                                                        isOverwrite: true);

            var fs_adapter = new ADL.StoreUploader.DataLakeStoreFrontEndAdapter(adls_acct, fs_client);
            var upload_client = new ADL.StoreUploader.DataLakeStoreUploader(upload_params, fs_adapter);
            upload_client.Execute();

            // ------------------------------
            var adla_acct = "mahiadlademo";
            //Microsoft.Azure.Management.DataLake.Analytics.
            var job_client = new ADL.Analytics.DataLakeAnalyticsJobManagementClient(creds);
            var job_props = new ADL.Analytics.Models.USqlJobProperties("your script here");
            var job_info = new ADL.Analytics.Models.JobInformation("Test job", ADL.Analytics.Models.JobType.USql, job_props);
            var job_id = System.Guid.NewGuid();
            ADL.Analytics.JobOperationsExtensions.Create(job_client.Job,adla_acct, job_id, job_info);
        }

        private static Microsoft.Rest.ServiceClientCredentials get_creds()
        {
            string nativeClientApp_clientId = "1950a258-227b-4e31-a9cf-717495945fc2";
            string domain = "common";
            var client_redirect_uri = new System.Uri("urn:ietf:wg:oauth:2.0:oob");

            var sync_context = new System.Threading.SynchronizationContext();
            System.Threading.SynchronizationContext.SetSynchronizationContext(sync_context);

            var ad_client_settings = RESTAUTH.ActiveDirectoryClientSettings.UsePromptOnly(nativeClientApp_clientId, client_redirect_uri);
            var creds = RESTAUTH.UserTokenProvider.LoginWithPromptAsync(domain, ad_client_settings).Result;
            return creds;
        }
    }
}
