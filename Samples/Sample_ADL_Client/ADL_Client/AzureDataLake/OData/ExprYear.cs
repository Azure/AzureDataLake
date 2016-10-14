namespace AzureDataLake.ODataQuery
{
    public class ExprYear : ExprFunction1
    {
        public Expr Expression;

        public ExprYear(Expr expr) :
            base(expr,"year")
        {
        }
    }
}