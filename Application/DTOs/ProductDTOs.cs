<<<<<<< HEAD
﻿

using Domain.Entities.ProductRelated;
using Domain.Enums.Product;

namespace Application.DTOs
{
    class ProductDTO
    {
    }

    public class ProductSearchCriteria
    {
        public string? SearchTerm { get; set; }
        public List<short>? CategoryIds { get; set; }
        public Gender? Genders { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public ProductSortBy SortBy { get; set; } = ProductSortBy.Relevance;

        /* When this is true, it takes the filter types into accounts (category, gender...etc)
         * Otherwise (if the filters are null), IncludeFacets = false and they won't take into consideration
         */
        public bool IncludeFacets { get; set; } = true; 
    }

    public enum ProductSortBy
    {
        Relevance,
        PriceAsc,
        PriceDesc,
        Newest
    }


    public class ProductSearchResult
    {
        public List<Product> Products { get; set; } = new List<Product>();
        public int TotalCount { get; set; }
        public List<CategoryFacet>? CategoryFacets { get; set; }
        public List<PriceRangeFacet>? PriceRangeFacets { get; set; }
    }


    public class CategoryFacet
    {
        public short CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int ProductCount { get; set; }
    }

    public class PriceRangeFacet
    {
        public string Range { get; set; }
        public int Count { get; set; }
    }
=======
using Domain.Entities.ProductRelated;
using Domain.Enums.Product;

public class ProductSearchCriteria
{
    public string? SearchTerm { get; set; }
    public List<short>? CategoryIds { get; set; }
    public Gender? Genders { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
>>>>>>> origin/main
}
