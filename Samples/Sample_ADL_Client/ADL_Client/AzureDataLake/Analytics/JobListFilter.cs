using System;
using AzureDataLake.Authentication;
using AzureDataLake.ODataQuery;
using Microsoft.Azure.Management.DataLake.Analytics.Models;

namespace AzureDataLake.Analytics
{
    public class JobListFilter
    {
        public bool SubmitterIsCurrentUser;
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

        public string ToFilterString(Authentication.AuthenticatedSession auth_session)
        {
            var expr_and = ToExpression(auth_session);

            var sb = new AzureDataLake.ODataQuery.ExpressionWriter();
            sb.Append(expr_and);
            string fs = sb.ToString();
            Console.WriteLine("DEBUG: FILTER {0}", fs);
            return fs;
        }

        private Expr ToExpression(AuthenticatedSession auth_session)
        {
            var expr_and = new AzureDataLake.ODataQuery.ExprLogicalAnd();
            var col_submitter = new ODataQuery.ExprField("submitter");

            if (this.DegreeOfParallelism != null)
            {
                var expr = this.DegreeOfParallelism.ToExpression();
                expr_and.Add(expr);
            }


            if (this.Submitter != null)
            {
                var expr = this.Submitter.ToExpression();
                expr_and.Add(expr);
            }

            if (this.Priority != null)
            {
                var expr = this.Priority.ToExpression();
                expr_and.Add(expr);
            }

            if (this.Name != null)
            {
                var expr = this.Name.ToExpression();
                expr_and.Add(expr);
            }

            if (this.SubmitTime != null)
            {
                var expr = this.SubmitTime.ToExpression();
                expr_and.Add(expr);
            }

            if (this.State != null)
            {
                expr_and.Add(this.State.ToExpression());
            }

            if (this.Result != null)
            {
                expr_and.Add(this.Result.ToExpression());
            }

            if (this.SubmitterIsCurrentUser)
            {
                var exprStringLiteral = new ODataQuery.ExprLiteralString(auth_session.Token.DisplayableId);
                expr_and.Add(new AzureDataLake.ODataQuery.ExprCompareString(col_submitter, exprStringLiteral,
                    ODataQuery.ComparisonString.Equals));
            }
            return expr_and;
        }
    }
}