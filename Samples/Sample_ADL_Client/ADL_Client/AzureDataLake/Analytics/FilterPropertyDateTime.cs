using AzureDataLake.ODataQuery;

namespace AzureDataLake.Analytics
{
    public class FilterPropertyDateTime : FilterProperty
    {
        private System.DateTime? value_before;
        private System.DateTime? value_after;

        public FilterPropertyDateTime(string colname) :
            base(colname)
        {
        }

        public void Before(System.DateTime value)
        {
            this.value_before = value;
        }

        public void After(System.DateTime value)
        {
            this.value_after = value;
        }

        public ODataQuery.Expr ToExpr()
        {
            if (!(this.value_before.HasValue || this.value_after.HasValue))
            {
                return null;
            }

            var expr1 = new ODataQuery.ExprLogicalAnd();

            if (this.value_before.HasValue)
            {
                var expr2 = new ODataQuery.ExprCompareDateTime(this.expr_field, new ODataQuery.ExprLiteralDateTime(this.value_before.Value), ComparisonDateTime.LesserThan);
                expr1.Add(expr2);
            }

            if (this.value_after.HasValue)
            {
                var expr2 = new ODataQuery.ExprCompareDateTime(this.expr_field, new ODataQuery.ExprLiteralDateTime(this.value_after.Value), ComparisonDateTime.GreaterThan);
                expr1.Add(expr2);
            }

            return expr1;
        }
    }
}