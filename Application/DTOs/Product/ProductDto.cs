using Domain.Enums.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Product
{
    public class ProductDto
    {
        public int Id { get; set; }
        public int ShopId { get; set; }
        public string Name { get; set; }
        public string? Brand { get; set; }
        public string? About { get; set; }
        public string? Ingredients { get; set; }
        public string? HowToUse { get; set; }
        public Domain.Enums.Account.Gender Gender { get; set; }
        public int CategoryId { get; set; }
        public ProductStatus Status { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
