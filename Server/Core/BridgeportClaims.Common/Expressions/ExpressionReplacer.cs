using System.Linq.Expressions;

namespace BridgeportClaims.Common.Expressions
{
    /// <summary>
    /// Replaces references to one specific instance of an expression node with another node
    /// </summary>
    public class ExpressionReplacer : ExpressionVisitor
    {
        private readonly Expression _searchFor;
        private readonly Expression _replaceWith;

        private ExpressionReplacer(Expression searchFor, Expression replaceWith)
        {
            _searchFor = searchFor;
            _replaceWith = replaceWith;
        }

        public static Expression Replace(Expression expression, Expression searchFor, Expression replaceWith)
        {
            return new ExpressionReplacer(searchFor, replaceWith).Visit(expression);
        }

        public static Expression ReplaceAll(Expression expression, Expression[] searchFor, Expression[] replaceWith)
        {
            for (int i = 0, n = searchFor.Length; i < n; i++)
            {
                expression = Replace(expression, searchFor[i], replaceWith[i]);
            }
            return expression;
        }

        public override Expression Visit(Expression exp)
        {
            if (exp == _searchFor)
            {
                return _replaceWith;
            }
            return base.Visit(exp);
        }
    }
}