using System.Collections.Generic;

namespace AzureDataLake.Analytics
{
    public class FilterPropertyString : FilterProperty
    {
        private List<string> OneOfList;
        private string beginsswith_prefix;
        private string endswith_suffix;
        private string contains;

        public FilterPropertyString(string colname) :
            base(colname)
        {
        }

        public void OneOf(params string[] items)
        {
            this.OneOfList = new List<string>(items.Length);
            this.OneOfList.AddRange(items);    
        }

        public void BeginsWith(string text)
        {
            this.beginsswith_prefix = text;
        }

        public void EndsWith(string text)
        {
            this.endswith_suffix = text;
        }

        public void Contains(string text)
        {
            this.contains = text;
        }

        public ODataQuery.Expr ToExpr()
        {
            if (this.OneOfList != null && this.OneOfList.Count > 0)
            {
                var expr1 = new ODataQuery.ExprLogicalOr();
                foreach (var item in this.OneOfList)
                {
                    var expr2 = new ODataQuery.ExprCompareString(this.expr_field, new ODataQuery.ExprLiteralString(item), ODataQuery.ComparisonString.Equals );
                    expr1.Add(expr2);
                }
                return expr1;
            }
            if (this.contains !=null)
            {
                var expr1 = new ODataQuery.ExprCompareString(this.expr_field, new ODataQuery.ExprLiteralString(this.contains), ODataQuery.ComparisonString.Contains);
                return expr1;
            }

            if (this.endswith_suffix != null)
            {
                var expr1 = new ODataQuery.ExprCompareString(this.expr_field, new ODataQuery.ExprLiteralString(this.endswith_suffix), ODataQuery.ComparisonString.EndsWith);
                return expr1;
            }

            if (this.beginsswith_prefix != null)
            {
                var expr1 = new ODataQuery.ExprCompareString(this.expr_field, new ODataQuery.ExprLiteralString(this.beginsswith_prefix), ODataQuery.ComparisonString.StartsWith);
                return expr1;
            }

            return null;
        }
    }
}