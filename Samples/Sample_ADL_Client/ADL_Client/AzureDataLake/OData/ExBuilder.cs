namespace AzureDataLake.ODataQuery
{
    public class ExBuilder
    {
        private System.Text.StringBuilder sb;

        public ExBuilder()
        {
            this.sb = new System.Text.StringBuilder();    
        }

        public void Append(string s)
        {
            this.sb.Append(s);
        }

        public void AppendQuotedString(string s)
        {
            this.sb.Append(string.Format("'{0}'",s));
        }

        public void Append(int s)
        {
            this.sb.Append(s);
        }

        public void Append(Expr e)
        {
            e.ToExprString(this);
        }

        public override string ToString()
        {
            return this.sb.ToString();
        }
    }
}