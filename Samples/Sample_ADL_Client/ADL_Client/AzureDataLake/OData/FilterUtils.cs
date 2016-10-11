namespace AzureDataLake.ODataQuery
{
    public class FilterUtils
    {
        public static string OpToString(CompareOps Op)
        {
            string op = "ge";
            if (Op == CompareOps.GreaterThan)
            {
                op = "gt";
            }
            else if (Op == CompareOps.GreaterThanOrEquals)
            {
                op = "ge";
            }
            else if (Op == CompareOps.LesserThan)
            {
                op = "lt";
            }
            else if (Op == CompareOps.LesserThanOrEquals)
            {
                op = "le";
            }
            else if (Op == CompareOps.Equals)
            {
                op = "eq";
            }
            else
            {
                throw new System.ArgumentOutOfRangeException();
            }
            return op;
        }

    }
}