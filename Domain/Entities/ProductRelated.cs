
using Domain.Enums.enProduct;

namespace Domain.Entities.ProductRelated
{
    public class Category
    {
        public short Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public short? ParentId { get; set; }
    }

    public class VariantType
    {
        public short Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public short CategoryId { get; set; }
    }

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
        public Gender Gender { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public short CategoryId { get; set; }
        public ProductStatus Status { get; set; }
    }

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

    public class Stock
    {
        public int Id { get; set; }
        public short StockQuantity { get; set; }
        public short SelledItemsNbr { get; set; }
        public short ReservedItemsNbr { get; set; } 
        public float? ItemLength { get; set; }
        public float? ItemWidth { get; set; }
        public float? ItemHeight { get; set; }
        public float? ItemWeight { get; set; }
        public float? DiscountRate { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public DateTime? LastRestockDate { get; set; }
        public StockStatus Status { get; set; }
    }

    public class ProductItem
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductCode { get; set; }
        public decimal OriginalPrice { get; set; }
        public decimal? SalePrice { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string ItemVariants { get; set; } // I'm not sure of the type as it is stored as JSON in the database
        public int ImageId { get; set; } // I still not sure how to do this (need discussion)
        public int StockId { get; set; }
    }
}
