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

        public FilterPropertyString FilterName;
        public FilterPropertyString FilterSubmitter;
        public FilterPropertyDateTime FilterSubmitTime;
        public FilterPropertyDateTime FilterStartTime;
        public FilterPropertyDateTime FilterEndTime;
        public FilterPropertyInteger FilterDegreeOfParallelism;
        public FilterPropertyInteger FilterPriority;
        public FilterPropertyEnum<ADL.Analytics.Models.JobState> FilterState;
        public FilterPropertyEnum<ADL.Analytics.Models.JobResult> FilterResult;

        public GetJobListOptions()
        {
            this.FilterSubmitter = new FilterPropertyString("submitter");
            this.FilterName = new FilterPropertyString("name");
            this.FilterSubmitTime = new FilterPropertyDateTime("submitTime");
            this.FilterStartTime = new FilterPropertyDateTime("startTime");
            this.FilterEndTime = new FilterPropertyDateTime("endTime");
            this.FilterDegreeOfParallelism = new FilterPropertyInteger("degreeOfParallelism");
            this.FilterPriority = new FilterPropertyInteger("priority");
            this.FilterState = new FilterPropertyEnum<JobState>("state");
            this.FilterResult = new FilterPropertyEnum<JobResult>("result");
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

            if (this.FilterDegreeOfParallelism!= null)
            {
                var expr = this.FilterDegreeOfParallelism.ToExpr();
                q.Add(expr);
            }


            if (this.FilterSubmitter != null)
            {
                var expr = this.FilterSubmitter.ToExpr();
                q.Add(expr);
            }

            if (this.FilterPriority != null)
            {
                var expr = this.FilterPriority.ToExpr();
                q.Add(expr);
            }

            if (this.FilterName != null)
            {
                var expr = this.FilterName.ToExpr();
                q.Add(expr);
            }

            if (this.FilterSubmitTime != null)
            {
                var expr = this.FilterSubmitTime.ToExpr();
                q.Add(expr);
            }

            if (this.FilterState != null)
            {
                q.Add(this.FilterState.ToExpr());
            }

            if (this.FilterResult != null)
            {
                q.Add(this.FilterResult.ToExpr());
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