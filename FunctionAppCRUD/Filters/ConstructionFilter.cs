using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FunctionAppCRUD.Filters
{
    public class ConstructionFilter : ListBaseFilter
    {
        public string SearchTerm { get; set; }

        public ConstructionFilter(string searchTerm, short? limit, short? offset) : base()
        {
            SearchTerm = searchTerm;

            if (limit is not null)
                Limit = limit.Value;
            if (offset is not null)
                Offset = offset.Value;
        }

        public static bool TryParse(string? value, IFormatProvider? provider,
                                    out ConstructionFilter? bookFilter)
        {
            //var trimmedValue = value?.TrimStart()
            bookFilter = null;
            return false;
        }

        public override object Clone()
        {
            var jsonString = JsonSerializer.Serialize(this);
            return JsonSerializer.Deserialize(jsonString, GetType());
        }
    }
}
