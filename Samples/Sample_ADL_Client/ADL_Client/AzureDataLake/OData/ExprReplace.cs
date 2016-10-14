namespace AzureDataLake.ODataQuery
{
    public class ExprReplace : Expr
    {
        public Expr Expression1;
        public Expr Expression2;
        public Expr Expression3;

        public ExprReplace(Expr expr1, Expr expr2, Expr expr3)
        {
            this.Expression1 = expr1;
            this.Expression2 = expr2;
            this.Expression3 = expr3;
        }

        public override void ToExprString(ExBuilder sb)
        {
            this.WriteFunction3(sb, "indexof", this.Expression1,this.Expression2,this.Expression2);
        }
    }
}