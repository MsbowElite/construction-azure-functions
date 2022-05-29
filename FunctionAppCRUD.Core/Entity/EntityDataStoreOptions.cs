using Microsoft.WindowsAzure.Storage.Table;

namespace FunctionAppCRUD.Core.Entity
{
    public class EntityDataStoreOptions
    {
        public CloudTableClient CloudTableClientPrimary { get; set; } = default!;

        public EntityDataStoreOptions()
        {

        }

        public EntityDataStoreOptions(CloudTableClient cloudTableClientPrimary)
        {
            CloudTableClientPrimary = cloudTableClientPrimary;
        }
    }
}