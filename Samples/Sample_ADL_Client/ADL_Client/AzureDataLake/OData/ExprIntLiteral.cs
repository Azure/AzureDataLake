namespace AzureDataLake.ODataQuery
{
    public class ExprIntLiteral : Expr
    {
        public int Integer;

        public ExprIntLiteral(int integer)
        {
            this.Integer = integer;
        }

        public override void ToExprString(ExBuilder sb)
        {
            sb.Append(this.Integer.ToString());
        }
    }
}