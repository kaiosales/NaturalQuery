using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NaturalQuery.QueryTreeSyntax
{
    public class FieldQueryNode : QueryNode
    {
        #region Properties

        public string MemberName { get; private set; }

        #endregion

        #region Constructors

        public FieldQueryNode(string memberName)
            : base(memberName.GetHashCode())
        {
            MemberName = memberName;
        }

        #endregion

        #region Exposed Methods

        internal override object Execute()
        {
            return MemberName;
        }

        #endregion
    }
}
