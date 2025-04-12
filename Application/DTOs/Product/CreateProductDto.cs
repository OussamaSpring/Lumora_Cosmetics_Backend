using Application.DTOs.ProductItem;

namespace Application.DTOs.Product;

public class CreateProductDto
{
    public string Name { get; set; }
    public string Brand { get; set; }
    public string About { get; set; }
    public string Ingredients { get; set; }
    public string HowToUse { get; set; }
    public int Gender { get; set; }
    public int CategoryId { get; set; }
    public int Status { get; set; }

    public List<CreateProductItemDto> ProductItems { get; set; }
}