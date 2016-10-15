using AzureDataLake.ODataQuery;

namespace AzureDataLake.ODataQuery
{
    public class PropertyFilterInteger : PropertyFilter
    {
        private int? upper_value;
        private int? lower_value;
        private int? exactly_value;
        public bool IsInclusive;

        public PropertyFilterInteger(string field_name) :
            base(field_name)
        {
        }

        public void LessThan(int value)
        {
            this.upper_value = value;
            this.lower_value = null;
            this.exactly_value = null;
        }

        public void GreaterThan(int value)
        {
            this.upper_value = null;
            this.lower_value = value;
            this.exactly_value = null;
        }

        public void InRange(int lower, int upper)
        {
            this.upper_value = upper;
            this.lower_value = lower;
            this.exactly_value = null;
        }


        public void Exactly(int value)
        {
            this.upper_value = null;
            this.lower_value = null;
            this.exactly_value = value;
        }
        
        public override ODataQuery.Expr ToExpression()
        {
            if (!(this.upper_value.HasValue || this.lower_value.HasValue || this.exactly_value.HasValue))
            {
                return null;
            }

            var expr_and = new ODataQuery.ExprLogicalAnd();

            if (this.upper_value.HasValue)
            {
                var op = this.IsInclusive ? ComparisonNumeric.LesserThanOrEquals : ComparisonNumeric.LesserThan;
                var expr_compare = new ODataQuery.ExprCompareNumeric(this.expr_field, new ODataQuery.ExprLiteralInt(this.upper_value.Value), op );
                expr_and.Add(expr_compare);
            }

            if (this.lower_value.HasValue)
            {
                var op = this.IsInclusive ? ComparisonNumeric.GreaterThanOrEquals : ComparisonNumeric.GreaterThan;
                var expr_compare = new ODataQuery.ExprCompareNumeric(this.expr_field, new ODataQuery.ExprLiteralInt(this.lower_value.Value), op );
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