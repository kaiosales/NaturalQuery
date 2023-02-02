using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaturalQuery
{
    public class TokenMetadata
    {
        #region Properties

        public Type TokenType { get; internal set; }
        public bool IsPlural { get; internal set; }
        public string Value { get; internal set; }
        public TokenGenderType Gender { get; internal set; }

        #endregion
    }
}
