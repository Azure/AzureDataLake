using System.Collections.Generic;
using AzureDataLake.ODataQuery;

namespace AzureDataLake.Analytics
{
    public class FilterPropertyEnum<T> : FilterProperty where T : struct, System.IComparable, System.IConvertible, System.IFormattable 
    {
        private List<T> OneOfList;

        public FilterPropertyEnum(string colname) :
            base(colname)
        {
        }

        public void OneOf(params T[] items)
        {
            this.OneOfList = new List<T>(items.Length);
            this.OneOfList.AddRange(items);    
        }

        public ODataQuery.Expr ToExpr()
        {
            if (this.OneOfList != null && this.OneOfList.Count > 0)
            {
                var expr1 = new ODataQuery.ExprLogicalOr();
                foreach (var item in this.OneOfList)
                {
                    var t_value = (T) item;
                    string t_string_value = t_value.ToString();
                    var expr_t = new ODataQuery.ExprLiteralString(t_string_value);
                    var expr_compare = new ODataQuery.ExprCompareString(this.expr_field, expr_t, ODataQuery.ComparisonString.Equals );
                    expr1.Add(expr_compare);
                }
                return expr1;
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