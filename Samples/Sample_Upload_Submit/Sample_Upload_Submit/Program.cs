using ADL = Microsoft.Azure.Management.DataLake;
using RESTAUTH = Microsoft.Rest.Azure.Authentication;

namespace Sample_Upload_Submit
{
    class Program
    {
        static void Main(string[] args)
        {

        }

        static void Main_Upload_File()
        {
            var adls_acct = "mahiadlsdemo";
            var local_file = @"C:\mva\localinput.txt";
            var remote_file = "/mva2/input1.csv";
            var overwrite = true;

            var creds = get_creds();
            var fs_client = new ADL.Store.DataLakeStoreFileSystemManagementClient(creds);
            var upload_params = new ADL.StoreUploader.UploadParameters(local_file, remote_file, adls_acct, isOverwrite: overwrite);
            var fs_adapter = new ADL.StoreUploader.DataLakeStoreFrontEndAdapter(adls_acct, fs_client);
            var upload_client = new ADL.StoreUploader.DataLakeStoreUploader(upload_params, fs_adapter);
            upload_client.Execute();
        }

        static void Main_Submit_Jobs()
        {
            var adla_acct = "mahiadlademo";
            var usql_script = "your script here";
            var job_name = "Test job";
            var job_id = System.Guid.NewGuid();

            var creds = get_creds();
            var job_client = new ADL.Analytics.DataLakeAnalyticsJobManagementClient(creds);
            var job_props = new ADL.Analytics.Models.USqlJobProperties(usql_script);
            var job_info = new ADL.Analytics.Models.JobInformation(job_name, ADL.Analytics.Models.JobType.USql, job_props);
            ADL.Analytics.JobOperationsExtensions.Create(job_client.Job, adla_acct, job_id, job_info);
        }

        private static Microsoft.Rest.ServiceClientCredentials get_creds()
        {
            string nativeClientApp_clientId = "1950a258-227b-4e31-a9cf-717495945fc2"; // Reuse the client id for ADL PowerShell
            string domain = "common"; // don't restrict it to a specific domain
            var client_redirect_uri = new System.Uri("urn:ietf:wg:oauth:2.0:oob"); // choose the magic uri which "just works"

            var sync_context = new System.Threading.SynchronizationContext();
            System.Threading.SynchronizationContext.SetSynchronizationContext(sync_context);

            var ad_client_settings = RESTAUTH.ActiveDirectoryClientSettings.UsePromptOnly(nativeClientApp_clientId, client_redirect_uri);
            var creds = RESTAUTH.UserTokenProvider.LoginWithPromptAsync(domain, ad_client_settings).Result;
            return creds;
        }
    }
}
