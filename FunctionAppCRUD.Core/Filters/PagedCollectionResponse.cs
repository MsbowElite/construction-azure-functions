using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionAppCRUD.Core.Filters
{
    public class PagedCollectionResponse<T> where T : class
    {
        public IEnumerable<T> Items { get; set; } = default!;

        public Uri NextPage { get; set; } = default!;

        public Uri MyProperty { get; set; } = default!;

        public int TotalCount { get; set; } = default!;
    }
}
