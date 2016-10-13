namespace AzureDataLake.ODataQuery
{
    public class ExprCompareNumeric : ExprCompare
    {
        public ComparisonNumeric Operator;

        public ExprCompareNumeric(Expr left, Expr right, ComparisonNumeric op) :
            base(left,right)
        {
            this.Operator = op;
        }

        public override void ToExprString(ExBuilder sb)
        {
            var op = ExprCompareNumeric.OpToString(this.Operator);

            this.WriteBinaryOperation(sb,op,this.LeftValue,this.RightValue);
        }

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
    }
}