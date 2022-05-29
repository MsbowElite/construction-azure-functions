using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FunctionAppCRUD.Helpers
{
    internal static class StreamExtensions
    {
        internal static async Task<T?> DeserializeAsync<T>(
           this Stream stream)
        {
            return await JsonSerializer.DeserializeAsync<T?>(stream);
        }
    }
}
