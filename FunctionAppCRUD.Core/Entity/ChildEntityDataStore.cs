using FunctionAppCRUD.Core.Helpers;
using Microsoft.WindowsAzure.Storage.Table;
using System.Net;

namespace FunctionAppCRUD.Core.Entity
{
    public interface IChildEntityDataStore<TParentKey, TKey, TEntity> where TEntity : IChildEntity<TKey>
    {
        Task AddAsync(
            TParentKey parentId,
            TEntity entity);

        Task DeleteByIdAsync(
            TParentKey parentId,
            TKey id);

        Task<TEntity> GetByIdAsync(
            TParentKey parentId,
            TKey id);

        Task UpdateAsync(
            TParentKey parentId,
            TEntity entity);
    }
    public abstract class ChildEntityDataStore<TParentKey, TKey, TEntity> : IChildEntityDataStore<TParentKey, TKey, TEntity> where TEntity : ChildEntity<TKey>, new()
    {
        protected readonly CloudTable _primaryCloudTable;
        protected readonly CloudTable _secondaryCloudTable;

        protected bool AutoFailover => _secondaryCloudTable != null;

        protected ChildEntityDataStore(
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

            _primaryCloudTable =
                entityDataStoreOptions.CloudTableClientPrimary.GetTableReference(tableName);
        }

        public virtual async Task AddAsync(
            TParentKey parentId,
            TEntity entity)
        {
            if (string.IsNullOrWhiteSpace(entity.Id.ToString()))
            {
                throw new ArgumentNullException(nameof(entity.Id));
            }

            if (string.IsNullOrWhiteSpace(parentId.ToString()))
            {
                throw new ArgumentNullException(nameof(parentId));
            }

            entity.RowKey = entity.Id.ToString();
            entity.PartitionKey = parentId.ToString();

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

        public virtual async Task DeleteByIdAsync(
            TParentKey parentId,
            TKey id)
        {
            if (string.IsNullOrWhiteSpace(id.ToString()))
            {
                throw new ArgumentNullException(nameof(id));
            }

            if (string.IsNullOrWhiteSpace(parentId.ToString()))
            {
                throw new ArgumentNullException(nameof(parentId));
            }

            var entity =
                await this.GetByIdAsync(parentId, id);

            if (entity == null) return;

            await this.DeleteAsync(entity);
        }

        public virtual async Task<TEntity> GetByIdAsync(
            TParentKey parentId,
            TKey id)
        {
            if (string.IsNullOrWhiteSpace(id.ToString()))
            {
                throw new ArgumentNullException(nameof(id));
            }

            if (string.IsNullOrWhiteSpace(parentId.ToString()))
            {
                throw new ArgumentNullException(nameof(parentId));
            }

            try
            {
                var entity =
                    await this.GetByIdAsync(parentId, id, _primaryCloudTable);

                return entity;
            }
            catch
            {
                if (this.AutoFailover)
                {
                    var entity =
                        await this.GetByIdAsync(parentId, id, _secondaryCloudTable);

                    return entity;
                }
                else
                {
                    throw;
                }
            }
        }

        private async Task<TEntity> GetByIdAsync(
            TParentKey parentId,
            TKey id,
            CloudTable cloudTable)
        {
            var tableOperation =
                TableOperation.Retrieve<TEntity>(parentId.ToString(), id.ToString());

            var tableResult =
                await cloudTable.ExecuteAsync(tableOperation);

            if (tableResult.HttpStatusCode == (int)HttpStatusCode.NotFound)
            {
                return null;
            }

            tableResult.EnsureSuccessStatusCode();

            return tableResult.Result as TEntity;
        }

        public virtual async Task UpdateAsync(
            TParentKey parentId,
            TEntity entity)
        {
            entity.RowKey = entity.Id.ToString();
            entity.PartitionKey = parentId.ToString();

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
        protected async Task<IEnumerable<TEntity>> ListAsync(
            string query = null)
        {
            try
            {
                var entityList =
                    await this.ListAsync(query, _primaryCloudTable);

                return entityList;
            }
            catch
            {
                if (this.AutoFailover)
                {
                    var entityList =
                        await this.ListAsync(query, _secondaryCloudTable);

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
