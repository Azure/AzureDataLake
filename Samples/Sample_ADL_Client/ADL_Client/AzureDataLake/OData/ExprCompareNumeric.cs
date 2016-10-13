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
            var op = FilterUtils.OpToString(this.Operator);

            sb.Append(this.LeftValue);
            sb.Append(" ");
            sb.Append(op);
            sb.Append(" ");
            sb.Append(this.RightValue);
        }
    }
}