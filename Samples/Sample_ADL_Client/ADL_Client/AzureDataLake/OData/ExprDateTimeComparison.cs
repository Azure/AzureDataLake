namespace AzureDataLake.ODataQuery
{
    public class ExprDateTimeComparison : Expr
    {
        public ExprColumn Column;
        public ExprDateLiteral Value;
        public NumericComparisonOperator Operator;
        public ExprDateTimeComparison(ExprColumn col, ExprDateLiteral val, NumericComparisonOperator op)
        {
            this.Column = col;
            this.Value = val;
            this.Operator = op;
        }

        public override void ToExprString(ExBuilder sb)
        {

            var op = FilterUtils.OpToString(this.Operator);
            sb.Append(this.Column);
            sb.Append(" ");
            sb.Append(op);
            sb.Append(" ");
            sb.Append(this.Value);
        }
    }
}