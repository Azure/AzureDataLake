namespace AzureDataLake.ODataQuery
{
    public class ExprToUpper : Expr
    {
        public Expr Expression;

        public ExprToUpper(Expr expr)
        {
            this.Expression = expr;
        }

        public override void ToExprString(ExpressionWriter sb)
        {
            this.WriteFunction1(sb, "toupper", this.Expression);
        }
    }
}