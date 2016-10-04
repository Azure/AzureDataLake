namespace AzureDataLake.Analytics
{
    public class GetJobListOptions
    {
        public int Top = 1000;
        public JobOrderByField OrderByField;
        public JobOrderByDirection OrderByDirection;
    }
}