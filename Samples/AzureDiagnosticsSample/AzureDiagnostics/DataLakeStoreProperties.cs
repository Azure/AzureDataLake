namespace AzureDiagnostics
{
    public class DataLakeStoreProperties
    {
        public string StreamName;

        public DataLakeStoreProperties (Newtonsoft.Json.Linq.JObject rec)
        {
            this.StreamName = rec["StreamName"].ToString();
        }
    }
}