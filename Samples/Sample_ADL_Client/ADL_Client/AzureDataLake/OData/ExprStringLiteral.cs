namespace AzureDataLake.ODataQuery
{
    public class ExprStringLiteral : Expr
    {
        public string Content;

        public ExprStringLiteral(string content)
        {
            this.Content = content;
        }

        public override void ToExprString(ExBuilder sb)
        {
            sb.AppendQuotedString(this.Content);
        }
    }
}