using Play.Catalog.Service.Entities;

namespace Play.Catalog.Service.Repositories
{
    public interface IRepository<T> where T : IEntity
    {
        Task CreateItemAsync(T entity);
        Task<IReadOnlyCollection<T>> GetAllItemAsync();
        Task<T> GetItemAsync(Guid id);
        Task RemoveItemAsync(Guid id);
        Task UpdateItemAsync(T entity);
    }
}