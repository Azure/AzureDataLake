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

    }
}