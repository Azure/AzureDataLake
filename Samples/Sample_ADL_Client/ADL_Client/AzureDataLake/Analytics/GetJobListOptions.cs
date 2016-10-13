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
                    expr1.Add(expr2);
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

    public class FilterPropertyDateTime
    {
        private string colname;
        private ODataQuery.ExprColumn expr_col;
        private System.DateTime? before;
        private System.DateTime? after;

        public FilterPropertyDateTime(string colname)
        {
            this.colname = colname;
            this.expr_col = new ExprColumn(colname);
        }

        public void Before(System.DateTime dt)
        {
            this.before = dt;
        }

        public void After(System.DateTime dt)
        {
            this.after = dt;
        }

        public ODataQuery.Expr ToExpr()
        {
            if (!(this.before.HasValue || this.after.HasValue))
            {
                return null;
            }

            var expr1 = new ODataQuery.ExprLogicalAnd();

            if (this.before.HasValue)
            {
                var expr2 = new ODataQuery.ExprCompareDateTime(this.expr_col, new ODataQuery.ExprLiteralDateTime(this.before.Value), NumericComparisonOperator.LesserThan);
                expr1.Add(expr2);
            }

            if (this.after.HasValue)
            {
                var expr2 = new ODataQuery.ExprCompareDateTime(this.expr_col, new ODataQuery.ExprLiteralDateTime(this.after.Value), NumericComparisonOperator.GreaterThan);
                expr1.Add(expr2);
            }

            return expr1;
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
        public FilterPropertyDateTime FilterSubmitTime;
        public FilterPropertyDateTime FilterStartTime;
        public FilterPropertyDateTime FilterEndTime;

        public int? FilterDegreeOfParallelism;
        public List<ADL.Analytics.Models.JobState> FilterState;
        public List<ADL.Analytics.Models.JobResult> FilterResult;

        public GetJobListOptions()
        {
            this.FilterSubmitter = new FilterPropertyString("submitter");
            this.FilterName = new FilterPropertyString("name");
            this.FilterSubmitTime = new FilterPropertyDateTime("submitTime");
            this.FilterStartTime = new FilterPropertyDateTime("startTime");
            this.FilterEndTime = new FilterPropertyDateTime("endTime");
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
            var col_submitter = new ExprColumn("submitter");
            var col_state = new ExprColumn("state");
            var col_result = new ExprColumn("result");
            var col_dop = new ExprColumn("degreeOfParallelism");

            if (this.FilterDegreeOfParallelism.HasValue)
            {
                var exprIntLiteral = new ExprLiteralInt(this.FilterDegreeOfParallelism.Value);
                q.Add(new AzureDataLake.ODataQuery.ExprCompareNumeric(col_dop, exprIntLiteral, ODataQuery.NumericComparisonOperator.Equals));
            }

            if (this.FilterSubmitter != null)
            {
                var expr = this.FilterSubmitter.ToExpr();
                if (expr != null)
                {
                    q.Add(expr);
                }
            }

            if (this.FilterName != null)
            {
                var expr = this.FilterName.ToExpr();
                if (expr != null)
                {
                    q.Add(expr);
                }
            }

            if (this.FilterSubmitTime != null)
            {
                var expr = this.FilterSubmitTime.ToExpr();
                if (expr != null)
                {
                    q.Add(expr);
                }
            }


            if (this.FilterState != null && this.FilterState.Count > 0)
            {
                var expr_and = new AzureDataLake.ODataQuery.ExprLogicalOr();
                q.Add(expr_and);
                foreach (var state in this.FilterState)
                {
                    var exprStringLiteral = new ExprLiteralString(state.ToString());
                    expr_and.Add( new AzureDataLake.ODataQuery.ExprCompareString(col_state, exprStringLiteral, StringCompareOperator.Equals));
                }
            }
            
            if (this.FilterResult != null && this.FilterResult.Count > 0)
            {
                var expr_and = new AzureDataLake.ODataQuery.ExprLogicalOr();
                q.Add(expr_and);
                foreach (var result in this.FilterResult)
                {
                    expr_and.Add(new AzureDataLake.ODataQuery.ExprCompareString(col_result, new ExprLiteralString(result.ToString()), StringCompareOperator.Equals));
                }

            }

            if (this.FilterSubmitterToCurrentUser)
            {
                var exprStringLiteral = new ExprLiteralString(auth_session.Token.DisplayableId);
                q.Add(new AzureDataLake.ODataQuery.ExprCompareString(col_submitter, exprStringLiteral, StringCompareOperator.Equals));
            }

            var sb = new AzureDataLake.ODataQuery.ExBuilder();
            sb.Append(q);
            string fs = sb.ToString();
            Console.WriteLine("DEBUG: FILTER {0}", fs);
            return fs;
        }
    }
}