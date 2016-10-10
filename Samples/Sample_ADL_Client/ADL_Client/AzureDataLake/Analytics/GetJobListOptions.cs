using System.Collections.Generic;
using System.Linq;

namespace AzureDataLake.ODataQuery
{
    public abstract class Expr
    {

        public abstract string ToExprString();
    }

    public class ExprAnd : Expr
    {
        public List<Expr> Items;
        public ExprAnd(params Expr[] items)
        {
            this.Items = new List<Expr>();
            this.Items.AddRange(items);
        }

        public override string ToExprString()
        {
            return string.Join(" and ", this.Items.Select( s=> "("+s.ToExprString()+")"));
        }
    }

    public class ExprOr : Expr
    {
        public List<Expr> Items;
        public ExprOr(params Expr[] items)
        {
            this.Items = new List<Expr>();
            this.Items.AddRange(items);
        }

        public override string ToExprString()
        {
            return string.Join(" or ", this.Items.Select(s => "(" + s.ToExprString() + ")"));
        }
    }

    public class ExprParens : Expr
    {
        public Expr Item;
        public ExprParens( Expr item)
        {
            this.Item = item;
        }

        public override string ToExprString()
        {
            return string.Format("({0})",this.Item.ToExprString());
        }
    }

    public class ExprStringEquals : Expr
    {
        public string Column;
        public string Value;
        public ExprStringEquals(string col, string val)
        {
            this.Column = col;
            this.Value = val;
        }

        public override string ToExprString()
        {
            return string.Format("{0} eq '{1}'", this.Column, this.Value);
        }
    }

    public class ExprDateTimeAfter : Expr
    {
        public string Column;
        public System.DateTime Value;
        public ExprDateTimeAfter(string col, System.DateTime val)
        {
            this.Column = col;
            this.Value = val;
        }

        public override string ToExprString()
        {
            string datestring = this.Value.ToString("O");

            // due to issue: https://github.com/Azure/autorest/issues/975,
            // date time offsets must be explicitly escaped before being passed to the filter

            var escaped_datestring = System.Uri.EscapeDataString(datestring);

            var expr = string.Format("{0} ge datetimeoffset'{1}'", this.Column, escaped_datestring);
            return expr;
        }
    }

    public class ExprRaw : Expr
    {
        public string Item;
        public ExprRaw(string s)
        {
            this.Item = s;
        }

        public override string ToExprString()
        {
            return this.Item;
        }
    }

}

namespace AzureDataLake.Analytics
{
    public class GetJobListOptions
    {
        public int Top=0; // 300 is the ADLA limit
        public JobOrderByField OrderByField;
        public JobOrderByDirection OrderByDirection;

        public string FilterSubmitter;
        public string FilterName;
        public System.DateTime? FilterSubmittedAfter;
        public System.DateTime? FilterSubmittedBefore;
        public Microsoft.Azure.Management.DataLake.Analytics.Models.JobState[] FilterState;
        public Microsoft.Azure.Management.DataLake.Analytics.Models.JobResult[] FilterResult;

        private static string get_order_field_name(JobOrderByField field)
        {
            if (field == JobOrderByField.None)
            {
                throw new System.ArgumentException();
            }

            string field_name_str = field.ToString();
            return StringUtil.ToLowercaseFirstLetter(field_name_str);
        }

        public string CreateOrderByString()
        {
            if (this.OrderByField != JobOrderByField.None)
            {
                var fieldname = get_order_field_name(this.OrderByField);
                var dir = (this.OrderByDirection == JobOrderByDirection.Ascending) ? "asc" : "desc";

                string orderBy = string.Format("{0} {1}", fieldname, dir);
                return orderBy;
            }

            return null;
        }

        public string CreateFilterString()
        {

            var q = new AzureDataLake.ODataQuery.ExprAnd();

            if (!string.IsNullOrEmpty(this.FilterSubmitter))
            {
                q.Items.Add( new AzureDataLake.ODataQuery.ExprStringEquals("submitter",this.FilterSubmitter));
            }

            if (this.FilterSubmittedAfter.HasValue)
            {
                q.Items.Add(new AzureDataLake.ODataQuery.ExprDateTimeAfter("submittedafter", this.FilterSubmittedAfter.Value));
            }

            if (!string.IsNullOrEmpty(this.FilterName))
            {
                q.Items.Add(new AzureDataLake.ODataQuery.ExprStringEquals("name", this.FilterName));
            }
            
            if (this.FilterState != null && this.FilterState.Length > 0)
            {
                var expr_and = new AzureDataLake.ODataQuery.ExprOr();
                q.Items.Add(expr_and);
                foreach (var state in this.FilterState)
                {
                    expr_and.Items.Add( new AzureDataLake.ODataQuery.ExprStringEquals("state", state.ToString()));
                }
            }
            
            if (this.FilterResult != null && this.FilterResult.Length > 0)
            {
                var expr_and = new AzureDataLake.ODataQuery.ExprOr();
                q.Items.Add(expr_and);
                foreach (var result in this.FilterResult)
                {
                    expr_and.Items.Add(new AzureDataLake.ODataQuery.ExprStringEquals("result", result.ToString()));
                }

            }

            var filterString = q.ToExprString();
            return filterString;
        }
    }
}