namespace AzureDataLake.ODataQuery
{
    public class ExprStringComparison : Expr
    {
        public ExprColumn Column;
        public ExprStringLiteral Value;
        public StringCompareOps Op;

        public ExprStringComparison(ExprColumn col, ExprStringLiteral val, StringCompareOps op)
        {
            this.Column = col;
            this.Value = val;
            this.Op = op;
        }

        public override void ToExprString(ExBuilder sb)
        {
            if (this.Op == StringCompareOps.Equals)
            {
                string op = "eq";
                sb.Append(this.Column);
                sb.Append(" ");
                sb.Append(op);
                sb.Append(" ");
                sb.Append(this.Value);
            }
            else if (this.Op == StringCompareOps.Contains)
            {
                sb.Append("substringof(");
                sb.Append(this.Value);
                sb.Append(",");
                sb.Append(this.Column);
                sb.Append(")");
            }
            else if (this.Op == StringCompareOps.StartsWith)
            {

                sb.Append("startswith(");
                sb.Append(this.Column);
                sb.Append(",");
                sb.Append(this.Value);
                sb.Append(")");
            }
            else if (this.Op == StringCompareOps.EndsWith)
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