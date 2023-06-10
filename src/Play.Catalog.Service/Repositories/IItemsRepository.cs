using Play.Catalog.Service.Entities;

namespace Play.Catalog.Service.Repositories
{
    public interface IItemsRepository
    {
        Task CreateItemAsync(Item entity);
        Task<IReadOnlyCollection<Item>> GetAllItemAsync();
        Task<Item> GetItemAsync(Guid id);
        Task RemoveItemAsync(Guid id);
        Task UpdateItemAsync(Item entity);
    }
}