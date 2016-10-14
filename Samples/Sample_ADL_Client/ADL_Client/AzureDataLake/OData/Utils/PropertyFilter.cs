
namespace AzureDataLake.ODataQuery
{
    public abstract class PropertyFilter
    {
        protected ODataQuery.ExprField expr_field;

        public PropertyFilter(string field_name)
        {
            this.expr_field = new ExprField(field_name);
        }

        public abstract ODataQuery.Expr ToExpression();
    }
}