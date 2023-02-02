using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaturalQuery
{
    public class ArticleToken : Token
    {
        protected override void BuildMapping(Dictionary<string, TokenMetadata> mapping)
        {
            if (mapping == null) 
                throw new ArgumentNullException("mapping");

            mapping.Add("o", new TokenMetadata { Value = "o", Gender = TokenGenderType.Male, TokenType = GetType() });
            mapping.Add("os", new TokenMetadata { Value = "os", IsPlural = true, Gender = TokenGenderType.Male, TokenType = GetType() });
            mapping.Add("a", new TokenMetadata { Value = "a", Gender = TokenGenderType.Female, TokenType = GetType() });
            mapping.Add("as", new TokenMetadata { Value = "as", IsPlural = true, Gender = TokenGenderType.Female, TokenType = GetType() });
        }
    }
}
