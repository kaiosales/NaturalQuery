using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaturalQuery
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix")]
    public class TokenChain : List<Token>
    {
        #region Properties

        public bool IsBuilt { get; private set; }
        public DataSource Source { get; set; }
        public Dictionary<string, string> Synonyms { get; private set; }

        #endregion

        #region Constructors

        public TokenChain()
            : base()
        {
            Synonyms = new Dictionary<string, string>();
        }

        #endregion

        #region Exposed Methods

        public new void Add(Token item)
        {
            if (item == null)
                throw new ArgumentNullException("item");

            item.SetChain(this);
            
            base.Add(item);
        }

        public void BuildChain()
        {
            IsBuilt = true;

            for (int i = 0; i < this.Count; i++)
            {
                Token current = this[i];
                Token next = i == this.Count - 1 ? null : this[i + 1];

                current.BuildMapping();
                current.SetSuccessor(next);
            }
        }

        public IEnumerable<TokenMetadata> Process(string inputText)
        {
            if (this.Count == 0)
                throw new InvalidOperationException("The chain is empty.");

            if (!IsBuilt)
                BuildChain();

            NormalizeInput(ref inputText);
            string[] tokens = inputText.Split(' ');

            for (int i = 0; i < tokens.Length; i++)
            {
                yield return this[0].Process(tokens[i]);
            }
        }

        #endregion

        #region Internal Methods

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")]
        private void NormalizeInput(ref string inputText)
        {
            inputText = inputText.ToLower(CultureInfo.InvariantCulture);

            ProcessSynonyms(ref inputText);
        }

        private void ProcessSynonyms(ref string inputText)
        {
            foreach (var pair in Synonyms)
            {
                inputText = inputText.Replace(pair.Key, pair.Value);
            }
        }

        #endregion
    }
}
