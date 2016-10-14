using System.Collections.Generic;
using AzureDataLake.ODataQuery;

namespace AzureDataLake.ODataQuery
{
    public class PropertyFilterEnum<T> : PropertyFilter where T : struct, System.IComparable, System.IConvertible, System.IFormattable 
    {
        private List<T> one_of_value;

        public PropertyFilterEnum(string field_name) :
            base(field_name)
        {
        }

        public void OneOf(params T[] items)
        {
            this.one_of_value = new List<T>(items.Length);
            this.one_of_value.AddRange(items);    
        }

        public override ODataQuery.Expr ToExpr()
        {
            if (this.one_of_value != null && this.one_of_value.Count > 0)
            {
                var expr_or = new ODataQuery.ExprLogicalOr();
                foreach (var item in this.one_of_value)
                {
                    var t_value = (T) item;
                    string t_string_value = t_value.ToString();
                    var expr_t = new ODataQuery.ExprLiteralString(t_string_value);
                    var expr_compare = new ODataQuery.ExprCompareString(this.expr_field, expr_t, ODataQuery.ComparisonString.Equals );
                    expr_or.Add(expr_compare);
                }
                return expr_or;
            }
            return null;
        }

        public static string GetEnumDescription(System.Enum value)
        {
            var field_info = value.GetType().GetField(value.ToString());

            var attributes =
              (System.ComponentModel.DescriptionAttribute[])field_info.GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);

            if (attributes != null && attributes.Length > 0)
            {
                return attributes[0].Description;
            }
            else
            {
                return value.ToString();
            }
        }
    }
}