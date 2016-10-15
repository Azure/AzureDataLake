namespace AzureDataLake.ODataQuery
{
    public class ExprGroup : Expr
    {
        public Expr Expression;
        public ExprGroup( Expr expression)
        {
            this.Expression = expression;
        }

        public override void ToExprString(ExpressionWriter writer)
        {
            writer.Append("(");
            writer.Append(this.Expression);
            writer.Append(")");
        }
    }
}