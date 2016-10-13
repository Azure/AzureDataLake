using System;
using System.Collections.Generic;
using System.Linq;
using AzureDataLake.Authentication;
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

        public bool FilterSubmitterToCurrentUser;
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

        public string CreateFilterString(AuthenticatedSession auth_session)
        {
            var q = new AzureDataLake.ODataQuery.ExprLogicalAnd();
            var col_name = new ExprColumn("name");
            var col_submitter = new ExprColumn("submitter");
            var col_submittedtime = new ExprColumn("submitTime");
            var col_state = new ExprColumn("state");
            var col_result = new ExprColumn("result");
            var col_dop = new ExprColumn("degreeOfParallelism");

            if (this.FilterDegreeOfParallelism.HasValue)
            {
                var exprIntLiteral = new ExprLiteralInt(this.FilterDegreeOfParallelism.Value);
                q.Items.Add(new AzureDataLake.ODataQuery.ExprCompareNumeric(col_dop, exprIntLiteral, ODataQuery.NumericComparisonOperator.Equals));
            }

            if (!string.IsNullOrEmpty(this.FilterSubmitter))
            {
                var exprStringLiteral = new ExprLiteralString(this.FilterSubmitter);
                q.Items.Add(new AzureDataLake.ODataQuery.ExprCompareString( col_submitter, exprStringLiteral, StringCompareOperator.Equals));
            }

            if (!string.IsNullOrEmpty(this.FilterSubmitterContains))
            {
                var exprStringLiteral = new ExprLiteralString(this.FilterSubmitterContains);
                q.Items.Add(new AzureDataLake.ODataQuery.ExprCompareString(col_submitter, exprStringLiteral, StringCompareOperator.Contains));
            }

            if (!string.IsNullOrEmpty(this.FilterNameContains))
            {
                var exprStringLiteral = new ExprLiteralString(this.FilterNameContains);
                q.Items.Add(new AzureDataLake.ODataQuery.ExprCompareString(col_submitter, exprStringLiteral, StringCompareOperator.Contains));
            }

            if (this.FilterSubmittedAfter.HasValue)
            {
                var expr_dt = new AzureDataLake.ODataQuery.ExprLiteralDateTime(this.FilterSubmittedAfter.Value);
                q.Items.Add(new AzureDataLake.ODataQuery.ExprCompareDateTime(col_submittedtime, expr_dt, ODataQuery.NumericComparisonOperator.GreaterThanOrEquals));
            }

            if (!string.IsNullOrEmpty(this.FilterName))
            {
                var exprStringLiteral = new ExprLiteralString(this.FilterName);
                q.Items.Add(new AzureDataLake.ODataQuery.ExprCompareString( col_name, exprStringLiteral, StringCompareOperator.Equals));
            }

            if (this.FilterSubmittedBefore.HasValue)
            {
                var exprDateLiteral = new ExprLiteralDateTime(this.FilterSubmittedBefore.Value);
                q.Items.Add(new AzureDataLake.ODataQuery.ExprCompareDateTime(col_submittedtime, exprDateLiteral, NumericComparisonOperator.LesserThan));
            }

            if (this.FilterState != null && this.FilterState.Count > 0)
            {
                var expr_and = new AzureDataLake.ODataQuery.ExprLogicalOr();
                q.Items.Add(expr_and);
                foreach (var state in this.FilterState)
                {
                    var exprStringLiteral = new ExprLiteralString(state.ToString());
                    expr_and.Items.Add( new AzureDataLake.ODataQuery.ExprCompareString(col_state, exprStringLiteral, StringCompareOperator.Equals));
                }
            }
            
            if (this.FilterResult != null && this.FilterResult.Count > 0)
            {
                var expr_and = new AzureDataLake.ODataQuery.ExprLogicalOr();
                q.Items.Add(expr_and);
                foreach (var result in this.FilterResult)
                {
                    expr_and.Items.Add(new AzureDataLake.ODataQuery.ExprCompareString(col_result, new ExprLiteralString(result.ToString()), StringCompareOperator.Equals));
                }

            }

            if (this.FilterSubmitterToCurrentUser)
            {
                var exprStringLiteral = new ExprLiteralString(auth_session.Token.DisplayableId);
                q.Items.Add(new AzureDataLake.ODataQuery.ExprCompareString(col_submitter, exprStringLiteral, StringCompareOperator.Equals));
            }

            var sb = new AzureDataLake.ODataQuery.ExBuilder();
            sb.Append(q);
            string fs = sb.ToString();
            Console.WriteLine(fs);
            return fs;
        }
    }
}