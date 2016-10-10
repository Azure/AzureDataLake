using System.Collections.Generic;
using System.Linq;

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
            var filter = new List<string>();
            if (!string.IsNullOrEmpty(this.FilterSubmitter))
            {
                string expr = ExprStringEquals("submitter",this.FilterSubmitter);
                filter.Add(expr);
            }

            if (this.FilterSubmittedAfter.HasValue)
            {
                string expr = ExprDateTimeGreaterThan("name", this.FilterSubmittedAfter.Value);
                filter.Add(expr);
            }

            if (!string.IsNullOrEmpty(this.FilterName))
            {
                string expr = string.Format("name eq '{0}'", this.FilterName);
                filter.Add(expr);
            }
            
            if (this.FilterState != null && this.FilterState.Length > 0)
            {
                var states = this.FilterState.Select(state => state.ToString());
                states = states.Select(s=>ExprStringEquals("state",s));
                string expr = ExprParens( ExprOr(states) );
                filter.Add(expr);
            }
            
            if (this.FilterResult != null && this.FilterResult.Length > 0)
            {
                var results = this.FilterResult.Select(s => s.ToString());
                results = results.Select(s => ExprStringEquals("result", s));
                var expr = ExprParens( ExprOr(results) );
                filter.Add(expr);
            }

            var filterString = ExprAnd(filter);
            return filterString;
        }

        public string ExprStringEquals(string prop, string value)
        {
            return string.Format("{0} eq '{1}'", prop, value);
        }

        public string ExprParens(string s)
        {
            return "(" + s + ")";
        }

        public string ExprAnd(IEnumerable<string> items)
        {
            return string.Join(" and ", items);
        }

        public string ExprOr(IEnumerable<string> items)
        {
            return string.Join(" or ", items);
        }

        public string ExprDateTimeGreaterThan(string name, System.DateTime value)
        {

            string datestring = value.ToString("O");

            // due to issue: https://github.com/Azure/autorest/issues/975,
            // date time offsets must be explicitly escaped before being passed to the filter

            var escaped_datestring = System.Uri.EscapeDataString(datestring);

            var expr = string.Format("{0} ge datetimeoffset'{1}'", name, escaped_datestring);
            return expr;
        }
    }
}