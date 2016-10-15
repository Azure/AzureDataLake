namespace AzureDataLake.ODataQuery
{
    public class ExprRaw : Expr
    {
        public string Item;
        public ExprRaw(string s)
        {
            this.Item = s;
        }

        public override void ToExprString(ExpressionWriter writer)
        {
            writer.Append(this.Item);
        }
    }
}