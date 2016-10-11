using System.Collections.Generic;

namespace AzureDataLake.ODataQuery
{

    public class ExBuilder
    {
        private System.Text.StringBuilder sb;

        public ExBuilder()
        {
            this.sb = new System.Text.StringBuilder();    
        }

        public void Append(string s)
        {
            this.sb.Append(s);
        }

        public void AppendQuotedString(string s)
        {
            this.sb.Append(string.Format("'{0}'",s));
        }

        public void Append(int s)
        {
            this.sb.Append(s);
        }

        public void Append(Expr e)
        {
            e.ToExprString(this);
        }

        public override string ToString()
        {
            return this.sb.ToString();
        }
    }

    public class ExprStringLiteral : Expr
    {
        public string Content;

        public ExprStringLiteral(string content)
        {
            this.Content = content;
        }

        public override void ToExprString(ExBuilder sb)
        {
            sb.AppendQuotedString(this.Content);
        }
    }

    public class ExprColumn : Expr
    {
        public string Name;

        public ExprColumn(string name)
        {
            this.Name = name;
        }

        public override void ToExprString(ExBuilder sb)
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