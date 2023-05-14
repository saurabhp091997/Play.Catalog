using System.ComponentModel.DataAnnotations;
using Play.Catalog.Service.Entities;

namespace Play.Catalog.Service.DTOS
{
    public record ItemDto(Guid Id, string Name, string Description, decimal Price, DateTimeOffset CreatedDate)
    {
        public static implicit operator ItemDto(Item v)
        {
            throw new NotImplementedException();
        }
    }

    public record CreateItemDto([Required] string Name, string Description, [Range(0, 1000)] decimal Price);

    public record UpdateItemDto([Required] string Name, string Description, [Range(0, 1000)] decimal Price);
}