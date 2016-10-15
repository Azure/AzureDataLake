namespace AzureDataLake.ODataQuery
{
    public class ExprField : Expr
    {
        public string Name;

        public ExprField(string name)
        {
            this.Name = name;
        }

        public override void ToExprString(ExpressionWriter sb)
        {
            sb.Append(this.Name);
        }
    }
}