using System;
using System.Collections.Generic;
using System.Linq;
using AzureDataLake.Authentication;
using AzureDataLake.ODataQuery;
using Microsoft.Azure.Management.DataLake.Analytics.Models;
using ADL= Microsoft.Azure.Management.DataLake;
namespace AzureDataLake.Analytics
{
    public class JobListFilter
    {
        public bool SubmitterToCurrentUser;
        public FilterPropertyString Name;
        public FilterPropertyString Submitter;
        public FilterPropertyDateTime SubmitTime;
        public FilterPropertyDateTime StartTime;
        public FilterPropertyDateTime EndTime;
        public FilterPropertyInteger DegreeOfParallelism;
        public FilterPropertyInteger Priority;
        public FilterPropertyEnum<ADL.Analytics.Models.JobState> State;
        public FilterPropertyEnum<ADL.Analytics.Models.JobResult> Result;

        public JobListFilter()
        {
            this.Submitter = new FilterPropertyString("submitter");
            this.Name = new FilterPropertyString("name");
            this.SubmitTime = new FilterPropertyDateTime("submitTime");
            this.StartTime = new FilterPropertyDateTime("startTime");
            this.EndTime = new FilterPropertyDateTime("endTime");
            this.DegreeOfParallelism = new FilterPropertyInteger("degreeOfParallelism");
            this.Priority = new FilterPropertyInteger("priority");
            this.State = new FilterPropertyEnum<JobState>("state");
            this.Result = new FilterPropertyEnum<JobResult>("result");

        }

        public string CreateFilterString(AuthenticatedSession auth_session)
        {
            var q = new AzureDataLake.ODataQuery.ExprLogicalAnd();
            var col_submitter = new ExprColumn("submitter");

            if (this.DegreeOfParallelism != null)
            {
                var expr = this.DegreeOfParallelism.ToExpr();
                q.Add(expr);
            }


            if (this.Submitter != null)
            {
                var expr = this.Submitter.ToExpr();
                q.Add(expr);
            }

            if (this.Priority != null)
            {
                var expr = this.Priority.ToExpr();
                q.Add(expr);
            }

            if (this.Name != null)
            {
                var expr = this.Name.ToExpr();
                q.Add(expr);
            }

            if (this.SubmitTime != null)
            {
                var expr = this.SubmitTime.ToExpr();
                q.Add(expr);
            }

            if (this.State != null)
            {
                q.Add(this.State.ToExpr());
            }

            if (this.Result != null)
            {
                q.Add(this.Result.ToExpr());
            }

            if (this.SubmitterToCurrentUser)
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

    public class GetJobListOptions
    {
        public int Top=0; // 300 is the ADLA limit
        public JobOrderByField OrderByField;
        public JobOrderByDirection OrderByDirection;
        public JobListFilter Filter;

        public GetJobListOptions()
        {
            this.Filter = new JobListFilter();

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

    }
}