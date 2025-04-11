using Domain.Enums.enProduct;


namespace Domain.Entities.ProductRelated;
public class Product
{
    public int Id { get; set; }
    public int ShopId { get; set; }
    public string? SerialNumber { get; set; }
    public string Name { get; set; }
    public string? Brand { get; set; }
    public string? About { get; set; }
    public string? Ingredients { get; set; }
    public string? HowToUse { get; set; }
    public Gender? Gender { get; set; }  // Change from Gender to Gender?
    public DateTime CreateDate { get; set; }
    public DateTime UpdateDate { get; set; }
    public short CategoryId { get; set; }
    public short StatusId { get; set; }
    public ProductStatus Status { get; set; }
}