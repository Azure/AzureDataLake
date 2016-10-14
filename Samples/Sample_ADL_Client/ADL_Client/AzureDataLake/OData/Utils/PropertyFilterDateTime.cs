using AzureDataLake.ODataQuery;

namespace AzureDataLake.ODataQuery
{
    public class PropertyFilterDateTime : PropertyFilter
    {
        private System.DateTime? before_value;
        private System.DateTime? after_value;

        public PropertyFilterDateTime(string field_name) :
            base(field_name)
        {
        }

        public void Before(System.DateTime value)
        {
            this.before_value = value;
        }

        public void After(System.DateTime value)
        {
            this.after_value = value;
        }

        public override ODataQuery.Expr ToExpr()
        {
            if (!(this.before_value.HasValue || this.after_value.HasValue))
            {
                return null;
            }

            var expr_and = new ODataQuery.ExprLogicalAnd();

            if (this.before_value.HasValue)
            {
                var expr_compare = new ODataQuery.ExprCompareDateTime(this.expr_field, new ODataQuery.ExprLiteralDateTime(this.before_value.Value), ComparisonDateTime.LesserThan);
                expr_and.Add(expr_compare);
            }

            if (this.after_value.HasValue)
            {
                var expr_compare = new ODataQuery.ExprCompareDateTime(this.expr_field, new ODataQuery.ExprLiteralDateTime(this.after_value.Value), ComparisonDateTime.GreaterThan);
                expr_and.Add(expr_compare);
            }

            return expr_and;
        }
    }
}