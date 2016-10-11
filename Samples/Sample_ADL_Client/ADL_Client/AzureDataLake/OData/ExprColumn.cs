namespace AzureDataLake.ODataQuery
{
    public class ExprColumn : Expr
    {
        public string Name;

        public ExprColumn(string name)
        {
            this.Name = name;
        }

        public override void ToExprString(ExBuilder sb)
        {
            sb.Append(this.Name);
        }
    }
}