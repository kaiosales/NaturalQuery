using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaturalQuery.QueryTreeSyntax
{
    public class ValueQueryNode : QueryNode
    {
        #region Properties

        public object Constant { get; private set; }

        #endregion

        #region Constructors

        public ValueQueryNode(object value)
            : base(value.GetHashCode())
        {
            Constant = value;
        }

        #endregion

        #region Exposed Methods

        internal override object Execute()
        {
            return Constant;
        }

        #endregion
    }
}
