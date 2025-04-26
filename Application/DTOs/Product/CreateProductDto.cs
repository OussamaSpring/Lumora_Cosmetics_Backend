using Domain.Enums.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Product
{
    public class CreateProductDto
    {
        public int ShopId { get; set; }
        public string Name { get; set; }
        public string? Brand { get; set; }
        public string? About { get; set; }
        public string? Ingredients { get; set; }
        public string? HowToUse { get; set; }
        public Gender Gender { get; set; }
        public int CategoryId { get; set; }
    }
}
