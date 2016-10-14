namespace AzureDataLake.ODataQuery
{
    public class ExprGroup : Expr
    {
        public Expr Expression;
        public ExprGroup( Expr expression)
        {
            this.Expression = expression;
        }

        public override void ToExprString(ExBuilder sb)
        {
            sb.Append("(");
            sb.Append(this.Expression);
            sb.Append(")");
        }
    }
}