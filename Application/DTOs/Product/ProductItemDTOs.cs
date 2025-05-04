

namespace Application.DTOs.Product
{
    public class CreateVariantDto
    {
        public int ProductId { get; set; }
        public string ProductCode { get; set; }
        public decimal OriginalPrice { get; set; }
        public decimal? SalePrice { get; set; }
        public Dictionary<string, string> VariantValues { get; set; }
        public StockInfo Stock { get; set; }
    }

    public class UpdateVariantDto
    {
        public string? ProductCode { get; set; }
        public decimal OriginalPrice { get; set; }
        public decimal? SalePrice { get; set; }
        public Dictionary<string, string>? VariantValues { get; set; }
        public StockUpdateDto? Stock { get; set; }
    }

    public class ProductItemDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string? ProductCode { get; set; }
        public decimal OriginalPrice { get; set; }
        public decimal? SalePrice { get; set; }
        public Dictionary<string, string> VariantValues { get; set; }
        public int? ImageId { get; set; }
        public StockDto? Stock { get; set; }
    }
}
