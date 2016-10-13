using System;
using System.Collections.Generic;
using System.Linq;
using AzureDataLake.Authentication;
using AzureDataLake.ODataQuery;
using Microsoft.Azure.Management.DataLake.Analytics.Models;
using ADL= Microsoft.Azure.Management.DataLake;
namespace AzureDataLake.Analytics
{
    public class FilterPropertyString
    {
        private string colname;
        private ODataQuery.ExprColumn expr_col;
        private List<string> OneOfList;
        private string beginsswith_prefix;
        private string endswith_suffix;
        private string contains;

        public FilterPropertyString(string colname)
        {
            this.colname = colname;
            this.expr_col = new ExprColumn(colname);
        }

        public void OneOf(params string[] items)
        {
            this.OneOfList = new List<string>(items.Length);
            this.OneOfList.AddRange(items);    
        }

        public void BeginsWith(string text)
        {
            this.beginsswith_prefix = text;
        }

        public void EndsWith(string text)
        {
            this.endswith_suffix = text;
        }

        public void Contains(string text)
        {
            this.contains = text;
        }

        public ODataQuery.Expr ToExpr()
        {
            if (this.OneOfList != null && this.OneOfList.Count > 0)
            {
                var expr1 = new ODataQuery.ExprLogicalOr();
                foreach (var item in this.OneOfList)
                {
                    var expr2 = new ODataQuery.ExprCompareString(this.expr_col, new ODataQuery.ExprLiteralString(item), ODataQuery.StringCompareOperator.Equals );
                    expr1.Items.Add(expr2);
                }
                return expr1;
            }
            if (this.contains !=null)
            {
                var expr1 = new ODataQuery.ExprCompareString(this.expr_col, new ODataQuery.ExprLiteralString(this.contains), ODataQuery.StringCompareOperator.Contains);
                return expr1;
            }

            if (this.endswith_suffix != null)
            {
                var expr1 = new ODataQuery.ExprCompareString(this.expr_col, new ODataQuery.ExprLiteralString(this.endswith_suffix), ODataQuery.StringCompareOperator.EndsWith);
                return expr1;
            }

            if (this.beginsswith_prefix != null)
            {
                var expr1 = new ODataQuery.ExprCompareString(this.expr_col, new ODataQuery.ExprLiteralString(this.beginsswith_prefix), ODataQuery.StringCompareOperator.StartsWith);
                return expr1;
            }

            return null;
        }
    }

    public class GetJobListOptions
    {
        public int Top=0; // 300 is the ADLA limit
        public JobOrderByField OrderByField;
        public JobOrderByDirection OrderByDirection;

        public bool FilterSubmitterToCurrentUser;

        public FilterPropertyString FilterName;
        public FilterPropertyString FilterSubmitter;

        public System.DateTime? FilterSubmittedAfter;
        public System.DateTime? FilterSubmittedBefore;
        public int? FilterDegreeOfParallelism;
        public List<ADL.Analytics.Models.JobState> FilterState;
        public List<ADL.Analytics.Models.JobResult> FilterResult;

        public GetJobListOptions()
        {
            this.FilterSubmitter = new FilterPropertyString("submitter");
            this.FilterName = new FilterPropertyString("name");
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

            if (this.FilterSubmitter != null)
            {
                var expr = this.FilterSubmitter.ToExpr();
                if (expr != null)
                {
                    q.Items.Add(expr);
                }
            }

            if (this.FilterName != null)
            {
                var expr = this.FilterName.ToExpr();
                if (expr != null)
                {
                    q.Items.Add(expr);
                }
            }

            if (this.FilterSubmittedAfter.HasValue)
            {
                var expr_dt = new AzureDataLake.ODataQuery.ExprLiteralDateTime(this.FilterSubmittedAfter.Value);
                q.Items.Add(new AzureDataLake.ODataQuery.ExprCompareDateTime(col_submittedtime, expr_dt, ODataQuery.NumericComparisonOperator.GreaterThanOrEquals));
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
            Console.WriteLine("!!!!!!!!!!!!!!!!!!!!!!!!!");
            Console.WriteLine(fs);
            return fs;
        }
    }
}