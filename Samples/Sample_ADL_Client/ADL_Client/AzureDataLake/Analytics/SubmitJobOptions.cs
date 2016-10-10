using ADL = Microsoft.Azure.Management.DataLake;

namespace AzureDataLake.Analytics
{
    public class JobListPage
    {
        public ADL.Analytics.Models.JobInformation[] Jobs;
    }

    public class SubmitJobOptions
    {
        public System.Guid JobID;
        public string JobName;
        public string ScriptText;
    }

    public enum JobOrderByField
    {
        None,
        SubmitTime,
        Submitter,
        DegreeOfParallelism,
        EndTime,
        Name,
        Priority,
        Result
    }

    public enum JobOrderByDirection
    {
        Ascending,
        Descending
    }

}