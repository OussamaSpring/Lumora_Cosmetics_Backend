
using Domain.Enums.Product;

namespace Application.DTOs.Product
{

    public class StockInfo
    {
        public short StockQuantity { get; set; }
    }

    public class StockUpdateDto
    {
        public short? StockQuantity { get; set; }
        public StockStatus? Status { get; set; }
    }

    public class StockDto
    {
        public int Id { get; set; }
        public short StockQuantity { get; set; }
        public StockStatus Status { get; set; }
        public DateTime? LastRestockDate { get; set; }
    }
}
