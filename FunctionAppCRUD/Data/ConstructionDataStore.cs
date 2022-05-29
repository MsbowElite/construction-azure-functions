using FunctionAppCRUD.Core.Entity;
using FunctionAppCRUD.Data.Entities;

namespace FunctionAppCRUD.Data
{
    public interface IConstructionDataStore : IEntityDataStore<string, Construction>
    {
        Task<IEnumerable<Construction>> ListAsync();
    }
    public class ConstructionDataStore : EntityDataStore<string, Construction>, IConstructionDataStore
    {
        public ConstructionDataStore(
            EntityDataStoreOptions entityDataStoreOptions) : base("Construction", entityDataStoreOptions)
        {
        }

        public async Task<IEnumerable<Construction>> ListAsync()
        {
            var query =
                "Object eq 'Construction'";

            return await ListAsync(query);
        }
    }
}
