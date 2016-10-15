using AzureDataLake.ODataQuery;

namespace AzureDataLake.ODataQuery
{
    public class PropertyFilterDateTime : PropertyFilter
    {
        private System.DateTime? before_value;
        private System.DateTime? after_value;
        public bool Inclusive;

        public PropertyFilterDateTime(string field_name) :
            base(field_name)
        {
        }

        public void Before(System.DateTime value)
        {
            this.before_value = value;
            this.after_value = null;
        }

        public void After(System.DateTime value)
        {
            this.before_value = null;
            this.after_value = value;
        }

        public void InTheLastNHours(int hours)
        {
            var now = System.DateTime.UtcNow;
            this.before_value = null;
            this.after_value = now.AddHours(-hours);
        }

        public void InTheLastNDays(int days)
        {
            var now = System.DateTime.UtcNow;
            this.before_value = null;
            this.after_value = now.AddDays(-days);
        }

        public void SinceLocalMidnight()
        {
            var now_localtime = System.DateTime.Now; 
            var midnight_utc = new System.DateTime(now_localtime.Year, now_localtime.Month, now_localtime.Day).ToUniversalTime();
            this.before_value = null;
            this.after_value = midnight_utc;
        }

        public void Between(System.DateTime lower, System.DateTime upper)
        {
            this.before_value = upper;
            this.after_value = lower;
        }

        public override ODataQuery.Expr ToExpression()
        {
            if (!(this.before_value.HasValue || this.after_value.HasValue))
            {
                return null;
            }

            var expr_and = new ODataQuery.ExprLogicalAnd();

            if (this.before_value.HasValue)
            {
                var op = this.Inclusive ? ComparisonDateTime.GreaterThanOrEquals : ComparisonDateTime.LesserThan;
                var expr_compare = new ODataQuery.ExprCompareDateTime(this.expr_field, new ODataQuery.ExprLiteralDateTime(this.before_value.Value), op);
                expr_and.Add(expr_compare);
            }

            if (this.after_value.HasValue)
            {
                var op = this.Inclusive ? ComparisonDateTime.GreaterThanOrEquals : ComparisonDateTime.GreaterThan;
                var expr_compare = new ODataQuery.ExprCompareDateTime(this.expr_field, new ODataQuery.ExprLiteralDateTime(this.after_value.Value), op);
                expr_and.Add(expr_compare);
            }

            return expr_and;
        }
    }
}