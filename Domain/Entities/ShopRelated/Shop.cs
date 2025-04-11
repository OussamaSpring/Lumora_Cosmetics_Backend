namespace Domain.Entities.ShopRelated;

public class Shop
{
    public int Id { get; set; }
    public Guid VendorId { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public string? LogoUrl { get; set; }
    public DateTime CreateDate { get; set; }
    public DateTime UpdateDate { get; set; }
    public string? MapAddress { get; set; }
}
