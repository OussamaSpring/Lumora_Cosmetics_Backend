using Domain.Enums.enProduct;
namespace Application.DTOs.Product
{
    public class StockDto
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public string Status { get; set; }
    }
}