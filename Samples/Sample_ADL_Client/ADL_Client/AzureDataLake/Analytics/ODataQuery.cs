using System.Collections.Generic;
using System.Linq;

namespace AzureDataLake.ODataQuery
{
    public abstract class Expr
    {

        public abstract string ToExprString();
    }

    public class ExprAnd : Expr
    {
        public List<Expr> Items;
        public ExprAnd(params Expr[] items)
        {
            this.Items = new List<Expr>();
            this.Items.AddRange(items);
        }

        public override string ToExprString()
        {
            return string.Join(" and ", this.Items.Select( s=> "("+s.ToExprString()+")"));
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

        public override string ToExprString()
        {
            return string.Join(" or ", this.Items.Select(s => "(" + s.ToExprString() + ")"));
        }
    }

    public class ExprParens : Expr
    {
        public Expr Item;
        public ExprParens( Expr item)
        {
            this.Item = item;
        }

        public override string ToExprString()
        {
            return string.Format("({0})",this.Item.ToExprString());
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

        public override string ToExprString()
        {
            return string.Format("{0} eq '{1}'", this.Column, this.Value);
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

        public override string ToExprString()
        {
            string datestring = this.Value.ToString("O");

            // due to issue: https://github.com/Azure/autorest/issues/975,
            // date time offsets must be explicitly escaped before being passed to the filter

            var escaped_datestring = System.Uri.EscapeDataString(datestring);

            var expr = string.Format("{0} ge datetimeoffset'{1}'", this.Column, escaped_datestring);
            return expr;
        }
    }

    public class ExprRaw : Expr
    {
        public string Item;
        public ExprRaw(string s)
        {
            this.Item = s;
        }

        public override string ToExprString()
        {
            return this.Item;
        }
    }

}

