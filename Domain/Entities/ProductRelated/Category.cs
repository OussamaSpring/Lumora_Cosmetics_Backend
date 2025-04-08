namespace Domain.Entities.ProductRelated;

public class Category
{
    public short Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public short? ParentId { get; set; }
}
