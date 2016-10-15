namespace AzureDataLake.ODataQuery
{
    public class ExprLogicalOr : ExprLogicalList
    {
        public ExprLogicalOr(params Expr[] expressions) :
            base(expressions)
        {
            this.AddRange(expressions);
        }

        public override void ToExprString(ExpressionWriter writer)
        {
            this.WriteItems(writer, " or ");
        }
    }
}