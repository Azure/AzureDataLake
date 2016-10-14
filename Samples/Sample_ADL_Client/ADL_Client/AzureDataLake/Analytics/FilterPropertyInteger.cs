using AzureDataLake.ODataQuery;

namespace AzureDataLake.Analytics
{
    public class FilterPropertyInteger : FilterProperty
    {
        private int? value_before;
        private int? value_after;
        private int? value_exactly;

        public FilterPropertyInteger(string colname) :
            base(colname)
        {
        }
        public void Before(int value)
        {
            this.value_before = value;
        }

        public void After(int value)
        {
            this.value_after = value;
        }

        public void Exactly(int value)
        {
            this.value_exactly = value;
        }
        
        public ODataQuery.Expr ToExpr()
        {
            if (!(this.value_before.HasValue || this.value_after.HasValue || this.value_exactly.HasValue))
            {
                return null;
            }

            var expr1 = new ODataQuery.ExprLogicalAnd();

            if (this.value_before.HasValue)
            {
                var expr2 = new ODataQuery.ExprCompareNumeric(this.expr_field, new ODataQuery.ExprLiteralInt(this.value_before.Value), ComparisonNumeric.LesserThan);
                expr1.Add(expr2);
            }

            if (this.value_after.HasValue)
            {
                var expr2 = new ODataQuery.ExprCompareNumeric(this.expr_field, new ODataQuery.ExprLiteralInt(this.value_after.Value), ComparisonNumeric.GreaterThan);
                expr1.Add(expr2);
            }

            if (this.value_exactly.HasValue)
            {
                var expr2 = new ODataQuery.ExprCompareNumeric(this.expr_field, new ODataQuery.ExprLiteralInt(this.value_exactly.Value), ComparisonNumeric.Equals);
                expr1.Add(expr2);
            }

            return expr1;
        }
    }
}