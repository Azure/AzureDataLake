using System;

namespace AzureDataLake.ODataQuery
{
    public class ExprLogicalAnd : ExprLogicalList
    {

        public ExprLogicalAnd(params Expr[] expressions) :
            base(expressions)
        {
            this.AddRange(expressions);
        }

        public override void ToExprString(ExpressionWriter writer)
        {
            this.WriteItems(writer, " and ");
        }
    }
}