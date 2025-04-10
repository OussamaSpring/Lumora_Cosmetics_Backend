namespace Domain.Entities.ProductRelated;

public class ProductImage
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public string? Title { get; set; }
    public string? AlternateText { get; set; }
    public string Url { get; set; }
    public bool IsPrimary { get; set; } = false;
    public DateTime CreateDate { get; set; }
    public DateTime UpdateDate { get; set; }
}