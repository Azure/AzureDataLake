namespace AzureDataLake.ODataQuery
{
    public static class Extensions
    {
        public static void AppendExpr(this System.Text.StringBuilder sb, Expr expr)
        {
            expr.ToExprString(sb);
        }

        public static void AppendQuotedString(this System.Text.StringBuilder sb, string s)
        {
            sb.AppendFormat("'{0}'", s);
        }

    }

    public class ExprStringComparison : Expr
    {
        public ExprColumn Column;
        public string Value;
        public StringCompareOps Op;

        public ExprStringComparison(ExprColumn col, string val, StringCompareOps op)
        {
            this.Column = col;
            this.Value = val;
            this.Op = op;
        }

        public override void ToExprString(System.Text.StringBuilder sb)
        {
            if (this.Op == StringCompareOps.Equals)
            {
                string op = "eq";
                sb.AppendExpr(this.Column);
                sb.Append(" ");
                sb.Append(op);
                sb.Append(" ");
                sb.AppendQuotedString(this.Value);
            }
            else if (this.Op == StringCompareOps.Contains)
            {
                sb.Append("substringof(");
                sb.AppendQuotedString(this.Value);
                sb.Append(",");
                sb.AppendExpr(this.Column);
                sb.Append(")");
            }
            else if (this.Op == StringCompareOps.StartsWith)
            {

                sb.Append("startswith(");
                sb.AppendExpr(this.Column);
                sb.Append(",");
                sb.AppendQuotedString(this.Value);
                sb.Append(")");
            }
            else if (this.Op == StringCompareOps.EndsWith)
            {
                sb.Append("endswith(");
                sb.AppendExpr(this.Column);
                sb.Append(",");
                sb.AppendQuotedString(this.Value);
                sb.Append(")");
            }
            else
            {
                throw new System.ArgumentOutOfRangeException();
            }
        }
    }
}