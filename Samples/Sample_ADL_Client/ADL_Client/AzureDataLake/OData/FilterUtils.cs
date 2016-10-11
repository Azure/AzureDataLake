namespace AzureDataLake.ODataQuery
{
    public class FilterUtils
    {
        public static string OpToString(NumericCompareOps Op)
        {
            string op = "ge";
            if (Op == NumericCompareOps.GreaterThan)
            {
                op = "gt";
            }
            else if (Op == NumericCompareOps.GreaterThanOrEquals)
            {
                op = "ge";
            }
            else if (Op == NumericCompareOps.LesserThan)
            {
                op = "lt";
            }
            else if (Op == NumericCompareOps.LesserThanOrEquals)
            {
                op = "le";
            }
            else if (Op == NumericCompareOps.Equals)
            {
                op = "eq";
            }
            else if (Op == NumericCompareOps.NotEquals)
            {
                op = "ne";
            }
            else
            {
                throw new System.ArgumentOutOfRangeException();
            }
            return op;
        }

    }
}