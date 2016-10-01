namespace AzureDataLake.Analytics
{
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
        Submitter
    }

    public enum JobOrderByDirection
    {
        Ascending,
        Descending
    }

}