namespace AzureDataLake.ODataQuery
{
    public class ExprRaw : Expr
    {
        public string Item;
        public ExprRaw(string s)
        {
            this.Item = s;
        }

        public override void ToExprString(ExpressionWriter sb)
        {
            sb.Append(this.Item);
        }
    }
}