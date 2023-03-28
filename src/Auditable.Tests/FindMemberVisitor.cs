using System.Linq.Expressions;
using System.Text;

namespace Auditable.Tests
{
    internal class FindMemberVisitor : ExpressionVisitor
    {
        private readonly string _alias;

        private readonly StringBuilder _memberName = new();
        private bool _isBody = false;

        private FindMemberVisitor()
        {
        }

        public static string GetMember(Expression linqExpression)
        {
            var visitor = new FindMemberVisitor();
            visitor.Visit(linqExpression);
            return visitor.ToString();
        }

        public override string ToString()
        {
            return _memberName.ToString();
        }


        protected override Expression VisitMember(MemberExpression expression)
        {
            Visit(expression.Expression);
            if (_memberName.Length > 0) _memberName.Append(".");
            _memberName.AppendFormat(expression.Member.Name);

            return expression;
        }
    }
}