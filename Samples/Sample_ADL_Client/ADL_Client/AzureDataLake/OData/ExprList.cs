using System.Collections.Generic;

namespace AzureDataLake.ODataQuery
{
    public abstract class ExprList : Expr
    {
        private List<Expr> Items;

        public ExprList(params Expr[] items)
        {
            this.Items = new List<Expr>();
            this.Items.AddRange(items);
        }

        public void Add(Expr e)
        {
            if (e != null)
            {
                this.Items.Add(e);
            }
        }

        public void AddRange(IEnumerable<Expr> e)
        {
            foreach (var item in e)
            {
                this.Add(item);
            }
        }


        public void WriteItems(ExBuilder sb, string op)
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
                    sb.Append(op);
                }

                sb.Append(this.Items[i]);
            }

            sb.Append(")");
        }

    }
}