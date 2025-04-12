namespace Application.DTOs.ProductItem;

public class CreateProductItemDto
{
    public string ProductCode { get; set; }
    public decimal OriginalPrice { get; set; }
    public decimal? SalePrice { get; set; }
    public string ItemVariants { get; set; }
    public int ImageId { get; set; }
    public int StockId { get; set; }
}