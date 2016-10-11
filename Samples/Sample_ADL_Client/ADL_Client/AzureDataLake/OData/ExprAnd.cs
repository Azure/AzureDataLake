using System.Collections.Generic;

namespace AzureDataLake.ODataQuery
{

    public class ExprColumn : Expr
    {
        public string Name;

        public ExprColumn(string name)
        {
            this.Name = name;
        }

        public override void ToExprString(System.Text.StringBuilder sb)
        {
            sb.Append(this.Name);
        }
    }

    public class ExprAnd : Expr
    {
        public List<Expr> Items;

        public ExprAnd(params Expr[] items)
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
                    sb.Append(" and ");
                }

                sb.AppendExpr(this.Items[i]);
            }

            sb.Append(")");
        }
    }
}