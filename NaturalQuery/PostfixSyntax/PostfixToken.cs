using NaturalQuery.QueryTreeSyntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NaturalQuery.PostfixSyntax
{
    public class PostfixToken
    {
        #region Properties

        public PostfixTokenType Type { get; set; }
        public string Name { get; set; }
        public object[] Parameters { get; set; }
        public int ParametersLength { get; set; }

        #endregion

        #region Constructors

        public PostfixToken(PostfixTokenType type, string name, params object[] parameters)
        {
            Type = type;
            Name = name;
            Parameters = parameters;
        }

        public PostfixToken(PostfixTokenType type, string name, int parametersLength)
        {
            Type = type;
            Name = name;
            ParametersLength = parametersLength;
            Parameters = new object[0];
        }

        #endregion

        #region Exposed Methods

        public QueryNode Evaluate()
        {
            QueryNode result;

            Type type = typeof(QueryNode);
            MethodInfo method = type.GetMethod(Name, BindingFlags.FlattenHierarchy | BindingFlags.Static | BindingFlags.Public);

            result = (QueryNode)method.Invoke(null, Parameters);

            return result;
        }

        public override string ToString()
        {
            if (Type == PostfixTokenType.Operator)
            {
                return Name.ToUpper();
            }
            else
            {
                var parameter = Parameters.FirstOrDefault();
                if (parameter != null)
                {
                    Type genericType = parameter.GetType().GenericTypeArguments.FirstOrDefault();
                    if (genericType != null)
                    {
                        return string.Format("{0} ({1})", genericType.Name, Name);
                    }
                    else
                    {
                        return string.Format("{0} ({1})", parameter, Name);
                    }
                }
                else
                {
                    return string.Format("({0})", Name);
                }
            }
        }

        #endregion

    }
}
