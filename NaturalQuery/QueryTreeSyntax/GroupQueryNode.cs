using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NaturalQuery.QueryTreeSyntax
{
    public class GroupQueryNode : BinaryQueryNode
    {
        #region Constructors

        public GroupQueryNode(QueryNode something, QueryNode bySomething)
            : base(something, bySomething)
        {
        }

        #endregion

        #region Exposed Methods

        internal override object Execute()
        {
            return DynamicQueryable.GroupBy(
                    (IQueryable<dynamic>)Left.Execute(),
                    string.Format(CultureInfo.InvariantCulture, "new({0})", Right.Execute()),
                    "it");
        }

        #endregion
    }
}
