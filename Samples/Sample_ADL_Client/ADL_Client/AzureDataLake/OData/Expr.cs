namespace AzureDataLake.ODataQuery
{
    public abstract class Expr
    {

        public abstract void ToExprString(ExpressionWriter sb);

        public void WriteBinaryOperation(ExpressionWriter sb, string op, Expr left, Expr right)
        {

            sb.Append("(");
            sb.Append(left);
            sb.Append(" ");
            sb.Append(op);
            sb.Append(" ");
            sb.Append(right);
            sb.Append(")");
        }
        public void WriteFunction3(ExpressionWriter sb, string name, Expr p0, Expr p1, Expr p2)
        {

            sb.Append(name);
            sb.Append("(");
            sb.Append(p0);
            sb.Append(",");
            sb.Append(p1);
            sb.Append(",");
            sb.Append(p2);
            sb.Append(")");
        }

        public void WriteFunction2(ExpressionWriter sb, string name, Expr p0, Expr p1)
        {

            sb.Append(name);
            sb.Append("(");
            sb.Append(p0);
            sb.Append(",");
            sb.Append(p1);
            sb.Append(")");
        }

        public void WriteFunction1(ExpressionWriter sb, string name, Expr p0)
        {

            sb.Append(name);
            sb.Append("(");
            sb.Append(p0);
            sb.Append(")");
        }
    }
}