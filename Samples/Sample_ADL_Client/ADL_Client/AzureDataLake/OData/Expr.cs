namespace AzureDataLake.ODataQuery
{
    public abstract class Expr
    {

        public abstract void ToExprString(System.Text.StringBuilder sb);
    }
}