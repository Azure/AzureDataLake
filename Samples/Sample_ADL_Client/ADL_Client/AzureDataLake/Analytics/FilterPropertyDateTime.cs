using AzureDataLake.ODataQuery;

namespace AzureDataLake.Analytics
{
    public class FilterPropertyDateTime
    {
        private ODataQuery.ExprColumn expr_col;
        private System.DateTime? value_before;
        private System.DateTime? value_after;

        public FilterPropertyDateTime(string colname)
        {
            this.expr_col = new ExprColumn(colname);
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
                var expr2 = new ODataQuery.ExprCompareDateTime(this.expr_col, new ODataQuery.ExprLiteralDateTime(this.value_before.Value), NumericComparisonOperator.LesserThan);
                expr1.Items.Add(expr2);
            }

            if (this.value_after.HasValue)
            {
                var expr2 = new ODataQuery.ExprCompareDateTime(this.expr_col, new ODataQuery.ExprLiteralDateTime(this.value_after.Value), NumericComparisonOperator.GreaterThan);
                expr1.Items.Add(expr2);
            }

            return expr1;
        }
    }
}