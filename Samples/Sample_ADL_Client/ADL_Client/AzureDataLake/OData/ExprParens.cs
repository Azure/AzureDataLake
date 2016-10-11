namespace AzureDataLake.ODataQuery
{
    public class ExprParens : Expr
    {
        public Expr Item;
        public ExprParens( Expr item)
        {
            this.Item = item;
        }

        public override void ToExprString(ExBuilder sb)
        {
            sb.Append("(");
            sb.Append(this.Item);
            sb.Append(")");
        }
    }
}