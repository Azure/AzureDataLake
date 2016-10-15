namespace AzureDataLake.ODataQuery
{
    public class ExprToLower : ExprFunction1
    {
        public Expr Expression;

        public ExprToLower(Expr expr) :
            base(expr, "tolower")
        {
        }
    }
}