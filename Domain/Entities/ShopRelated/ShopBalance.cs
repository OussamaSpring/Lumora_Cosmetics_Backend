namespace Domain.Entities.ShopRelated;

public class ShopBalance
{
    public int Id { get; set; }
    public decimal AvailableBalance { get; set; }

    public decimal PendingBalance { get; set; }

    public decimal TotalBalance { get; set; }

    public decimal WithdrawnBalance { get; set; }

    public decimal LifeTimeEarning { get; set; }
}
