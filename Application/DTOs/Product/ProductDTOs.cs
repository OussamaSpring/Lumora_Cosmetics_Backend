using Domain.Enums.Account;
using Domain.Enums.Product;

namespace Application.DTOs.Product
{


    public class UpdateProductDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Brand { get; set; }
        public string? About { get; set; }
        public string? Ingredients { get; set; }
        public string? HowToUse { get; set; }
        public Domain.Enums.Account.Gender? Gender { get; set; }
        public int? CategoryId { get; set; }
    }

   

    public class ProductBriefDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Brand { get; set; }
        public string? MainImageUrl { get; set; }
        public ProductStatus Status { get; set; }
    }
}
