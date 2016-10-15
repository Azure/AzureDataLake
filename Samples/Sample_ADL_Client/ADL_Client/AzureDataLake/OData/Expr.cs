namespace AzureDataLake.ODataQuery
{
    public abstract class Expr
    {

        public abstract void ToExprString(ExpressionWriter writer);

        public void WriteBinaryOperation(ExpressionWriter writer, string op, Expr left, Expr right)
        {

            writer.Append("(");
            writer.Append(left);
            writer.Append(" ");
            writer.Append(op);
            writer.Append(" ");
            writer.Append(right);
            writer.Append(")");
        }
        public void WriteFunction3(ExpressionWriter writer, string name, Expr p0, Expr p1, Expr p2)
        {

            writer.Append(name);
            writer.Append("(");
            writer.Append(p0);
            writer.Append(",");
            writer.Append(p1);
            writer.Append(",");
            writer.Append(p2);
            writer.Append(")");
        }

        public void WriteFunction2(ExpressionWriter writer, string name, Expr p0, Expr p1)
        {

            writer.Append(name);
            writer.Append("(");
            writer.Append(p0);
            writer.Append(",");
            writer.Append(p1);
            writer.Append(")");
        }

        public void WriteFunction1(ExpressionWriter writer, string name, Expr p0)
        {

            writer.Append(name);
            writer.Append("(");
            writer.Append(p0);
            writer.Append(")");
        }
    }
}