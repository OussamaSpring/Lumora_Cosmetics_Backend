namespace Domain.Entities.ProductRelated;

public class VariantType
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public int CategoryId { get; set; }
}