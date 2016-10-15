using System;

namespace AzureDataLake.ODataQuery
{
    public class ExprLogicalAnd : ExprList
    {

        public ExprLogicalAnd(params Expr[] items) :
            base(items)
        {
            this.AddRange(items);
        }

        public override void ToExprString(ExpressionWriter sb)
        {
            this.WriteItems(sb, " and ");
        }
    }
}