using System;
using System.Linq.Expressions;
using BridgeportClaims.Common.Extensions;

namespace BridgeportClaims.Common.Expressions
{
    public class ExpressionHelper
    {
        public static ConditionalExpression NullPropagation(Expression receiver, ParameterExpression accessParameter, Expression accessExpression)
        {
            var fullAccessExpression = ExpressionReplacer.Replace(accessExpression, accessParameter,
                Nullable.GetUnderlyingType(receiver.Type) == null ? receiver : Expression.Property(receiver, "Value"));

            var type = accessExpression.Type.Nullify();

            if (fullAccessExpression.Type != type)
                fullAccessExpression = Expression.Convert(fullAccessExpression, type);

            return Expression.Condition(Expression.Equal(receiver, Expression.Constant(null, receiver.Type)),
                Expression.Constant(null, type), fullAccessExpression);
        }
    }
}