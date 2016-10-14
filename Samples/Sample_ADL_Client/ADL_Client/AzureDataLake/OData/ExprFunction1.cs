namespace AzureDataLake.ODataQuery
{
    public abstract class ExprFunction1 : Expr
    {
        public Expr Expression;
        public string name;

        public ExprFunction1(Expr expr, string name)
        {
            this.Expression = expr;
            this.name = name;
        }

        public override void ToExprString(ExBuilder sb)
        {
            this.WriteFunction1(sb, this.name, this.Expression);
        }
    }
}