using AzureDataLake.ODataQuery;

namespace AzureDataLake.Analytics
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
        
        public ODataQuery.Expr ToExpr()
        {
            if (!(this.before_value.HasValue || this.after_value.HasValue || this.exactly_value.HasValue))
            {
                return null;
            }

            var expr1 = new ODataQuery.ExprLogicalAnd();

            if (this.before_value.HasValue)
            {
                var expr2 = new ODataQuery.ExprCompareNumeric(this.expr_field, new ODataQuery.ExprLiteralInt(this.before_value.Value), ComparisonNumeric.LesserThan);
                expr1.Add(expr2);
            }

            if (this.after_value.HasValue)
            {
                var expr2 = new ODataQuery.ExprCompareNumeric(this.expr_field, new ODataQuery.ExprLiteralInt(this.after_value.Value), ComparisonNumeric.GreaterThan);
                expr1.Add(expr2);
            }

            if (this.exactly_value.HasValue)
            {
                var expr2 = new ODataQuery.ExprCompareNumeric(this.expr_field, new ODataQuery.ExprLiteralInt(this.exactly_value.Value), ComparisonNumeric.Equals);
                expr1.Add(expr2);
            }

            return expr1;
        }
    }
}