namespace AzureDataLake.ODataQuery
{
    public class FilterUtils
    {
        public static string OpToString(NumericComparisonOperator operation)
        {
            if (operation == NumericComparisonOperator.GreaterThan)
            {
                return "gt";
            }
            else if (operation == NumericComparisonOperator.GreaterThanOrEquals)
            {
                return "ge";
            }
            else if (operation == NumericComparisonOperator.LesserThan)
            {
                return "lt";
            }
            else if (operation == NumericComparisonOperator.LesserThanOrEquals)
            {
                return "le";
            }
            else if (operation == NumericComparisonOperator.Equals)
            {
                return "eq";
            }
            else if (operation == NumericComparisonOperator.NotEquals)
            {
                return "ne";
            }
            else
            {
                throw new System.ArgumentOutOfRangeException();
            }
        }

    }
}