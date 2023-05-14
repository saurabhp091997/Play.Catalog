using Microsoft.AspNetCore.Mvc;
using Play.Catalog.Service;
using Play.Catalog.Service.DTOS;
using Play.Catalog.Service.Entities;
using Play.Catalog.Service.Repositories;

namespace Play.Catalog.Controllers
{
    [ApiController]
    [Route("items")]
    public class ItemsController : ControllerBase
    {
        private readonly ItemsRepository itemsRepository = new();

        //GET /items
        [HttpGet]
        public async Task<IEnumerable<ItemDto>> GetItemsAsync()
        {
            var items = (await itemsRepository.GetAllItemAsync()).Select(item => item.AsDto());
            return items;
        }

        //GET /items/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ItemDto>> GetItemByIdAsync(Guid id)
        {
            var item = await itemsRepository.GetItemAsync(id);

            if (item == null)
            {
                return NotFound();
            }

            return item.AsDto();
        }

        //POST /items
        [HttpPost]
        public async Task<ActionResult<ItemDto>> PostItemAsync(CreateItemDto createItemDto)
        {
            var item = new Item
            {
                Name = createItemDto.Name,
                Description = createItemDto.Description,
                Price = createItemDto.Price,
                CreatedDate = DateTimeOffset.UtcNow
            };

            await itemsRepository.CreateItemAsync(item);

            return CreatedAtAction(nameof(GetItemByIdAsync), new { id = item.Id }, item);
        }

        //PUT /items/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult<ItemDto>> PutItemAsync(Guid id, UpdateItemDto updateItemDto)
        {
            var existingItem = await itemsRepository.GetItemAsync(id);

            if (existingItem == null)
            {
                return NotFound();
            }

            existingItem.Name = updateItemDto.Name;
            existingItem.Description = updateItemDto.Description;
            existingItem.Price = updateItemDto.Price;

            await itemsRepository.UpdateItemAsync(existingItem);

            return NoContent();
        }

        //DELETE /items/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItemAsync(Guid id)
        {
            var item = await itemsRepository.GetItemAsync(id);

            if (item == null)
            {
                return NotFound();
            }

            await itemsRepository.RemoveItemAsync(id);

            return NoContent();
        }
    }
}