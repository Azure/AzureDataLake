using System;
using System.Collections.Generic;
using System.Linq;
using AzureDataLake.ODataQuery;

namespace AzureDataLake.Analytics
{
    public class GetJobListOptions
    {
        public int Top=0; // 300 is the ADLA limit
        public JobOrderByField OrderByField;
        public JobOrderByDirection OrderByDirection;

        public string FilterSubmitter;
        public string FilterSubmitterContains;
        public string FilterName;
        public string FilterNameContains;
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
                q.Items.Add( new AzureDataLake.ODataQuery.ExprStringComparison("submitter",this.FilterSubmitter, StringCompareOps.Equals));
            }

            if (!string.IsNullOrEmpty(this.FilterSubmitterContains))
            {
                q.Items.Add(new AzureDataLake.ODataQuery.ExprStringComparison("submitter", this.FilterSubmitterContains, StringCompareOps.Contains));
            }

            if (!string.IsNullOrEmpty(this.FilterNameContains))
            {
                q.Items.Add(new AzureDataLake.ODataQuery.ExprStringComparison("name", this.FilterNameContains, StringCompareOps.Contains));
            }

            if (this.FilterSubmittedAfter.HasValue)
            {
                q.Items.Add(new AzureDataLake.ODataQuery.ExprDateTimeComparison("submittedafter", this.FilterSubmittedAfter.Value, ODataQuery.CompareOps.GreaterThanOrEquals));
            }

            if (!string.IsNullOrEmpty(this.FilterName))
            {
                q.Items.Add(new AzureDataLake.ODataQuery.ExprStringComparison("name", this.FilterName, StringCompareOps.Equals));
            }
            
            if (this.FilterState != null && this.FilterState.Length > 0)
            {
                var expr_and = new AzureDataLake.ODataQuery.ExprOr();
                q.Items.Add(expr_and);
                foreach (var state in this.FilterState)
                {
                    expr_and.Items.Add( new AzureDataLake.ODataQuery.ExprStringComparison("state", state.ToString(), StringCompareOps.Equals));
                }
            }
            
            if (this.FilterResult != null && this.FilterResult.Length > 0)
            {
                var expr_and = new AzureDataLake.ODataQuery.ExprOr();
                q.Items.Add(expr_and);
                foreach (var result in this.FilterResult)
                {
                    expr_and.Items.Add(new AzureDataLake.ODataQuery.ExprStringComparison("result", result.ToString(), StringCompareOps.Equals));
                }

            }

            var sb = new System.Text.StringBuilder();
            q.ToExprString(sb);
            string fs = sb.ToString();
            Console.WriteLine(fs);
            return fs;
        }
    }
}