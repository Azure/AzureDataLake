using System.Collections.Generic;
using System.Linq;

namespace AzureDataLake.Analytics
{
    public class GetJobListOptions
    {
        public int Top = 300; // 300 is the ADLA limit
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
                string expr = string.Format("submitter eq '{0}'", this.FilterSubmitter);
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
                var states = this.FilterState.Select(state => string.Format("state eq '{0}'", state));
                string expr = "(" + string.Join(" or ", states) + ")";
                filter.Add(expr);
            }
            
            if (this.FilterResult != null && this.FilterResult.Length > 0)
            {
                var results = this.FilterResult.Select(result => string.Format("result eq '{0}'", result));
                var expr = "(" +  string.Join(" or ", results) + ")";
                filter.Add(expr);
            }

            var filterString = string.Join(" and ", filter);
            return filterString;
        }
    }
}