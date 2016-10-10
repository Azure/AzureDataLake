using System.Collections.Generic;
using System.Linq;

namespace AzureDataLake.ODataQuery
{
    public abstract class Expr
    {

        public abstract void ToExprString(System.Text.StringBuilder sb);
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

                this.Items[i].ToExprString(sb);
            }

            sb.Append(")");
        }
    }

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

                this.Items[i].ToExprString(sb);
            }

            sb.Append(")");
        }
    }

    public class ExprParens : Expr
    {
        public Expr Item;
        public ExprParens( Expr item)
        {
            this.Item = item;
        }

        public override void ToExprString(System.Text.StringBuilder sb)
        {
            sb.Append("(");
            this.Item.ToExprString(sb);
            sb.Append(")");
        }
    }

    public class ExprStringEquals : Expr
    {
        public string Column;
        public string Value;
        public ExprStringEquals(string col, string val)
        {
            this.Column = col;
            this.Value = val;
        }

        public override void ToExprString(System.Text.StringBuilder sb)
        {
            sb.Append(string.Format("{0} eq '{1}'", this.Column, this.Value));
        }
    }

    public class ExprDateTimeAfter : Expr
    {
        public string Column;
        public System.DateTime Value;
        public ExprDateTimeAfter(string col, System.DateTime val)
        {
            this.Column = col;
            this.Value = val;
        }

        public override void ToExprString(System.Text.StringBuilder sb)
        {
            string datestring = this.Value.ToString("O");

            // due to issue: https://github.com/Azure/autorest/issues/975,
            // date time offsets must be explicitly escaped before being passed to the filter

            var escaped_datestring = System.Uri.EscapeDataString(datestring);

            sb.Append(string.Format("{0} ge datetimeoffset'{1}'", this.Column, escaped_datestring));
        }
    }

    public class ExprRaw : Expr
    {
        public string Item;
        public ExprRaw(string s)
        {
            this.Item = s;
        }

        public override void ToExprString(System.Text.StringBuilder sb)
        {
            sb.Append(this.Item);
        }
    }

}

