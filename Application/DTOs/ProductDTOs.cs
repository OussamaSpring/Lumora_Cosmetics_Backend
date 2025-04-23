using Domain.Entities.ProductRelated;
using Domain.Enums.Product;

public class ProductSearchCriteria
{
    public string? SearchTerm { get; set; }
    public List<short>? CategoryIds { get; set; }
    public Gender? Genders { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
}
