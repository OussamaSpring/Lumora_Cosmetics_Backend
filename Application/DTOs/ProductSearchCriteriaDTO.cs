


using Domain.Enums.Product;

namespace Application.DTOs;

public class ProductSearchCriteriaDTO
{
    public string? SearchTerm { get; set; }
    public List<short>? CategoryIds { get; set; } // Accept multiple category filters
    public Gender? Genders { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
}
