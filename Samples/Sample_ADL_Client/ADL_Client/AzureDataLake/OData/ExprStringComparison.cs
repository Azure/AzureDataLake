namespace AzureDataLake.ODataQuery
{
    public class ExprStringComparison : Expr
    {
        public Expr LeftValue;
        public Expr RightValue;
        public StringCompareOperator Operator;

        public ExprStringComparison(Expr leftvalue, Expr rightvalue, StringCompareOperator op)
        {
            this.LeftValue = leftvalue;
            this.RightValue = rightvalue;
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