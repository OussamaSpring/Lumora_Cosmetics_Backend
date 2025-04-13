namespace Domain.Entities.ProductRelated;

public class ProductItem
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public string ProductCode { get; set; }
    public decimal OriginalPrice { get; set; }
    public decimal? SalePrice { get; set; }
    public DateTime CreateDate { get; set; }
    public DateTime UpdateDate { get; set; }
    public string ItemVariants { get; set; } // I'm not sure of the type as it is stored as JSON in the database
    public int ImageId { get; set; } // I still not sure how to do this (need discussion)
    public int StockId { get; set; }

    // Navigation properties
    public Stock Stock { get; set; }
    public ProductImage ProductImage { get; set; }
}