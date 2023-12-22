using System;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Expressions.Task3.E3SQueryProvider
{
    public class ExpressionToFtsRequestTranslator : ExpressionVisitor
    {
        readonly StringBuilder _resultStringBuilder;

        public ExpressionToFtsRequestTranslator()
        {
            _resultStringBuilder = new StringBuilder();
        }

        public string Translate(Expression exp)
        {
            Visit(exp);

            return _resultStringBuilder.ToString();
        }

        #region protected methods

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            if (node.Method.DeclaringType == typeof(Queryable)
                && node.Method.Name == "Where")
            {
                var predicate = node.Arguments[1];
                Visit(predicate);

                return node;
            }
            else if (node.Method.DeclaringType == typeof(string) && node.Method.Name == "Equals")
            {
                var predicate = node.Arguments[0];
                Visit(node.Object);
                _resultStringBuilder.Append("(");
                Visit(predicate);
                _resultStringBuilder.Append(")");

                return node;
            }else if (node.Method.DeclaringType == typeof(string) && node.Method.Name == "StartsWith")
            {
                var predicate = node.Arguments[0];
                Visit(node.Object);
                _resultStringBuilder.Append("(");
                Visit(predicate);
                _resultStringBuilder.Append("*");
                _resultStringBuilder.Append(")");

                return node;
            } 
            else if (node.Method.DeclaringType == typeof(string) &&( node.Method.Name == "EndsWith" || node.Method.Name == "Contains"))
            {
                var predicate = node.Arguments[0];
                Visit(node.Object);
                _resultStringBuilder.Append("(");
                _resultStringBuilder.Append("*");
                Visit(predicate);
                if(node.Method.Name == "Contains")
                {
                    _resultStringBuilder.Append("*");
                }
                _resultStringBuilder.Append(")");

                return node;
            } 
            return base.VisitMethodCall(node);
        }

        protected override Expression VisitBinary(BinaryExpression node)
        {
            switch (node.NodeType)
            {
                case ExpressionType.Equal: // should return Workstation:(EPRUIZHW006) constant should always right
                    if (node.Left.NodeType != ExpressionType.MemberAccess)
                    {
                        Visit(node.Right);
                        _resultStringBuilder.Append("(");
                        Visit(node.Left);
                        _resultStringBuilder.Append(")");
                        break;
                    }
                       

                    Visit(node.Left);
                    _resultStringBuilder.Append("(");
                    Visit(node.Right);
                    _resultStringBuilder.Append(")");
                    break;

                case ExpressionType.AndAlso: 
                    Visit(node.Left);
                    _resultStringBuilder.Append(" AND ");
                    Visit(node.Right);
                    break;

                default:
                    throw new NotSupportedException($"Operation '{node.NodeType}' is not supported");
            };

            return node;
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            _resultStringBuilder.Append(node.Member.Name).Append(":");

            return base.VisitMember(node);
        }

        protected override Expression VisitConstant(ConstantExpression node)
        {
            _resultStringBuilder.Append(node.Value);

            return node;
        }

        #endregion
    }
}
