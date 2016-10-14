namespace AzureDataLake.ODataQuery
{
    public class ExprSecond : ExprFunction1
    {
        public Expr Expression;

        public ExprSecond(Expr expr) :
            base(expr, "second")
        {
        }
    }
}