using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Threading.Tasks;

namespace NaturalQuery.QueryTreeSyntax
{
    public enum OrderDirection
    {
        Ascending,
        Descending
    }

    public class OrderQueryNode : BinaryQueryNode
    {
        #region Properties

        public OrderDirection Direction { get; private set; }

        #endregion

        #region Constructors

        public OrderQueryNode(QueryNode dataSource, QueryNode by, OrderDirection direction)
            : base (dataSource, by)
        {
            Direction = direction;
        }

        #endregion

        #region Exposed Methods

        internal override object Execute()
        {
            return DynamicQueryable.OrderBy(
                        (Left.Execute() as IQueryable<dynamic>),
                        string.Format(CultureInfo.InvariantCulture, "{0} {1}", Right.Execute(), Direction));

        }

        #endregion
    }
}
