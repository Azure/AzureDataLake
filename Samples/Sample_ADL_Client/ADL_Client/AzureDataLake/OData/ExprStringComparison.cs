namespace AzureDataLake.ODataQuery
{
    public class ExprStringComparison : Expr
    {
        public string Column;
        public string Value;
        public StringCompareOps Op;
        public ExprStringComparison(string col, string val, StringCompareOps op)
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
                sb.Append(string.Format("{0} {1} '{2}'", this.Column, op, this.Value));
            }
            else if (this.Op == StringCompareOps.Contains)
            {
                sb.Append(string.Format("substringof('{0}', {1})", this.Value, this.Column));
            }
            else if (this.Op == StringCompareOps.StartsWith)
            {
                sb.Append(string.Format("startswith({0}, '{1}')", this.Column, this.Value));
            }
            else if (this.Op == StringCompareOps.EndsWith)
            {
                sb.Append(string.Format("endswith({0}, '{1}')", this.Column, this.Value));
            }
            else
            {
                throw new System.ArgumentOutOfRangeException();
            }
        }
    }
}