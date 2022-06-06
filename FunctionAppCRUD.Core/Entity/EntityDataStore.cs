using FunctionAppCRUD.Core.Helpers;
using Microsoft.WindowsAzure.Storage.Table;
using System.Net;

namespace FunctionAppCRUD.Core.Entity
{
    public interface IEntityDataStore<TKey, TEntity> where TEntity : IEntity<TKey>
    {
        Task AddAsync(
            TEntity entity);

        Task DeleteByIdAsync(
            TKey id);

        Task<TEntity> GetByIdAsync(
            TKey id);

        Task UpdateAsync(
            TEntity entity);
    }
    public abstract class EntityDataStore<TKey, TEntity> : IEntityDataStore<TKey, TEntity> where TEntity : Entity<TKey>, new()
    {
        protected readonly CloudTable _primaryCloudTable;
        protected readonly CloudTable _secondaryCloudTable;

        protected bool AutoFailover => _secondaryCloudTable != null;

        protected EntityDataStore(
            string tableName,
            EntityDataStoreOptions entityDataStoreOptions)
        {
            if (string.IsNullOrWhiteSpace(tableName))
            {
                throw new ArgumentNullException(nameof(tableName));
            }

            if (entityDataStoreOptions == null)
            {
                throw new ArgumentNullException(nameof(entityDataStoreOptions));
            }

            if (entityDataStoreOptions.CloudTableClientPrimary == null)
            {
                throw new ArgumentNullException(nameof(entityDataStoreOptions.CloudTableClientPrimary));
            }

            _primaryCloudTable =
                entityDataStoreOptions.CloudTableClientPrimary.GetTableReference(tableName);

            _primaryCloudTable.CreateIfNotExistsAsync();
        }

        public async Task AddAsync(
            TEntity entity)
        {
            if (string.IsNullOrWhiteSpace(entity.Id.ToString()))
            {
                throw new ArgumentNullException(nameof(entity.Id));
            }

            entity.RowKey = entity.Id.ToString();
            entity.PartitionKey = entity.Id.ToString();

            try
            {
                await this.AddAsync(entity, _primaryCloudTable);
            }
            catch
            {
                if (this.AutoFailover)
                {
                    await this.AddAsync(entity, _secondaryCloudTable);
                }
                else
                {
                    throw;
                }
            }
        }

        private async Task AddAsync(
            TEntity entity,
            CloudTable cloudTable)
        {
            var tableOperation =
                TableOperation.Insert(entity);

            var tableResult =
                await cloudTable.ExecuteAsync(tableOperation);

            tableResult.EnsureSuccessStatusCode();
        }

        private async Task DeleteAsync(
            TEntity entity)
        {
            try
            {
                await this.DeleteAsync(entity, _primaryCloudTable);
            }
            catch
            {
                if (this.AutoFailover)
                {
                    await this.DeleteAsync(entity, _secondaryCloudTable);
                }
                else
                {
                    throw;
                }
            }
        }

        private async Task DeleteAsync(
            TEntity entity,
            CloudTable cloudTable)
        {
            var tableOperation =
                TableOperation.Delete(entity);

            var tableResult =
                await cloudTable.ExecuteAsync(tableOperation);

            tableResult.EnsureSuccessStatusCode();
        }

        public async Task DeleteByIdAsync(
            TKey id)
        {
            if (string.IsNullOrWhiteSpace(id.ToString()))
            {
                throw new ArgumentNullException(nameof(id));
            }

            var entity =
                await this.GetByIdAsync(id);

            if (entity == null) return;

            await this.DeleteAsync(entity);
        }

        public async Task<TEntity> GetByIdAsync(
            TKey id)
        {
            if (string.IsNullOrWhiteSpace(id.ToString()))
            {
                throw new ArgumentNullException(nameof(id));
            }

            try
            {
                var entity =
                    await this.GetByIdAsync(id, _primaryCloudTable);

                return entity;
            }
            catch
            {
                if (this.AutoFailover)
                {
                    var entity =
                        await this.GetByIdAsync(id, _secondaryCloudTable);

                    return entity;
                }
                else
                {
                    throw;
                }
            }
        }

        private async Task<TEntity> GetByIdAsync(
            TKey id,
            CloudTable cloudTable)
        {
            var tableOperation =
                TableOperation.Retrieve<TEntity>(id.ToString(), id.ToString());

            var tableResult =
                await cloudTable.ExecuteAsync(tableOperation);

            if (tableResult.HttpStatusCode == (int)HttpStatusCode.NotFound)
            {
                return null;
            }

            tableResult.EnsureSuccessStatusCode();

            return tableResult.Result as TEntity;
        }

        protected async Task<IEnumerable<TEntity>> ListAsync(
            string query = null)
        {
            try
            {
                var entityList =
                    await ListAsync(query, _primaryCloudTable);

                return entityList;
            }
            catch
            {
                if (this.AutoFailover)
                {
                    var entityList =
                        await ListAsync(query, _secondaryCloudTable);

                    return entityList;
                }
                else
                {
                    throw;
                }
            }
        }

        // https://stackoverflow.com/questions/26257822/azure-table-query-async-continuation-token-always-returned

        private async Task<IEnumerable<TEntity>> ListAsync(
            string query,
            CloudTable cloudTable)
        {
            var tableQuery =
                new TableQuery<TEntity>();

            if (!string.IsNullOrWhiteSpace(query))
            {
                tableQuery =
                    new TableQuery<TEntity>().Where(query);
            }

            var entityList =
                new List<TEntity>();

            var continuationToken =
                default(TableContinuationToken);

            do
            {
                var tableQuerySegement =
                    await cloudTable.ExecuteQuerySegmentedAsync(tableQuery, continuationToken);

                continuationToken =
                    tableQuerySegement.ContinuationToken;

                entityList.AddRange(tableQuerySegement.Results);
            }
            while (continuationToken != null);

            return entityList;
        }

        public async Task UpdateAsync(
            TEntity entity)
        {
            if (string.IsNullOrWhiteSpace(entity.Id.ToString()))
            {
                throw new ArgumentNullException(nameof(entity.Id));
            }

            entity.RowKey = entity.Id.ToString();
            entity.PartitionKey = entity.Id.ToString();

            try
            {
                await this.UpdateAsync(entity, _primaryCloudTable);
            }
            catch
            {
                if (this.AutoFailover)
                {
                    await this.UpdateAsync(entity, _secondaryCloudTable);
                }
                else
                {
                    throw;
                }
            }
        }

        private async Task UpdateAsync(
            TEntity entity,
            CloudTable cloudTable)
        {
            var tableOperation =
                TableOperation.InsertOrReplace(entity);

            var tableResult =
                await cloudTable.ExecuteAsync(tableOperation);

            tableResult.EnsureSuccessStatusCode();
        }
    }
}
