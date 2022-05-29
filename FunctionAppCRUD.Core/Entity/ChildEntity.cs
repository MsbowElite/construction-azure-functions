namespace FunctionAppCRUD.Core.Entity
{
    public interface IChildEntity<TKey>
    {
    }
    public abstract class ChildEntity<TKey> : Entity<TKey>, IChildEntity<TKey>
    {
    }
}
