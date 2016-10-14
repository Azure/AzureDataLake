using AzureDataLake.ODataQuery;

namespace AzureDataLake.ODataQuery
{
    public class PropertyFilterInteger : PropertyFilter
    {
        private int? before_value;
        private int? after_value;
        private int? exactly_value;

        public PropertyFilterInteger(string field_name) :
            base(field_name)
        {
        }
        public void Before(int value)
        {
            this.before_value = value;
        }

        public void After(int value)
        {
            this.after_value = value;
        }

        public void Exactly(int value)
        {
            this.exactly_value = value;
        }
        
        public override ODataQuery.Expr ToExpr()
        {
            if (!(this.before_value.HasValue || this.after_value.HasValue || this.exactly_value.HasValue))
            {
                return null;
            }

            var expr_and = new ODataQuery.ExprLogicalAnd();

            if (this.before_value.HasValue)
            {
                var expr_compare = new ODataQuery.ExprCompareNumeric(this.expr_field, new ODataQuery.ExprLiteralInt(this.before_value.Value), ComparisonNumeric.LesserThan);
                expr_and.Add(expr_compare);
            }

            if (this.after_value.HasValue)
            {
                var expr_compare = new ODataQuery.ExprCompareNumeric(this.expr_field, new ODataQuery.ExprLiteralInt(this.after_value.Value), ComparisonNumeric.GreaterThan);
                expr_and.Add(expr_compare);
            }

            if (this.exactly_value.HasValue)
            {
                var expr_compare = new ODataQuery.ExprCompareNumeric(this.expr_field, new ODataQuery.ExprLiteralInt(this.exactly_value.Value), ComparisonNumeric.Equals);
                expr_and.Add(expr_compare);
            }

            return expr_and;
        }
    }
}