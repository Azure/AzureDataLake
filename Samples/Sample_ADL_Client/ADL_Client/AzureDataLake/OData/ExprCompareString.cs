namespace AzureDataLake.ODataQuery
{
    public class ExprCompareString : ExprCompare
    {
        public StringCompareOperator Operator;

        public ExprCompareString(Expr left, Expr right, StringCompareOperator op) :
            base(left, right)
        {
            this.Operator = op;
        }

        public override void ToExprString(ExBuilder sb)
        {
            if (this.Operator == StringCompareOperator.Equals)
            {
                string op = "eq";
                sb.Append(this.LeftValue);
                sb.Append(" ");
                sb.Append(op);
                sb.Append(" ");
                sb.Append(this.RightValue);
            }
            else if (this.Operator == StringCompareOperator.NotEquals)
            {
                string op = "ne";
                sb.Append(this.LeftValue);
                sb.Append(" ");
                sb.Append(op);
                sb.Append(" ");
                sb.Append(this.RightValue);
            }
            else if (this.Operator == StringCompareOperator.Contains)
            {
                sb.Append("substringof(");
                sb.Append(this.RightValue);
                sb.Append(",");
                sb.Append(this.LeftValue);
                sb.Append(")");
            }
            else if (this.Operator == StringCompareOperator.StartsWith)
            {

                sb.Append("startswith(");
                sb.Append(this.LeftValue);
                sb.Append(",");
                sb.Append(this.RightValue);
                sb.Append(")");
            }
            else if (this.Operator == StringCompareOperator.EndsWith)
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