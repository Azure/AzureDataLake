using System.Collections.Generic;

namespace AzureDataLake.ODataQuery
{
    public class ExprOr : Expr
    {
        public List<Expr> Items;
        public ExprOr(params Expr[] items)
        {
            this.Items = new List<Expr>();
            this.Items.AddRange(items);
        }

        public override void ToExprString(System.Text.StringBuilder sb)
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
                    sb.Append(" or ");
                }

                sb.AppendExpr(this.Items[i]);
            }

            sb.Append(")");
        }
    }
}