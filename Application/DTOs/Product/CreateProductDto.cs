using Application.DTOs.ProductItem;
using Domain.Enums.enProduct;

namespace Application.DTOs.Product;

public class CreateProductDto
{
    public string Name { get; set; }
    public string Brand { get; set; }
    public string About { get; set; }
    public string Ingredients { get; set; }
    public string HowToUse { get; set; }
    public Gender? Gender { get; set; }
    public short CategoryId { get; set; }
    public ProductStatus Status { get; set; }

    public List<CreateProductItemDto> ProductItems { get; set; }
}