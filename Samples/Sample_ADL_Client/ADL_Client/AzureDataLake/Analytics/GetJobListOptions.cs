using System;
using System.Collections.Generic;
using System.Linq;
using AzureDataLake.Authentication;
using AzureDataLake.ODataQuery;
using ADL= Microsoft.Azure.Management.DataLake;
namespace AzureDataLake.Analytics
{
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
        public FilterPropertyInteger FilterDegreeOfParallelism;
        public FilterPropertyInteger FilterPriority;

        public List<ADL.Analytics.Models.JobState> FilterState;
        public List<ADL.Analytics.Models.JobResult> FilterResult;

        public GetJobListOptions()
        {
            this.FilterSubmitter = new FilterPropertyString("submitter");
            this.FilterName = new FilterPropertyString("name");
            this.FilterSubmitTime = new FilterPropertyDateTime("submitTime");
            this.FilterStartTime = new FilterPropertyDateTime("startTime");
            this.FilterEndTime = new FilterPropertyDateTime("endTime");
            this.FilterDegreeOfParallelism = new FilterPropertyInteger("degreeOfParallelism");
            this.FilterPriority = new FilterPropertyInteger("priority");
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

            if (this.FilterDegreeOfParallelism!= null)
            {
                var expr = this.FilterDegreeOfParallelism.ToExpr();
                if (expr != null)
                {
                    q.Add(expr);
                }
            }


            if (this.FilterSubmitter != null)
            {
                var expr = this.FilterSubmitter.ToExpr();
                if (expr != null)
                {
                    q.Add(expr);
                }
            }

            if (this.FilterPriority != null)
            {
                var expr = this.FilterPriority.ToExpr();
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
                    expr_and.Add( new AzureDataLake.ODataQuery.ExprCompareString(col_state, exprStringLiteral, ComparisonString.Equals));
                }
            }
            
            if (this.FilterResult != null && this.FilterResult.Count > 0)
            {
                var expr_and = new AzureDataLake.ODataQuery.ExprLogicalOr();
                q.Add(expr_and);
                foreach (var result in this.FilterResult)
                {
                    expr_and.Add(new AzureDataLake.ODataQuery.ExprCompareString(col_result, new ExprLiteralString(result.ToString()), ComparisonString.Equals));
                }

            }

            if (this.FilterSubmitterToCurrentUser)
            {
                var exprStringLiteral = new ExprLiteralString(auth_session.Token.DisplayableId);
                q.Add(new AzureDataLake.ODataQuery.ExprCompareString(col_submitter, exprStringLiteral, ComparisonString.Equals));
            }

            var sb = new AzureDataLake.ODataQuery.ExBuilder();
            sb.Append(q);
            string fs = sb.ToString();
            Console.WriteLine("DEBUG: FILTER {0}", fs);
            return fs;
        }
    }
}