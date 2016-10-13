namespace AzureDataLake.ODataQuery
{
    public abstract class ExprCompare : Expr
    {
        public Expr LeftValue;
        public Expr RightValue;

        public ExprCompare(Expr left, Expr right)
        {
            this.LeftValue = left;
            this.RightValue = right;
        }

    }
}