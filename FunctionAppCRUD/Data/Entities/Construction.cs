using FunctionAppCRUD.Core.Entity;
using System.Text.Json.Serialization;

namespace FunctionAppCRUD.Data.Entities
{
    public interface IToDoEntityDataStore : IEntityDataStore<string, Construction>
    {
        Task<IEnumerable<Construction>> ListAsync();
    }
    public class Construction : Entity<string>
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = default!;

        [JsonPropertyName("description")]
        public string Description { get; set; } = default!;

        [JsonPropertyName("imageUrl")]
        public string ImageUrl { get; set; } = default!;

        [JsonPropertyName("builtAt")]
        public DateTime BuiltAt { get; set; } = default!;

        [JsonPropertyName("cost")]
        public long Cost { get; set; } = default!;

        public Construction() : base()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Object = "Construction";
        }
    }
}
