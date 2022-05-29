using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using System;

namespace FunctionAppCRUD.Core.Entity
{
    public interface IEntity<TKey>
    {
    }
    public abstract class Entity<TKey> : TableEntity, IEntity<TKey>
    {
        public TKey Id { get; set; } = default!;

        public string Object { get; set; } = default!;

        public DateTime CreatedOn { get; set; } = default!;

        protected Entity()
        {
            this.CreatedOn = DateTime.UtcNow;
        }
    }
}
