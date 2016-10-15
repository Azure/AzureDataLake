namespace AzureDataLake.ODataQuery
{
    public class ExprLogicalNot : Expr
    {
        public Expr Expression;

        public ExprLogicalNot(Expr expr)
        {
            this.Expression = expr;
        }

        public override void ToExprString(ExpressionWriter writer)
        {
            writer.Append("(not");
            writer.Append(this.Expression);
            writer.Append(")");
        }
    }
}