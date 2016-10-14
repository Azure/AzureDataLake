namespace AzureDataLake.ODataQuery
{
    public class ExprFloor : ExprFunction1
    {
        public Expr Expression;

        public ExprFloor(Expr expr) :
            base(expr, "floor")
        {
        }
    }
}