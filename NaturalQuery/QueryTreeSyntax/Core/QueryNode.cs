using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NaturalQuery.QueryTreeSyntax
{
    public abstract class QueryNode
    {
        #region Fields

        protected readonly int HASHCODE;
        protected PropertyInfo[] properties;
        protected string name;

        #endregion

        #region Constructor

        protected QueryNode(int hashcode)
        {
            HASHCODE = hashcode;
        }

        #endregion

        #region Equality Methods

        public override int GetHashCode()
        {
            return this.HASHCODE;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            QueryNode node = obj as QueryNode;
            if ((System.Object)node == null)
            {
                return false;
            }

            return this.GetHashCode() == node.GetHashCode();
        }

        public static bool operator ==(QueryNode nodeA, QueryNode nodeB)
        {
            if (System.Object.ReferenceEquals(nodeA, nodeB))
            {
                return true;
            }

            if (((object)nodeA == null) || ((object)nodeB == null))
            {
                return false;
            }

            return nodeA.Equals(nodeB);
        }

        public static bool operator !=(QueryNode nodeA, QueryNode nodeB)
        {
            return !(nodeA == nodeB);
        }

        #endregion

        #region Exposed Methods

        internal virtual object Execute()
        {
            throw new NotImplementedException();
        }

        public bool IsEmpty()
        {
            return this is EmptyQueryNode;
        }

        public override string ToString()
        {
            if (string.IsNullOrEmpty(name))
            {
                name = this.GetType().Name.Replace("QueryNode", string.Empty);
            }

            return name;
        }

        public virtual string ToString(int indent, string propertyName)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat("{0}: {{", this);
            if (properties == null)
            {
                properties = this.GetType().GetProperties(BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.Instance);
            }
            int length = str.Length;

            for (int i = 0; i < properties.Count(); i++)
            {
                PropertyInfo prop = properties[i];

                if (prop.PropertyType == typeof(QueryNode) || prop.PropertyType.IsSubclassOf(typeof(QueryNode)))
                {
                    continue;
                }

                if (prop.PropertyType == typeof(IQueryable))
                {
                    var valueType = prop.GetValue(this, null).GetType();
                    str.AppendFormat(" {0}: '{1}',", prop.Name, valueType.GenericTypeArguments.First().Name);
                }
                else
                {
                    str.AppendFormat(" {0}: '{1}',", prop.Name, prop.GetValue(this, null));
                }
            }

            str.Remove(str.Length - 1, 1);
            if (str.Length != length - 1)
            {
                str.Append(" }");
            }



            return str.ToString();
        }

        #endregion

        #region Static Build Methods

        public static AggregationQueryNode Aggregation(ValueQueryNode value, QueryNode expression)
        {
            return new AggregationQueryNode(value, expression);
        }

        public static ContainsQueryNode Contains(QueryNode field, QueryNode value)
        {
            return new ContainsQueryNode(field, value);
        }

        public static EntityQueryNode Entity(IQueryable queryable)
        {
            return new EntityQueryNode(queryable);
        }

        public static FieldQueryNode Field(string memberName)
        {
            return new FieldQueryNode(memberName);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public static GreaterThanQueryNode GreaterThan(FieldQueryNode field, ValueQueryNode value)
        {
            return new GreaterThanQueryNode(field, value);
        }

        public static GroupQueryNode Group(QueryNode something, QueryNode bySomething)
        {
            return new GroupQueryNode(something, bySomething);
        }

        public static FieldQueryNode GroupField(string memberName)
        {
            return new FieldQueryNode("Key." + memberName);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public static IsEqualsQueryNode IsEquals(FieldQueryNode field, ValueQueryNode value)
        {
            return new IsEqualsQueryNode(field, value);
        }

        public static JoinQueryNode Join(EntityQueryNode something, FieldQueryNode leftKey, QueryNode withSomething, FieldQueryNode rightKey)
        {
            return new JoinQueryNode(something, leftKey, withSomething, rightKey);
        }

        public static OrderQueryNode Order(QueryNode something, QueryNode bySomething)
        {
            return new OrderQueryNode(something, bySomething, OrderDirection.Ascending);
        }

        public static OrderQueryNode OrderDescending(QueryNode something, QueryNode bySomething)
        {
            return new OrderQueryNode(something, bySomething, OrderDirection.Descending);
        }

        public static BinaryQueryNode Select(QueryNode @this, QueryNode fromThis)
        {
            return new SelectQueryNode(@this, fromThis);
        }

        public static ProjectionQueryNode Projection(QueryNode first, QueryNode second)
        {
            return new ProjectionQueryNode(first, second);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1719:ParameterNamesShouldNotMatchMemberNames", MessageId = "0#")]
        public static ValueQueryNode Value(object value)
        {
            return new ValueQueryNode(value);
        }

        public static WhereQueryNode Where(QueryNode something, QueryNode condition)
        {
            return new WhereQueryNode(something, condition);
        }

        public static EmptyQueryNode Empty()
        {
            return new EmptyQueryNode();
        }

        #endregion
    }
}
