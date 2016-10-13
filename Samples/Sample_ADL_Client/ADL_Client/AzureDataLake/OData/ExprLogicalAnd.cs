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

        public override void ToExprString(ExBuilder sb)
        {
            this.WriteItems(sb, " and ");
        }
    }
}