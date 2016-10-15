namespace AzureDataLake.ODataQuery
{
    public class ExprYear : ExprFunction1
    {
        public ExprYear(Expr expr) :
            base(expr,"year")
        {
        }
    }
}