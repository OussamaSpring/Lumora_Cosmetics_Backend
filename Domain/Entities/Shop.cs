

namespace Domain.Entities
{
    public class Shop
    {
        public int Id { get; set; }
        public Guid VendorId { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? LogoUrl { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string? MapAddress { get; set; }
    }

    public class Shop_Balance
    {
        public int Id { get; set; }
        public decimal AvailableBalance { get; set; }

        public decimal PendingBalance { get; set; }

        public decimal TotalBalance { get; set; }

        public decimal WithdrawnBalance { get; set; }

        public decimal LifeTimeEarning { get; set; }
    }
}
