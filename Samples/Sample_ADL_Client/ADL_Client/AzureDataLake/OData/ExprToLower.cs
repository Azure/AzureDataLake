namespace AzureDataLake.ODataQuery
{
    public class ExprToLower : Expr
    {
        public Expr Expression;

        public ExprToLower(Expr expr)
        {
            this.Expression = expr;
        }

        public override void ToExprString(ExBuilder sb)
        {
            this.WriteFunction1(sb, "tolower", this.Expression);
        }
    }
}