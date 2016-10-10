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
                string expr = QueryEqualsString("submitter",this.FilterSubmitter);
                filter.Add(expr);
            }

            // due to issue: https://github.com/Azure/autorest/issues/975,
            // date time offsets must be explicitly escaped before being passed to the filter
            if (this.FilterSubmittedAfter.HasValue)
            {
                string datestring = this.FilterSubmittedAfter.Value.ToString("O");
                var escaped_datestring = System.Uri.EscapeDataString(datestring);
                var expr = string.Format("submitTime ge datetimeoffset'{0}'", escaped_datestring);
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
                states = states.Select(s=>QueryEqualsString("state",s));
                string expr = QueryParens( QueryOR(states) );
                filter.Add(expr);
            }
            
            if (this.FilterResult != null && this.FilterResult.Length > 0)
            {
                var results = this.FilterResult.Select(s => s.ToString());
                results = results.Select(s => QueryEqualsString("result", s));
                var expr = QueryParens( QueryOR(results) );
                filter.Add(expr);
            }

            var filterString = QueryAND(filter);
            return filterString;
        }

        public string QueryEqualsString(string prop, string s)
        {
            return string.Format("{0} eq '{1}'", prop, s);
        }

        public string QueryParens(string s)
        {
            return "(" + s + ")";
        }

        public string QueryAND(IEnumerable<string> items)
        {
            return string.Join(" and ", items);
        }

        public string QueryOR(IEnumerable<string> items)
        {
            return string.Join(" or ", items);
        }
    }
}