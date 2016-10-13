namespace AzureDataLake.ODataQuery
{
    public class ExprLiteralString : Expr
    {
        public string Content;

        public ExprLiteralString(string content)
        {
            this.Content = content;
        }

        public override void ToExprString(ExBuilder sb)
        {
            string s = string.Format("'{0}'", this.Content);
            sb.Append(s);
        }
    }
}