namespace AzureDataLake.ODataQuery
{
    public class ExprNull : Expr
    {
        public ExprNull()
        {
        }

        public override void ToExprString(ExpressionWriter writer)
        {
            writer.Append("null");
        }
    }
}