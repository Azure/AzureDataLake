namespace AzureDataLake.ODataQuery
{
    public class FilterUtils
    {
        public static string OpToString(ComparisonNumeric operation)
        {
            if (operation == ComparisonNumeric.GreaterThan)
            {
                return "gt";
            }
            else if (operation == ComparisonNumeric.GreaterThanOrEquals)
            {
                return "ge";
            }
            else if (operation == ComparisonNumeric.LesserThan)
            {
                return "lt";
            }
            else if (operation == ComparisonNumeric.LesserThanOrEquals)
            {
                return "le";
            }
            else if (operation == ComparisonNumeric.Equals)
            {
                return "eq";
            }
            else if (operation == ComparisonNumeric.NotEquals)
            {
                return "ne";
            }
            else
            {
                throw new System.ArgumentOutOfRangeException();
            }
        }

        public static string OpToString(ComparisonDateTime operation)
        {
            if (operation == ComparisonDateTime.GreaterThan)
            {
                return "gt";
            }
            else if (operation == ComparisonDateTime.GreaterThanOrEquals)
            {
                return "ge";
            }
            else if (operation == ComparisonDateTime.LesserThan)
            {
                return "lt";
            }
            else if (operation == ComparisonDateTime.LesserThanOrEquals)
            {
                return "le";
            }
            else if (operation == ComparisonDateTime.Equals)
            {
                return "eq";
            }
            else if (operation == ComparisonDateTime.NotEquals)
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