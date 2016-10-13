using System.Collections.Generic;
using AzureDataLake.ODataQuery;

namespace AzureDataLake.Analytics
{
    public class FilterPropertyString
    {
        private ODataQuery.ExprColumn expr_col;
        private List<string> OneOfList;
        private string beginsswith_prefix;
        private string endswith_suffix;
        private string contains;

        public FilterPropertyString(string colname)
        {
            this.expr_col = new ExprColumn(colname);
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
                    var expr2 = new ODataQuery.ExprCompareString(this.expr_col, new ODataQuery.ExprLiteralString(item), ODataQuery.StringCompareOperator.Equals );
                    expr1.Items.Add(expr2);
                }
                return expr1;
            }
            if (this.contains !=null)
            {
                var expr1 = new ODataQuery.ExprCompareString(this.expr_col, new ODataQuery.ExprLiteralString(this.contains), ODataQuery.StringCompareOperator.Contains);
                return expr1;
            }

            if (this.endswith_suffix != null)
            {
                var expr1 = new ODataQuery.ExprCompareString(this.expr_col, new ODataQuery.ExprLiteralString(this.endswith_suffix), ODataQuery.StringCompareOperator.EndsWith);
                return expr1;
            }

            if (this.beginsswith_prefix != null)
            {
                var expr1 = new ODataQuery.ExprCompareString(this.expr_col, new ODataQuery.ExprLiteralString(this.beginsswith_prefix), ODataQuery.StringCompareOperator.StartsWith);
                return expr1;
            }

            return null;
        }
    }
}