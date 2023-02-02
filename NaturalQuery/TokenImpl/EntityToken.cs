using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaturalQuery
{
    public class EntityToken : Token
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")]
        protected override void BuildMapping(Dictionary<string, TokenMetadata> mapping)
        {
            if (mapping == null) 
                throw new ArgumentNullException("mapping");

            if (Chain.Source == null)
                return;

            foreach (SourceEntity entity in Chain.Source.Entities)
            {
                string name = entity.Name.ToLower(CultureInfo.InvariantCulture);

                mapping.Add(name, new TokenMetadata { Value = name, TokenType = GetType() });
                mapping.Add(name.Pluralize(), new TokenMetadata { Value = name.Pluralize(), TokenType = GetType(), IsPlural = true });
            }
        }
    }
}
