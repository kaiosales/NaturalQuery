using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaturalQuery
{
    public abstract class Token
    {
        #region Fields

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        public TokenChain Chain { get; private set; }
        public Token Successor { get; private set; }
        public Dictionary<string, TokenMetadata> Mapping { get; private set; }

        #endregion

        #region Cosntructors

        public Token()
        {
            Mapping = new Dictionary<string, TokenMetadata>();
        }

        #endregion

        #region Exposed Methods

        internal virtual void BuildMapping()
        {
            BuildMapping(Mapping);
        }

        internal virtual void SetChain(TokenChain chain)
        {
            this.Chain = chain;
        }

        internal virtual void SetSuccessor(Token successor)
        {
            this.Successor = successor;
        }

        public virtual TokenMetadata Process(string word)
        {
            TokenMetadata result = null;
            Mapping.TryGetValue(word, out result);

            if (result == null && Successor != null)
            {
                return Successor.Process(word);
            }

            return result;
        }

        #endregion

        #region Internal Methods

        protected abstract void BuildMapping(Dictionary<string, TokenMetadata> mapping);

        #endregion
    }
}
