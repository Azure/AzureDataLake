namespace AzureDataLake.ODataQuery
{
    public class ExprLogicalNot : Expr
    {
        public Expr Expression;

        public ExprLogicalNot(Expr expr)
        {
            this.Expression = expr;
        }

        public override void ToExprString(ExpressionWriter sb)
        {
            sb.Append("(not");
            sb.Append(this.Expression);
            sb.Append(")");
        }
    }
}