using Domain.Enums.enProduct;
namespace Application.DTOs.Product
{
    public class ProductItemDto
    {
        public int Id { get; set; }
        public string ProductCode { get; set; }
        public decimal OriginalPrice { get; set; }
        public decimal? SalePrice { get; set; }
        public List<VariantDto> ItemVariants { get; set; }
        public StockDto Stock { get; set; }
        public ProductImageDto Image { get; set; }
    }
}
