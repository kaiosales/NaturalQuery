using NaturalQuery.QueryTreeSyntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaturalQuery.PostfixSyntax
{
    public class PostfixExpression
    {
        #region Fields

        private Stack<QueryNode> stack;
        private Queue<PostfixToken> expression;
        private Queue<PostfixToken> _expression;

        #endregion

        #region Constructors

        public PostfixExpression()
        {
            stack = new Stack<QueryNode>();
            expression = new Queue<PostfixToken>();
            _expression = new Queue<PostfixToken>();
        }

        #endregion

        #region Exposed Methods

        public void AddValue(string name, params object[] parameters)
        {
            expression.Enqueue(new PostfixToken(PostfixTokenType.Value, name, parameters: parameters));
            _expression.Enqueue(new PostfixToken(PostfixTokenType.Value, name, parameters: parameters));
        }

        public void AddOperator(string name, int parametersLength)
        {
            expression.Enqueue(new PostfixToken(PostfixTokenType.Operator, name, parametersLength: parametersLength));
            _expression.Enqueue(new PostfixToken(PostfixTokenType.Operator, name, parametersLength: parametersLength));
        }

        public QueryTree ToQueryTree()
        {
            QueryNode root = null;

            while (expression.Count > 0)
            {
                PostfixToken token = expression.Dequeue();

                if (token.Type == PostfixTokenType.Value)
                {
                    stack.Push(token.Evaluate());
                }
                else
                {
                    if (stack.Count < token.ParametersLength)
                    {
                        throw new InvalidOperationException("");
                    }
                    else
                    {
                        QueryNode[] args = new QueryNode[token.ParametersLength];
                        for (int i = token.ParametersLength - 1; i >= 0; i--)
                        {
                            args[i] = stack.Pop();
                        }

                        token.Parameters = args.Concat(token.Parameters).ToArray();
                        stack.Push(token.Evaluate());
                    }
                }
            }

            if (stack.Count == 1)
            {
                root = stack.Pop();
            }
            else
            {
                throw new InvalidOperationException();
            }

            QueryTree tree = new QueryTree(root);
            return tree;
        }

        public override string ToString()
        {
            StringBuilder str = new StringBuilder();
            str.Append("{ ");

            foreach (var token in _expression)
            {
                str.AppendFormat("{0}, ", token);
            }

            if (_expression.Count > 0)
            {
                str.Remove(str.Length - 2, 2);
            }

            str.Append(" }");

            return str.ToString();
        }

        #endregion
    }
}
