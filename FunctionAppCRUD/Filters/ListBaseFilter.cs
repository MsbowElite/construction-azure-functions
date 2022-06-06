using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionAppCRUD.Filters
{
    public abstract class ListBaseFilter : ICloneable
    {
        public short Limit { get; set; }
        public short Offset { get; set; }

        public ListBaseFilter()
        {
            Limit = 10;
            Offset = 0;
        }

        public abstract object Clone();
    }
}
