using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Linq.Dynamic;

namespace NaturalQuery.QueryTreeSyntax
{
    public class SelectQueryNode : BinaryQueryNode
    {
        #region Constructors

        public SelectQueryNode(QueryNode selector, QueryNode dataSource)
            : base(selector, dataSource)
        {
        }

        #endregion

        #region Exposed Methods

        internal override object Execute()
        {
            return DynamicQueryable.Select((IQueryable)Right.Execute(), Left.Execute().ToString());
        }

        #endregion
    }
}
