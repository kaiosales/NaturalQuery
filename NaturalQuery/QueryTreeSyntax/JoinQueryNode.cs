using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Dynamic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NaturalQuery.QueryTreeSyntax
{
    public class JoinQueryNode : BinaryQueryNode
    {
        #region Properties

        public FieldQueryNode LeftKey { get; set; }
        public FieldQueryNode RightKey { get; set; }

        #endregion

        #region Constructors

        public JoinQueryNode(EntityQueryNode something, FieldQueryNode leftKey, QueryNode withSomething, FieldQueryNode rightKey)
            : base(something, withSomething)
        {
            LeftKey = leftKey;
            RightKey = rightKey;
        }

        #endregion

        #region Exposed Methods

        internal override object Execute()
        {
            var left = (IQueryable)Left.Execute();
            var right = (IQueryable)Right.Execute();

            string expression;
            if (right.ElementType.IsSubclassOf(typeof(DynamicClass)))
            {
                StringBuilder builder = new StringBuilder("new(left as {0}");
                PropertyInfo[] properties = right.ElementType.GetProperties();

                for (int i = 0; i < properties.Length; i++)
                {
                    builder.AppendFormat(", right.{0} as {0}", properties[i].Name);
                }

                builder.Append(")");
                expression = builder.ToString();
            }
            else
            {
                expression = "new(left as {0}, right as {1})";
            }

            return DynamicQueryable.Join(
                left,
                right,
                LeftKey.Execute().ToString(),
                RightKey.Execute().ToString(),
                string.Format(CultureInfo.InvariantCulture, expression, left.ElementType.Name, right.ElementType.Name));
        }

        public override string ToString(int indent, string propertyName)
        {
            string name = base.ToString();

            indent = indent + name.Length;
            int childrenIndent = (int)Math.Ceiling(indent + name.Length / 2d) + 2;

            string value = string.Format("{0}{1}{2}{3}{1}{2}{4}{5}{6} {7}{1}{2}{3}{1}{2}{4}{5}{6} {8}{1}{2}{3}{1}{2}{4}{5}{6} {9}{1}{2}{3}{1}{2}{4}{5}{6} {10}",
                name,
                Environment.NewLine,
                "".PadLeft(indent, ' '),
                (char)0x2502,
                (char)0x2514,
                (char)0x2500,
                "".PadLeft(name.Length / 4, (char)0x2500),
                Left.ToString(childrenIndent, "Left"),
                LeftKey.ToString(childrenIndent, "LeftKey"),
                Right.ToString(childrenIndent, "Right"),
                RightKey.ToString(childrenIndent, "RightKey")
            );


            return value;
        }

        #endregion
    }
}
