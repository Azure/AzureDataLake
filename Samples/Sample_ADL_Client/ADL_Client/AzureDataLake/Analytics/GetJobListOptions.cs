namespace AzureDataLake.Analytics
{
    public class GetJobListOptions
    {
        public int Top = 300; // 300 is the ADLA limit
        public JobOrderByField OrderByField;
        public JobOrderByDirection OrderByDirection;
    }
}