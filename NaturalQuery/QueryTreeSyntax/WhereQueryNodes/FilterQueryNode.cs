using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NaturalQuery.QueryTreeSyntax
{
    public class FilterQueryNode : QueryNode
    {
        #region Properties

        public string Expression { get; private set; }

        #endregion

        #region Constructors

        public FilterQueryNode(string expression)
            : base(expression.GetHashCode())
        {
            Expression = expression;
        }

        #endregion

        #region Exposed Methods

        internal override object Execute()
        {
            return Expression;
        }

        #endregion
    }
}
