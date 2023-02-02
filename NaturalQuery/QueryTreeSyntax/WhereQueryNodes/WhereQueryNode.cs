using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NaturalQuery.QueryTreeSyntax
{
    public class WhereQueryNode : BinaryQueryNode
    {
        #region Constructors

        public WhereQueryNode(QueryNode dataSource, QueryNode condition)
            : base(dataSource, condition)
        {
        }

        #endregion

        #region Exposed Methods

        internal override object Execute()
        {
            return DynamicQueryable.Where(
                    (IQueryable<dynamic>)Left.Execute(),
                    Right.Execute().ToString()
            );
        }

        #endregion
    }
}
