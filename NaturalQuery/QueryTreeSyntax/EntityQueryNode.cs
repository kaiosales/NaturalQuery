using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NaturalQuery.QueryTreeSyntax
{
    public class EntityQueryNode : QueryNode
    {
        #region Properties

        public IQueryable EntitySource { get; private set; }

        #endregion

        #region Constructors

        public EntityQueryNode(IQueryable entity)
            : base(entity.GetHashCode())
        {
            EntitySource = entity;
        }

        #endregion

        #region Exposed Methods

        internal override object Execute()
        {
            return EntitySource;
        }

        #endregion
    }
}
