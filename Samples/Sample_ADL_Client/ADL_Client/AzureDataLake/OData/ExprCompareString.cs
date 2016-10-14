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
                this.WriteBinaryOperation(sb,op,this.LeftValue,this.RightValue);
            }
            else if (this.Operator == ComparisonString.NotEquals)
            {
                string op = "ne";
                this.WriteBinaryOperation(sb, op, this.LeftValue, this.RightValue);
            }
            else if (this.Operator == ComparisonString.Contains)
            {
                this.WriteFunction2(sb,"substringof", this.RightValue,this.LeftValue);
            }
            else if (this.Operator == ComparisonString.StartsWith)
            {
                this.WriteFunction2(sb,"substringof", this.LeftValue,this.RightValue);
            }
            else if (this.Operator == ComparisonString.EndsWith)
            {
                this.WriteFunction2(sb, "endswith", this.LeftValue, this.RightValue);
            }
            else
            {
                throw new System.ArgumentOutOfRangeException();
            }
        }
    }
}