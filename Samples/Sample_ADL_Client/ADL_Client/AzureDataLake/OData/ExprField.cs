namespace AzureDataLake.ODataQuery
{
    public class ExprField : Expr
    {
        public string Name;

        public ExprField(string name)
        {
            this.Name = name;
        }

        public override void ToExprString(ExpressionWriter writer)
        {
            writer.Append(this.Name);
        }
    }
}