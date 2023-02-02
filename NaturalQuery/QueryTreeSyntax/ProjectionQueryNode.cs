using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NaturalQuery.QueryTreeSyntax
{
    public class ProjectionQueryNode : BinaryQueryNode
    {
        #region Constructors

        public ProjectionQueryNode(QueryNode left, QueryNode right)
            : base(left, right)
        {

        }

        #endregion

        #region Exposed Methods

        internal override object Execute()
        {
            string left = Left.Execute().ToString();
            string right = Right.Execute().ToString();
            int index = right.IndexOf("new(", StringComparison.OrdinalIgnoreCase);

            if (index > -1)
            {
                right = right.Insert(index + 4, string.Format(CultureInfo.InvariantCulture, "{0}, ", left));
            }
            else
            {
                right = string.Format(CultureInfo.InvariantCulture, "new({0}, {1})", left, right);
            }

            return right;
        }

        #endregion
    }
}
