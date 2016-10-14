namespace AzureDataLake.ODataQuery
{
    public class ExprCeiling : ExprFunction1
    {
        public Expr Expression;

        public ExprCeiling(Expr expr) :
            base(expr, "ceiling")
        {
        }
    }
}