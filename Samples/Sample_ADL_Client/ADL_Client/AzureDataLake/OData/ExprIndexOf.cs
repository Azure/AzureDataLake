namespace AzureDataLake.ODataQuery
{
    public class ExprIndexOf : Expr
    {
        public Expr Expression1;
        public Expr Expression2;

        public ExprIndexOf(Expr expr1, Expr expr2)
        {
            this.Expression1 = expr1;
            this.Expression2 = expr2;
        }

        public override void ToExprString(ExBuilder sb)
        {
            this.WriteFunction2(sb, "indexof", this.Expression1, this.Expression2);
        }
    }
}