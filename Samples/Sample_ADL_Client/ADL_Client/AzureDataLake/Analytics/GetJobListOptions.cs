using System;
using System.Collections.Generic;
using System.Linq;
using AzureDataLake.ODataQuery;
using Microsoft.Azure.Management.DataLake.Analytics.Models;
using ADL= Microsoft.Azure.Management.DataLake;
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
        public int? FilterDegreeOfParallelism;
        public List<ADL.Analytics.Models.JobState> FilterState;
        public List<ADL.Analytics.Models.JobResult> FilterResult;

        public GetJobListOptions()
        {
        }

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
            var col_name = new ExprColumn("name");
            var col_submitter = new ExprColumn("submitter");
            var col_submittedtime = new ExprColumn("submittedtime");
            var col_state = new ExprColumn("state");
            var col_result = new ExprColumn("result");
            var col_dop = new ExprColumn("degreeOfParallelism");

            if (this.FilterDegreeOfParallelism.HasValue)
            {
                q.Items.Add(new AzureDataLake.ODataQuery.IntegerComparison(col_dop, new ExprIntLiteral(this.FilterDegreeOfParallelism.Value), ODataQuery.NumericCompareOps.Equals));
            }

            if (!string.IsNullOrEmpty(this.FilterSubmitter))
            {
                q.Items.Add(new AzureDataLake.ODataQuery.ExprStringComparison( col_submitter, new ExprStringLiteral(this.FilterSubmitter), StringCompareOps.Equals));
            }

            if (!string.IsNullOrEmpty(this.FilterSubmitterContains))
            {
                q.Items.Add(new AzureDataLake.ODataQuery.ExprStringComparison(col_submitter, new ExprStringLiteral(this.FilterSubmitterContains), StringCompareOps.Contains));
            }

            if (!string.IsNullOrEmpty(this.FilterNameContains))
            {
                q.Items.Add(new AzureDataLake.ODataQuery.ExprStringComparison(col_submitter, new ExprStringLiteral(this.FilterNameContains), StringCompareOps.Contains));
            }

            if (this.FilterSubmittedAfter.HasValue)
            {
                var expr_dt = new AzureDataLake.ODataQuery.ExprDateLiteral(this.FilterSubmittedAfter.Value);
                q.Items.Add(new AzureDataLake.ODataQuery.ExprDateTimeComparison(col_submittedtime, expr_dt, ODataQuery.NumericCompareOps.GreaterThanOrEquals));
            }

            if (!string.IsNullOrEmpty(this.FilterName))
            {
                q.Items.Add(new AzureDataLake.ODataQuery.ExprStringComparison( col_name, new ExprStringLiteral(this.FilterName), StringCompareOps.Equals));
            }
            
            if (this.FilterState != null && this.FilterState.Count > 0)
            {
                var expr_and = new AzureDataLake.ODataQuery.ExprOr();
                q.Items.Add(expr_and);
                foreach (var state in this.FilterState)
                {
                    expr_and.Items.Add( new AzureDataLake.ODataQuery.ExprStringComparison(col_state, new ExprStringLiteral(state.ToString()), StringCompareOps.Equals));
                }
            }
            
            if (this.FilterResult != null && this.FilterResult.Count > 0)
            {
                var expr_and = new AzureDataLake.ODataQuery.ExprOr();
                q.Items.Add(expr_and);
                foreach (var result in this.FilterResult)
                {
                    expr_and.Items.Add(new AzureDataLake.ODataQuery.ExprStringComparison(col_result, new ExprStringLiteral(result.ToString()), StringCompareOps.Equals));
                }

            }

            var sb = new AzureDataLake.ODataQuery.ExBuilder();
            sb.Append(q);
            string fs = sb.ToString();
            Console.WriteLine(fs);
            return fs;
        }
    }
}