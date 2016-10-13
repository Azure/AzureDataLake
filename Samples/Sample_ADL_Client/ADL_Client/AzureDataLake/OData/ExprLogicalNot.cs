using System.Collections.Generic;

namespace AzureDataLake.ODataQuery
{
    public class ExprLogicalNot : Expr
    {
        public Expr Expression;

        public ExprLogicalNot(Expr item)
        {
            this.Expression = item;
        }

        public override void ToExprString(ExBuilder sb)
        {
            sb.Append("(not ");
            sb.Append(this.Expression);
            sb.Append(")");
        }
    }
}