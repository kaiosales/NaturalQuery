using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaturalQuery.QueryTreeSyntax
{
    public class EmptyQueryNode : QueryNode
    {
        #region Constructors

        public EmptyQueryNode()
            : base(0)
        {

        }

        #endregion
    }
}
