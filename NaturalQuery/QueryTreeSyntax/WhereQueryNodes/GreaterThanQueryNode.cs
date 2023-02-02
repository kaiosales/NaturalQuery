using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaturalQuery.QueryTreeSyntax
{
    public class GreaterThanQueryNode : BinaryQueryNode
    {
        #region Constructors

        public GreaterThanQueryNode(QueryNode field, QueryNode value)
            : base(field, value)
        {
        }

        #endregion

        #region Exposed Methods

        internal override object Execute()
        {
            return string.Format(CultureInfo.InvariantCulture, "{0} > {1}", Left.Execute(), Right.Execute());
        }

        #endregion
    }
}
