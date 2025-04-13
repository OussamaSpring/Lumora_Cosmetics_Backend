namespace Domain.Entities.ProductRelated;

public class ProductItem
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public string? ProductCode { get; set; }
    public decimal OriginalPrice { get; set; }
    public decimal? SalePrice { get; set; }
    public DateTime CreateDate { get; set; }
    public DateTime UpdateDate { get; set; }

    /* for JSON (I used strings for easy implementation)
     * NOTE: Key string is name of the variant type     
     */
    public Dictionary<string, string>? Variants { get; set; } 
    public int? ImageId { get; set; }
    public Stock? Stock { get; set; }
}