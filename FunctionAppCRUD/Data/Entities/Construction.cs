using FunctionAppCRUD.Core.Entity;

namespace FunctionAppCRUD.Data.Entities
{
    public interface IToDoEntityDataStore : IEntityDataStore<string, Construction>
    {
        Task<IEnumerable<Construction>> ListAsync();
    }
    public class Construction : Entity<string>
    {
        public string Name { get; set; } = default!;

        public string Description { get; set; } = default!;

        public string ImageUrl { get; set; } = default!;

        public DateTime BuiltAt { get; set; } = default!;

        public long Cost { get; set; } = default!;

        public Construction() : base()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Object = "Construction";
        }
    }
}
