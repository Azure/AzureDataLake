namespace AzureDataLake.ODataQuery
{
    public class ExprStringComparison : Expr
    {
        public Expr Column;
        public Expr Value;
        public StringCompareOperator Op;

        public ExprStringComparison(Expr col, Expr val, StringCompareOperator op)
        {
            this.Column = col;
            this.Value = val;
            this.Op = op;
        }

        public override void ToExprString(ExBuilder sb)
        {
            if (this.Op == StringCompareOperator.Equals)
            {
                string op = "eq";
                sb.Append(this.Column);
                sb.Append(" ");
                sb.Append(op);
                sb.Append(" ");
                sb.Append(this.Value);
            }
            else if (this.Op == StringCompareOperator.NotEquals)
            {
                string op = "ne";
                sb.Append(this.Column);
                sb.Append(" ");
                sb.Append(op);
                sb.Append(" ");
                sb.Append(this.Value);
            }
            else if (this.Op == StringCompareOperator.Contains)
            {
                sb.Append("substringof(");
                sb.Append(this.Value);
                sb.Append(",");
                sb.Append(this.Column);
                sb.Append(")");
            }
            else if (this.Op == StringCompareOperator.StartsWith)
            {

                sb.Append("startswith(");
                sb.Append(this.Column);
                sb.Append(",");
                sb.Append(this.Value);
                sb.Append(")");
            }
            else if (this.Op == StringCompareOperator.EndsWith)
            {
                sb.Append("endswith(");
                sb.Append(this.Column);
                sb.Append(",");
                sb.Append(this.Value);
                sb.Append(")");
            }
            else
            {
                throw new System.ArgumentOutOfRangeException();
            }
        }
    }
}