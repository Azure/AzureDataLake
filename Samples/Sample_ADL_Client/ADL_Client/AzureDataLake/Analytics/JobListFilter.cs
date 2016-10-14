using System;
using Microsoft.Azure.Management.DataLake.Analytics.Models;

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
        public FilterPropertyEnum<JobState> State;
        public FilterPropertyEnum<JobResult> Result;

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

        public string CreateFilterString(Authentication.AuthenticatedSession auth_session)
        {
            var q = new AzureDataLake.ODataQuery.ExprLogicalAnd();
            var col_submitter = new ODataQuery.ExprField("submitter");

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
                var exprStringLiteral = new ODataQuery.ExprLiteralString(auth_session.Token.DisplayableId);
                q.Add(new AzureDataLake.ODataQuery.ExprCompareString(col_submitter, exprStringLiteral, ODataQuery.ComparisonString.Equals));
            }

            var sb = new AzureDataLake.ODataQuery.ExBuilder();
            sb.Append(q);
            string fs = sb.ToString();
            Console.WriteLine("DEBUG: FILTER {0}", fs);
            return fs;
        }

    }
}