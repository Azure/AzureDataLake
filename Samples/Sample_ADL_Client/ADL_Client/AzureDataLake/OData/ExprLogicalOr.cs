namespace AzureDataLake.ODataQuery
{
    public class ExprLogicalOr : ExprList
    {
        public ExprLogicalOr(params Expr[] items) :
            base(items)
        {
            this.AddRange(items);
        }

        public override void ToExprString(ExBuilder sb)
        {
            this.WriteItems(sb, " or ");
        }
    }
}