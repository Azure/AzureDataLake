namespace AzureDataLake.ODataQuery
{
    public class ExpressionWriter
    {
        private System.Text.StringBuilder sb;

        public ExpressionWriter()
        {
            this.sb = new System.Text.StringBuilder();    
        }

        public void Append(string s)
        {
            this.sb.Append(s);
        }

        public void Append(Expr expr)
        {
            expr.ToExprString(this);
        }

        public override string ToString()
        {
            return this.sb.ToString();
        }
    }
}