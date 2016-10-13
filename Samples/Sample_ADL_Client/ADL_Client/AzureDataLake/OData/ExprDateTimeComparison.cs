namespace AzureDataLake.ODataQuery
{
    public class ExprDateTimeComparison : Expr
    {
        public ExprColumn Column;
        public ExprDateLiteral Value;
        public NumericCompareOps Op;
        public ExprDateTimeComparison(ExprColumn col, ExprDateLiteral val, NumericCompareOps op)
        {
            this.Column = col;
            this.Value = val;
        }

        public override void ToExprString(ExBuilder sb)
        {

            var op = FilterUtils.OpToString(this.Op);
            sb.Append(this.Column);
            sb.Append(" ");
            sb.Append(op);
            sb.Append(" ");
            sb.Append(this.Value);
        }
    }

    public class IntegerComparison : Expr
    {
        public ExprColumn Column;
        public ExprIntLiteral Value;
        public NumericCompareOps Op;
        public IntegerComparison(ExprColumn col, ExprIntLiteral val, NumericCompareOps op)
        {
            this.Column = col;
            this.Value = val;
        }

        public override void ToExprString(ExBuilder sb)
        {
            var op = FilterUtils.OpToString(this.Op);

            sb.Append(this.Column);
            sb.Append(" ");
            sb.Append(op);
            sb.Append(" ");
            sb.Append(this.Value);
        }
    }

}