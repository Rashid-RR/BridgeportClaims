using System;
using System.Linq.Expressions;
using BridgeportClaims.Common.Extensions;

namespace BridgeportClaims.Common.Expressions
{
    public class NullPropagationExpression : Expression
    {
        public Expression Receiver { get; private set; }
        public ParameterExpression AccessParameter { get; private set; }
        public Expression AccessExpression { get; private set; }
        public Type type;

        public NullPropagationExpression(Expression receiver, ParameterExpression accessParameter, 
            Expression accessExpression)
        {
            Receiver = receiver;
            AccessParameter = accessParameter;
            AccessExpression = accessExpression;
            type = AccessExpression.Type.Nullify();
        }

        public override Type Type
        {
            get { return type; }
        }
    }
}