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
            string s = string.Format("'{0}'", this.Content);
            sb.Append(s);
        }
    }

    public class ExprIntLiteral : Expr
    {
        public int Content;

        public ExprIntLiteral(int content)
        {
            this.Content = content;
        }

        public override void ToExprString(ExBuilder sb)
        {
            sb.Append(this.Content.ToString());
        }
    }
}