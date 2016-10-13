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

    public class ExprDateLiteral : Expr
    {
        public System.DateTime Content;

        public ExprDateLiteral(System.DateTime content)
        {
            this.Content = content;
        }

        public override void ToExprString(ExBuilder sb)
        {
            // due to issue: https://github.com/Azure/autorest/issues/975,
            // date time offsets must be explicitly escaped before being passed to the filter

            sb.Append("datetimeoffset");
            string datestring = this.Content.ToString("O");
            var escaped_datestring = System.Uri.EscapeDataString(datestring);
            sb.Append(escaped_datestring);
        }
    }
}