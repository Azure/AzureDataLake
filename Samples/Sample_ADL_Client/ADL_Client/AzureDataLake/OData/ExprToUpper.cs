namespace AzureDataLake.ODataQuery
{
    public class ExprToUpper : Expr
    {
        public Expr Expression;

        public ExprToUpper(Expr expr)
        {
            this.Expression = expr;
        }

        public override void ToExprString(ExpressionWriter writer)
        {
            this.WriteFunction1(writer, "toupper", this.Expression);
        }
    }
}