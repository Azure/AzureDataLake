using System.Collections.Generic;
using System.Linq;
using Microsoft.Azure.Management.DataLake.Analytics;
using Microsoft.Azure.Management.DataLake.Analytics.Models;
using ADL=Microsoft.Azure.Management.DataLake;

namespace AzureDataLake.Analytics
{
    public class AnalyticsJobClient : AccountClientBase
    {
        public static int ADLJobPageSize = 300;

        private ADL.Analytics.DataLakeAnalyticsJobManagementClient _adla_job_rest_client;

        public AnalyticsJobClient(string account, AzureDataLake.Authentication.AuthenticatedSession authSession) :
            base(account, authSession)
        {
            this._adla_job_rest_client = new ADL.Analytics.DataLakeAnalyticsJobManagementClient(this.AuthenticatedSession.Credentials);
        }

        public ADL.Analytics.Models.JobInformation GetJob(System.Guid jobid)
        {
            var job = this._adla_job_rest_client.Job.Get(this.Account, jobid);

            return job;
        }

        public List<ADL.Analytics.Models.JobInformation> GetJobList(GetJobListOptions options)
        {
            var results = new List<ADL.Analytics.Models.JobInformation>();
            foreach (var page in this.GetJobListPaged(options))
            {
                results.AddRange(page);
            }
            return results;
        }

        public IEnumerable<ADL.Analytics.Models.JobInformation[]> GetJobListPaged(GetJobListOptions options)
        {

            // Construct OData query from options
            var odata_query = new Microsoft.Rest.Azure.OData.ODataQuery<JobInformation>();
            if (options.Top > 0)
            {
                if (options.Top > AnalyticsJobClient.ADLJobPageSize)
                {
                    
                }
                else
                {
                    odata_query.Top = options.Top;
                }
            }
            odata_query.OrderBy = options.CreateOrderByString();
            odata_query.Filter = options.CreateFilterString();

            // Other parameters
            string list_select = null;
            bool? list_count = null;
            string list_search = null;
            string list_format = null;

            // Handle the initial response

            int item_count = 0;
            var page = this._adla_job_rest_client.Job.List(this.Account, odata_query, list_select, list_count, list_search, list_format);
            foreach (var cur_page in RESTUtil.EnumPages<JobInformation>(page, p => this._adla_job_rest_client.Job.ListNext(p.NextPageLink)))
            {
                yield return cur_page;

                item_count += cur_page.Length;
                if (item_count >= options.Top)
                {
                    break;
                }
            }
        }

        public ADL.Analytics.Models.JobInformation  SubmitJob(SubmitJobOptions options)
        {
            if (options.JobID == default(System.Guid))
            {
                options.JobID = System.Guid.NewGuid();
            }

            if (options.JobName == null)
            {
                options.JobName = "ADL_Demo_Client_Job_" + System.DateTime.Now.ToString();
            }

            var parameters = CreateNewJobProperties(options);
            var jobInfo = this._adla_job_rest_client.Job.Create(this.Account, options.JobID, parameters);

            return jobInfo;
        }

        private static JobInformation CreateNewJobProperties(SubmitJobOptions options)
        {
            var jobprops = new USqlJobProperties();
            jobprops.Script = options.ScriptText;

            var jobType = JobType.USql;
            int priority = 1;
            int dop = 1;

            var parameters = new JobInformation(
                name: options.JobName,
                type: jobType,
                properties: jobprops,
                priority: priority,
                degreeOfParallelism: dop,
                jobId: options.JobID);
            return parameters;
        }


        public ADL.Analytics.Models.JobStatistics GetStatistics(System.Guid jobid)
        {
            var stats = this._adla_job_rest_client.Job.GetStatistics(this.Account, jobid);
            return stats;
        }

        public ADL.Analytics.Models.JobDataPath GetDebugDataPath(System.Guid jobid)
        {
            var jobdatapath = this._adla_job_rest_client.Job.GetDebugDataPath(this.Account, jobid);
            return jobdatapath;
        }
    }
}