using AzureDataLake.ODataQuery;

namespace AzureDataLake.Analytics
{
    public abstract class PropertyFilter
    {
        protected ODataQuery.ExprField expr_field;

        public PropertyFilter(string field_name)
        {
            this.expr_field = new ExprField(field_name);
        }
    }
}