namespace AzureDataLake.ODataQuery
{
    public class ExprCompareString : ExprCompare
    {
        public ComparisonString Operator;

        public ExprCompareString(Expr left, Expr right, ComparisonString op) :
            base(left, right)
        {
            this.Operator = op;
        }

        public override void ToExprString(ExBuilder sb)
        {
            if (this.Operator == ComparisonString.Equals)
            {
                string op = "eq";
                sb.Append(this.LeftValue);
                sb.Append(" ");
                sb.Append(op);
                sb.Append(" ");
                sb.Append(this.RightValue);
            }
            else if (this.Operator == ComparisonString.NotEquals)
            {
                string op = "ne";
                sb.Append(this.LeftValue);
                sb.Append(" ");
                sb.Append(op);
                sb.Append(" ");
                sb.Append(this.RightValue);
            }
            else if (this.Operator == ComparisonString.Contains)
            {
                sb.Append("substringof(");
                sb.Append(this.RightValue);
                sb.Append(",");
                sb.Append(this.LeftValue);
                sb.Append(")");
            }
            else if (this.Operator == ComparisonString.StartsWith)
            {

                sb.Append("startswith(");
                sb.Append(this.LeftValue);
                sb.Append(",");
                sb.Append(this.RightValue);
                sb.Append(")");
            }
            else if (this.Operator == ComparisonString.EndsWith)
            {
                sb.Append("endswith(");
                sb.Append(this.LeftValue);
                sb.Append(",");
                sb.Append(this.RightValue);
                sb.Append(")");
            }
            else
            {
                throw new System.ArgumentOutOfRangeException();
            }
        }
    }
}