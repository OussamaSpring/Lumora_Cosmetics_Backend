

using Domain.Enums;
using Domain.Enums.Product;

namespace Domain.Entities.ProductRelated;

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