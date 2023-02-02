using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaturalQuery.QueryTreeSyntax
{
    public class BinaryQueryNode : QueryNode
    {
        #region Properties

        public QueryNode Left { get; internal set; }
        public QueryNode Right { get; internal set; }

        #endregion

        #region Constructors

        public BinaryQueryNode(QueryNode left, QueryNode right)
            : base(new Tuple<int, int>(left.GetHashCode(), right.GetHashCode()).GetHashCode())
        {
            Left = left;
            Right = right;
        }

        #endregion

        #region Exposed Methods

        public override string ToString(int indent, string propertyName)
        {
            string name = base.ToString();

            indent = indent + name.Length;
            int childrenIndent = (int)Math.Ceiling(indent + name.Length / 2d) + 2;

            string value = string.Format("{0}{1}{2}{3}{1}{2}{4}{5}{6} {7}{1}{2}{3}{1}{2}{4}{5}{6} {8}",
                name,
                Environment.NewLine,
                "".PadLeft(indent, ' '),
                (char)0x2502,
                (char)0x2514,
                (char)0x2500,
                "".PadLeft(name.Length / 4, (char)0x2500),
                Left.ToString(childrenIndent, "Left"),
                Right.ToString(childrenIndent, "Right")
            );


            return value;
        }

        #endregion
    }
}
