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
    public class AggregationQueryNode : BinaryQueryNode
    {
        #region Constructors

        public AggregationQueryNode(QueryNode name, QueryNode expression)
            : base(name, expression)
        {

        }

        #endregion

        #region Exposed Methods

        internal override object Execute()
        {
            string left = Left.Execute().ToString();

            string expression;
            string name;
            if (Right.IsEmpty())
            {
                expression = string.Empty;
                name = left;
            }
            else
            {
                expression = Right.Execute().ToString();
                name = expression.Substring(expression.LastIndexOf('.') + 1);
            }

            return string.Format(CultureInfo.InvariantCulture, "it.{0}({2}) as {1}", left, name, expression);
        }

        #endregion
    }
}
