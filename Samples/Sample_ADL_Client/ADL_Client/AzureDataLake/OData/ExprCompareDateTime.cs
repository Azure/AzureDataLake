namespace AzureDataLake.ODataQuery
{
    public class ExprCompareDateTime : ExprCompare
    {
        public NumericComparisonOperator Operator;

        public ExprCompareDateTime(Expr left, Expr right, NumericComparisonOperator op) :
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