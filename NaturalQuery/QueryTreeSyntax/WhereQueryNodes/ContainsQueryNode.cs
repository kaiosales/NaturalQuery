using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NaturalQuery.QueryTreeSyntax
{
    public class ContainsQueryNode : BinaryQueryNode
    {
        #region Constructors

        public ContainsQueryNode(QueryNode field, QueryNode value)
            : base(field, value)
        {
        }

        #endregion

        #region Exposed Methods

        internal override object Execute()
        {
            return string.Format(CultureInfo.InvariantCulture, "{0}.Contains(\"{1}\")", Left.Execute(), Right.Execute());
        }

        #endregion
    }
}
