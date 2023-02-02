using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaturalQuery
{
    public class SourceEntity
    {
        #region Properties

        public string Name { get; set; }
        public ICollection<SourceProperty> Attributes { get; private set; }

        #endregion

        #region Constructors

        public SourceEntity()
        {
            Attributes = new List<SourceProperty>();
        }

        #endregion
    }
}
