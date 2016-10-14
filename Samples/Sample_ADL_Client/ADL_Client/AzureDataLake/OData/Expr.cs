namespace AzureDataLake.ODataQuery
{
    public abstract class Expr
    {

        public abstract void ToExprString(ExBuilder sb);

        public void WriteBinaryOperation(ExBuilder sb, string op, Expr left, Expr right)
        {

            sb.Append("(");
            sb.Append(left);
            sb.Append(" ");
            sb.Append(op);
            sb.Append(" ");
            sb.Append(right);
            sb.Append(")");
        }

        public void WriteFunction2(ExBuilder sb, string name, Expr p0, Expr p1)
        {

            sb.Append(name);
            sb.Append("(");
            sb.Append(p0);
            sb.Append(",");
            sb.Append(p1);
            sb.Append(")");
        }
    }
}