using System.Collections.Generic;

namespace AzureDataLake.ODataQuery
{
    public class ExprLogicalAnd : Expr
    {
        public List<Expr> Items;

        public ExprLogicalAnd(params Expr[] items)
        {
            this.Items = new List<Expr>();
            this.Items.AddRange(items);
        }

        public override void ToExprString(ExBuilder sb)
        {
            if (this.Items.Count < 1)
            {
                return;
            }

            sb.Append("(");
            for (int i = 0; i < this.Items.Count; i++)
            {
                if (i > 0)
                {
                    sb.Append(" and ");
                }

                sb.Append(this.Items[i]);
            }

            sb.Append(")");
        }
    }
}