namespace Domain.Entities.ProductRelated;

public class VariantType
{
    public short Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public short CategoryId { get; set; }
}