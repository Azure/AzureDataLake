namespace AzureDataLake.ODataQuery
{
    public class ExprField : Expr
    {
        public string Name;

        public ExprField(string name)
        {
            this.Name = name;
        }

        public override void ToExprString(ExBuilder sb)
        {
            sb.Append(this.Name);
        }
    }
}