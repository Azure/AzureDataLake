namespace AzureDataLake.ODataQuery
{
    public class ExprDateTimeComparison : Expr
    {
        public ExprColumn Column;
        public System.DateTime Value;
        public NumericCompareOps Op;
        public ExprDateTimeComparison(ExprColumn col, System.DateTime val, NumericCompareOps op)
        {
            this.Column = col;
            this.Value = val;
        }

        public override void ToExprString(ExBuilder sb)
        {
            string datestring = this.Value.ToString("O");

            // due to issue: https://github.com/Azure/autorest/issues/975,
            // date time offsets must be explicitly escaped before being passed to the filter

            var escaped_datestring = System.Uri.EscapeDataString(datestring);

            var op = FilterUtils.OpToString(this.Op);
            sb.Append(this.Column);
            sb.Append(" ");
            sb.Append(op);
            sb.Append("datetimeoffset");
            sb.Append(escaped_datestring);
        }
    }

    public class IntegerComparison : Expr
    {
        public ExprColumn Column;
        public int Value;
        public NumericCompareOps Op;
        public IntegerComparison(ExprColumn col, int val, NumericCompareOps op)
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
            sb.Append(this.Value);
        }
    }

}