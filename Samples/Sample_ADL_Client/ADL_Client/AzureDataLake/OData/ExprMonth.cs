namespace AzureDataLake.ODataQuery
{
    public class ExprMonth : ExprFunction1
    {
        public Expr Expression;

        public ExprMonth(Expr expr) :
            base(expr, "month")
        {
        }
    }
}