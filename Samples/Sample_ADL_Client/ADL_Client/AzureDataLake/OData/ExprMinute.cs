namespace AzureDataLake.ODataQuery
{
    public class ExprMinute : ExprFunction1
    {
        public Expr Expression;

        public ExprMinute(Expr expr) :
            base(expr, "minute")
        {
        }
    }
}