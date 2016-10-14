namespace AzureDataLake.ODataQuery
{
    public class ExprRound : ExprFunction1
    {
        public Expr Expression;

        public ExprRound(Expr expr) :
            base(expr, "round")
        {
        }
    }
}