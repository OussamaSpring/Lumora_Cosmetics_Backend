namespace Application.DTOs.Product
{
    public class ProductDetailDto : ProductDto
    {
        public List<ProductItemDto> ProductItems { get; set; }
    }
}