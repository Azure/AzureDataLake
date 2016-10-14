using System;
using Microsoft.Azure.Management.DataLake.Analytics.Models;

namespace AzureDataLake.Analytics
{
    public class JobListFilter
    {
        public bool SubmitterToCurrentUser;
        public ODataQuery.PropertyFilterString Name;
        public ODataQuery.PropertyFilterString Submitter;
        public ODataQuery.PropertyFilterDateTime SubmitTime;
        public ODataQuery.PropertyFilterDateTime StartTime;
        public ODataQuery.PropertyFilterDateTime EndTime;
        public ODataQuery.PropertyFilterInteger DegreeOfParallelism;
        public ODataQuery.PropertyFilterInteger Priority;
        public ODataQuery.PropertyFilterEnum<JobState> State;
        public ODataQuery.PropertyFilterEnum<JobResult> Result;

        public JobListFilter()
        {
            this.Submitter = new ODataQuery.PropertyFilterString("submitter");
            this.Name = new ODataQuery.PropertyFilterString("name");
            this.SubmitTime = new ODataQuery.PropertyFilterDateTime("submitTime");
            this.StartTime = new ODataQuery.PropertyFilterDateTime("startTime");
            this.EndTime = new ODataQuery.PropertyFilterDateTime("endTime");
            this.DegreeOfParallelism = new ODataQuery.PropertyFilterInteger("degreeOfParallelism");
            this.Priority = new ODataQuery.PropertyFilterInteger("priority");
            this.State = new ODataQuery.PropertyFilterEnum<JobState>("state");
            this.Result = new ODataQuery.PropertyFilterEnum<JobResult>("result");

        }

        public string CreateFilterString(Authentication.AuthenticatedSession auth_session)
        {
            var expr_and = new AzureDataLake.ODataQuery.ExprLogicalAnd();
            var col_submitter = new ODataQuery.ExprField("submitter");

            if (this.DegreeOfParallelism != null)
            {
                var expr = this.DegreeOfParallelism.ToExpr();
                expr_and.Add(expr);
            }


            if (this.Submitter != null)
            {
                var expr = this.Submitter.ToExpr();
                expr_and.Add(expr);
            }

            if (this.Priority != null)
            {
                var expr = this.Priority.ToExpr();
                expr_and.Add(expr);
            }

            if (this.Name != null)
            {
                var expr = this.Name.ToExpr();
                expr_and.Add(expr);
            }

            if (this.SubmitTime != null)
            {
                var expr = this.SubmitTime.ToExpr();
                expr_and.Add(expr);
            }

            if (this.State != null)
            {
                expr_and.Add(this.State.ToExpr());
            }

            if (this.Result != null)
            {
                expr_and.Add(this.Result.ToExpr());
            }

            if (this.SubmitterToCurrentUser)
            {
                var exprStringLiteral = new ODataQuery.ExprLiteralString(auth_session.Token.DisplayableId);
                expr_and.Add(new AzureDataLake.ODataQuery.ExprCompareString(col_submitter, exprStringLiteral, ODataQuery.ComparisonString.Equals));
            }

            var sb = new AzureDataLake.ODataQuery.ExBuilder();
            sb.Append(expr_and);
            string fs = sb.ToString();
            Console.WriteLine("DEBUG: FILTER {0}", fs);
            return fs;
        }

    }
}