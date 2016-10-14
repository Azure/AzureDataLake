using AzureDataLake.ODataQuery;

namespace AzureDataLake.Analytics
{
    public abstract class FilterProperty
    {
        protected ODataQuery.ExprField expr_field;

        public FilterProperty(string field_name)
        {
            this.expr_field = new ExprField(field_name);
        }
    }
}