using Microsoft.AspNetCore.Mvc;
using Play.Catalog.Service;
using Play.Catalog.Service.DTOS;
using Play.Catalog.Service.Entities;
using Play.Common;

namespace Play.Catalog.Controllers
{
    [ApiController]
    [Route("items")]
    public class ItemsController : ControllerBase
    {
        private readonly IRepository<Item> itemsRepository;
        private static int requestCounter = 0;

        public ItemsController(IRepository<Item> _itemsRepository)
        {
            this.itemsRepository = _itemsRepository;
        }

        //GET /items
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ItemDto>>> GetItemsAsync()
        {
            requestCounter++;
            Console.WriteLine($"Request {requestCounter}: Starting...");

            if (requestCounter <= 2)
            {
                Console.WriteLine($"Request {requestCounter}: Delaying...");
                await Task.Delay(TimeSpan.FromSeconds(10));
            }

            if (requestCounter <= 4)
            {
                Console.WriteLine($"Request {requestCounter}: 500 (Internal Server Error).");
                return StatusCode(500);
            }

            var items = (await itemsRepository.GetAllItemAsync()).Select(item => item.AsDto());

            Console.WriteLine($"Request {requestCounter}: 200 (OK).");

            return Ok(items);
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