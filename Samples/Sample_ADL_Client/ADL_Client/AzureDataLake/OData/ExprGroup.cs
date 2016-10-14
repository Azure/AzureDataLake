namespace AzureDataLake.ODataQuery
{
    public class ExprGroup : Expr
    {
        public Expr Item;
        public ExprGroup( Expr item)
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