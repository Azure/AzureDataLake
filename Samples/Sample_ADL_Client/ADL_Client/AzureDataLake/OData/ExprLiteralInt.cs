namespace AzureDataLake.ODataQuery
{
    public class ExprLiteralInt : Expr
    {
        public int Integer;

        public ExprLiteralInt(int integer)
        {
            this.Integer = integer;
        }

        public override void ToExprString(ExpressionWriter writer)
        {
            writer.Append(this.Integer.ToString());
        }
    }
}