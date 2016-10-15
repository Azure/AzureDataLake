namespace AzureDataLake.ODataQuery
{
    public class ExprLiteralString : Expr
    {
        public string Content;

        public ExprLiteralString(string content)
        {
            this.Content = content;
        }

        public override void ToExprString(ExpressionWriter writer)
        {
            string s = string.Format("'{0}'", this.Content);
            writer.Append(s);
        }
    }
}