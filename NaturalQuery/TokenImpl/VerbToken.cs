using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaturalQuery
{
    public class VerbToken : Token
    {
        protected override void BuildMapping(Dictionary<string, TokenMetadata> mapping)
        {
            if (mapping == null)
                throw new ArgumentNullException("mapping");

            mapping.Add("mostre", new TokenMetadata { Value = "mostre", TokenType = GetType() });
            mapping.Add("liste", new TokenMetadata { Value = "liste", IsPlural = true, TokenType = GetType() });
            mapping.Add("traga", new TokenMetadata { Value = "traga", TokenType = GetType() });
        }
    }
}
