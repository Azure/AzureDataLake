namespace AzureDataLake.ODataQuery
{
    public class ExprLogicalOr : ExprList
    {
        public ExprLogicalOr(params Expr[] items) :
            base(items)
        {
            this.AddRange(items);
        }

        public override void ToExprString(ExpressionWriter sb)
        {
            this.WriteItems(sb, " or ");
        }
    }
}