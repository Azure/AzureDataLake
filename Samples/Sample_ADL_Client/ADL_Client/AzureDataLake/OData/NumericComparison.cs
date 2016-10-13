namespace AzureDataLake.ODataQuery
{
    public class NumericComparison : Expr
    {
        public Expr LeftValue;
        public Expr RightValue;
        public NumericComparisonOperator Operator;

        public NumericComparison(Expr left, Expr right, NumericComparisonOperator op)
        {
            this.LeftValue = left;
            this.RightValue = right;
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