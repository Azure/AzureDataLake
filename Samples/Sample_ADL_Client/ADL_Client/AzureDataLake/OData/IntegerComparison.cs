namespace AzureDataLake.ODataQuery
{
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