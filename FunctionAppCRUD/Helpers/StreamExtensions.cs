using System.Text.Json;

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
