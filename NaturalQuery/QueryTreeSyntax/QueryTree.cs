using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaturalQuery.QueryTreeSyntax
{
    public class QueryTree
    {
        #region Properties

        public QueryNode Root
        {
            get;
            private set;
        }

        #endregion

        #region Constructors

        public QueryTree(QueryNode root)
        {
            Root = root;
        }

        #endregion

        #region Equality Methods

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (System.Object.ReferenceEquals(this, obj))
            {
                return true;
            }

            if (((object)this == null) || ((object)obj == null))
            {
                return false;
            }

            QueryTree tree = obj as QueryTree;
            if ((System.Object)tree == null)
            {
                return false;
            }

            return this.Root == tree.Root;
        }

        public static bool operator ==(QueryTree treeA, QueryTree treeB)
        {
            return treeA.Equals(treeB);
        }

        public static bool operator !=(QueryTree treeA, QueryTree treeB)
        {
            return !(treeA == treeB);
        }

        #endregion

        #region Exposed Methods

        public IQueryable<dynamic> ToQueryable()
        {
            if (Root == null)
                throw new InvalidOperationException();

            return (Root.Execute() as IQueryable<dynamic>);
        }

        public override string ToString()
        {
            return Root.ToString(0, string.Empty);
        }

        #endregion
    }
}
