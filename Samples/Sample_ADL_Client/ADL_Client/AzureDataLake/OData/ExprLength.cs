namespace AzureDataLake.ODataQuery
{
    public class ExprLength : ExprFunction1
    {
        public Expr Expression;

        public ExprLength(Expr expr) :
            base(expr, "length")
        {
        }
    }
}