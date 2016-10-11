namespace AzureDataLake.ODataQuery
{
    public class ExprParens : Expr
    {
        public Expr Item;
        public ExprParens( Expr item)
        {
            this.Item = item;
        }

        public override void ToExprString(System.Text.StringBuilder sb)
        {
            sb.Append("(");
            this.Item.ToExprString(sb);
            sb.Append(")");
        }
    }
}