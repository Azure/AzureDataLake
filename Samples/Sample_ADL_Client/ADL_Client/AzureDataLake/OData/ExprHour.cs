namespace AzureDataLake.ODataQuery
{
    public class ExprHour: ExprFunction1
    {
        public Expr Expression;

        public ExprHour(Expr expr) :
            base(expr, "hour")
        {
        }
    }
}