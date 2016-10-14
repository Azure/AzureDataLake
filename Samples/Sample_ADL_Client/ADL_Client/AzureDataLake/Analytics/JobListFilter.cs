using System;
using Microsoft.Azure.Management.DataLake.Analytics.Models;

namespace AzureDataLake.Analytics
{
    public class JobListFilter
    {
        public bool SubmitterToCurrentUser;
        public PropertyFilterString Name;
        public PropertyFilterString Submitter;
        public PropertyFilterDateTime SubmitTime;
        public PropertyFilterDateTime StartTime;
        public PropertyFilterDateTime EndTime;
        public PropertyFilterInteger DegreeOfParallelism;
        public PropertyFilterInteger Priority;
        public PropertyFilterEnum<JobState> State;
        public PropertyFilterEnum<JobResult> Result;

        public JobListFilter()
        {
            this.Submitter = new PropertyFilterString("submitter");
            this.Name = new PropertyFilterString("name");
            this.SubmitTime = new PropertyFilterDateTime("submitTime");
            this.StartTime = new PropertyFilterDateTime("startTime");
            this.EndTime = new PropertyFilterDateTime("endTime");
            this.DegreeOfParallelism = new PropertyFilterInteger("degreeOfParallelism");
            this.Priority = new PropertyFilterInteger("priority");
            this.State = new PropertyFilterEnum<JobState>("state");
            this.Result = new PropertyFilterEnum<JobResult>("result");

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