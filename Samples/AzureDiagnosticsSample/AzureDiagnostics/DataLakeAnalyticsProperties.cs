namespace AzureDiagnostics
{
    public class DataLakeAnalyticsProperties
    {
        public string JobId;
        public string JobName;
        public string JobRuntimeName;
        public System.DateTimeOffset? StartTime;
        public System.DateTimeOffset? SubmitTime;
        public System.DateTimeOffset? EndTime;

        public DataLakeAnalyticsProperties(Newtonsoft.Json.Linq.JObject rec)
        {
            this.JobId = rec["JobId"].ToString();
            this.JobName = rec["JobName"].ToString();
            this.JobRuntimeName = rec["JobRuntimeName"].ToString();
            this.StartTime = rec.GetDateTimeOffsetNullable("StartTime");
            this.SubmitTime = rec.GetDateTimeOffsetNullable("SubmitTime");
            this.EndTime = rec.GetDateTimeOffsetNullable("EndTime");
        }
    }
}