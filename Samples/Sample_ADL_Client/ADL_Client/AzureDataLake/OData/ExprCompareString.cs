namespace AzureDataLake.ODataQuery
{
    public class ExprCompareString : ExprCompare
    {
        public ComparisonString Operator;
        public bool IgnoreCase;

        public ExprCompareString(Expr left, Expr right, ComparisonString op) :
            base(left, right)
        {
            this.Operator = op;
        }

        public override void ToExprString(ExBuilder sb)
        {
            var left = this.LeftValue;
            var right = this.RightValue;

            if (this.IgnoreCase)
            {
                left = new ExprToLower(this.LeftValue);
                right = new ExprToLower(this.RightValue);
            }

            if (this.Operator == ComparisonString.Equals)
            {
                string op = "eq";
                this.WriteBinaryOperation(sb,op,left,right);
            }
            else if (this.Operator == ComparisonString.NotEquals)
            {
                string op = "ne";
                this.WriteBinaryOperation(sb, op, left, right);
            }
            else if (this.Operator == ComparisonString.Contains)
            {
                this.WriteFunction2(sb,"substringof", right,left);
            }
            else if (this.Operator == ComparisonString.StartsWith)
            {
                this.WriteFunction2(sb,"substringof", left,right);
            }
            else if (this.Operator == ComparisonString.EndsWith)
            {
                this.WriteFunction2(sb, "endswith", left, right);
            }
            else
            {
                throw new System.ArgumentOutOfRangeException();
            }
        }
    }
}