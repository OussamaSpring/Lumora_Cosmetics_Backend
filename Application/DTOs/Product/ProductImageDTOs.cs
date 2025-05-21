
namespace Application.DTOs.Product
{
    public class AddImageDto
    {
        public int? ProductId { get; set; }
        public string Url { get; set; }
        public string? Title { get; set; }
        public string? AlternateText { get; set; }
        public bool IsPrimary { get; set; }
    }

    public class ProductImageDto
    {
        public int Id { get; set; }
        public int? ProductId { get; set; }
        public string Url { get; set; }
        public string? Title { get; set; }
        public string? AlternateText { get; set; }
        public bool IsPrimary { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
