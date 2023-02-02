using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaturalQuery.QueryTreeSyntax
{
    public class IsEqualsQueryNode : BinaryQueryNode
    {
        #region Constructors

        public IsEqualsQueryNode(QueryNode field, QueryNode value)
            : base(field, value)
        {

        }

        #endregion

        #region Exposed Methods

        internal override object Execute()
        {
            var value = Right.Execute();

            string expression;

            if (value.GetType() == typeof(string))
            {
                expression = "{0} == \"{1}\"";
            }
            else
            {
                expression = "{0} == {1}";
            }

            return string.Format(CultureInfo.InvariantCulture, expression, Left.Execute(), value);
        }

        #endregion
    }
}
