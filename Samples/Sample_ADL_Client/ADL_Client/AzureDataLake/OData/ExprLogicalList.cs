using System.Collections.Generic;

namespace AzureDataLake.ODataQuery
{
    public abstract class ExprLogicalList : Expr
    {
        private List<Expr> Expressions;

        public ExprLogicalList(params Expr[] expressions)
        {
            this.Expressions = new List<Expr>();
            this.Expressions.AddRange(expressions);
        }

        public void Add(Expr e)
        {
            if (e != null)
            {
                this.Expressions.Add(e);
            }
        }

        public void AddRange(IEnumerable<Expr> e)
        {
            foreach (var item in e)
            {
                this.Add(item);
            }
        }


        public void WriteItems(ExpressionWriter writer, string op)
        {
            if (this.Expressions.Count < 1)
            {
                return;
            }

            writer.Append("(");
            for (int i = 0; i < this.Expressions.Count; i++)
            {
                if (i > 0)
                {
                    writer.Append(op);
                }

                writer.Append(this.Expressions[i]);
            }

            writer.Append(")");
        }

    }
}