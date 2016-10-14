namespace AzureDataLake.ODataQuery
{
    public class ExprDay: ExprFunction1
    {
        public Expr Expression;

        public ExprDay(Expr expr) :
            base(expr, "day")
        {
        }
    }
}