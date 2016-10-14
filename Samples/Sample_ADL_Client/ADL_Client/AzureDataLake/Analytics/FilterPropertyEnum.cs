using System.Collections.Generic;
using AzureDataLake.ODataQuery;

namespace AzureDataLake.Analytics
{
    public class FilterPropertyEnum<T> where T : struct, System.IComparable, System.IConvertible, System.IFormattable 
    {
        private ODataQuery.ExprColumn expr_col;
        private List<T> OneOfList;

        public FilterPropertyEnum(string colname)
        {
            this.expr_col = new ExprColumn(colname);
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
                    var zzz = (T) item;
                    string stringValue = zzz.ToString();
                    var z = new ODataQuery.ExprLiteralString(stringValue);
                    var expr2 = new ODataQuery.ExprCompareString(this.expr_col, z, ODataQuery.ComparisonString.Equals );
                    expr1.Add(expr2);
                }
                return expr1;
            }
            return null;
        }

        public static string GetEnumDescription(System.Enum value)
        {
            var fi = value.GetType().GetField(value.ToString());

            var attributes =
              (System.ComponentModel.DescriptionAttribute[])fi.GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);

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