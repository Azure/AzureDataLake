using System.Collections.Generic;

namespace AzureDataLake.Analytics
{
    public class PropertyFilterString : PropertyFilter
    {
        private List<string> one_of_text;
        private string begins_with_text;
        private string ends_with_text;
        private string contains_text;

        public PropertyFilterString(string field_name) :
            base(field_name)
        {
        }

        public void OneOf(params string[] items)
        {
            this.one_of_text = new List<string>(items.Length);
            this.one_of_text.AddRange(items);    
        }

        public void BeginsWith(string text)
        {
            this.begins_with_text = text;
        }

        public void EndsWith(string text)
        {
            this.ends_with_text = text;
        }

        public void Contains(string text)
        {
            this.contains_text = text;
        }

        public override ODataQuery.Expr ToExpr()
        {
            if (this.one_of_text != null && this.one_of_text.Count > 0)
            {
                var expr_or = new ODataQuery.ExprLogicalOr();
                foreach (var item in this.one_of_text)
                {
                    var expr_compare = new ODataQuery.ExprCompareString(this.expr_field, new ODataQuery.ExprLiteralString(item), ODataQuery.ComparisonString.Equals );
                    expr_or.Add(expr_compare);
                }
                return expr_or;
            }
            if (this.contains_text !=null)
            {
                var expr_compare = new ODataQuery.ExprCompareString(this.expr_field, new ODataQuery.ExprLiteralString(this.contains_text), ODataQuery.ComparisonString.Contains);
                return expr_compare;
            }

            if (this.ends_with_text != null)
            {
                var expr_compare = new ODataQuery.ExprCompareString(this.expr_field, new ODataQuery.ExprLiteralString(this.ends_with_text), ODataQuery.ComparisonString.EndsWith);
                return expr_compare;
            }

            if (this.begins_with_text != null)
            {
                var expr_compare = new ODataQuery.ExprCompareString(this.expr_field, new ODataQuery.ExprLiteralString(this.begins_with_text), ODataQuery.ComparisonString.StartsWith);
                return expr_compare;
            }

            return null;
        }
    }
}