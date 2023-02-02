using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaturalQuery
{
    public class DataSource
    {
        #region Properties

        public ICollection<SourceEntity> Entities { get; private set; }

        #endregion

        #region Constructors

        public DataSource()
        {
            Entities = new List<SourceEntity>();
        }

        #endregion
    }
}
