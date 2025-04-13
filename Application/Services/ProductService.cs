using Application.DTOs.Product;
using Application.Interfaces;
using Domain;
using Domain.Entities.ProductRelated;
using Microsoft.Extensions.Logging;
using Domain.Enums.enProduct;
using System.Text.Json;

namespace Application.Services
{
    public sealed class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger<ProductService> _logger;

        public ProductService(IProductRepository productRepository, ILogger<ProductService> logger)
        {
            _productRepository = productRepository;
            _logger = logger;
        }

        public async Task<ProductDto> CreateProductAsync(CreateProductDto dto)
        {
            ArgumentNullException.ThrowIfNull(dto);

            try
            {
                var product = new Product
                {
                    Name = dto.Name,
                    Brand = dto.Brand,
                    About = dto.About,
                    Ingredients = dto.Ingredients,
                    HowToUse = dto.HowToUse,
                    Gender = dto.Gender,
                    CategoryId = dto.CategoryId,
                    Status = dto.Status,
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow
                };

                var productItems = dto.ProductItems.Select(item => new ProductItem
                {
                    ProductCode = item.ProductCode,
                    OriginalPrice = item.OriginalPrice,
                    SalePrice = item.SalePrice,
                    ItemVariants = item.ItemVariants,
                    ImageId = item.ImageId,
                    StockId = item.StockId,
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow
                }).ToList();

                await _productRepository.CreateWithItemsAsync(product, productItems);

                return MapToDto(product);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while creating product with items: {ex.Message}");
                throw new ProductServiceException("An error occurred while creating the product and its items.", ex);
            }
        }

        public async Task<ProductDto> UpdateProductAsync(int id, UpdateProductDto dto)
        {
            ArgumentNullException.ThrowIfNull(dto);

            // Fetch the product by id from the repository
            var product = await _productRepository.GetByIdAsync(id)
                ?? throw new NotFoundException($"Product {id} not found.");

            // Update properties if they are provided, otherwise keep existing values
            product.Name = dto.Name ?? product.Name;
            product.Brand = dto.Brand ?? product.Brand;
            product.About = dto.About ?? product.About;
            product.Ingredients = dto.Ingredients ?? product.Ingredients;
            product.HowToUse = dto.HowToUse ?? product.HowToUse;

            // Handle Gender assignment properly (nullable enum)
            if (dto.Gender != 0)  // assuming 0 is the default or "empty" value for Gender
            {
                product.Gender = dto.Gender;
            }
            product.CategoryId = dto.CategoryId ?? product.CategoryId;
            product.Status = dto.Status ?? product.Status;
            product.UpdateDate = DateTime.UtcNow; // Set the update date

            // Save the updated product in the repository
            await _productRepository.UpdateAsync(product);

            // Return the mapped ProductDto
            return MapToDto(product);
        }

        public async Task<ProductDto> GetProductByIdAsync(int id)
        {
            try
            {
                // Fetch the product with all its related items from the repository
                var product = await _productRepository.GetByIdWithDetailsAsync(id) ?? throw new NotFoundException($"Product {id} not found.");

                // Map to a more detailed DTO that includes all related data
                return new ProductDetailDto
                {
                    Id = product.Id,
                    Name = product.Name,
                    Brand = product.Brand,
                    About = product.About,
                    Ingredients = product.Ingredients,
                    HowToUse = product.HowToUse,
                    Gender = product.Gender,
                    CategoryId = product.CategoryId,
                    Status = product.Status,
                    // Map product items with their details
                    ProductItems = product.ProductItems?.Select(item => new ProductItemDto
                    {
                        Id = item.Id,
                        ProductCode = item.ProductCode,
                        OriginalPrice = item.OriginalPrice,
                        SalePrice = item.SalePrice,
                        ItemVariants = JsonSerializer.Deserialize<List<VariantDto>>(item.ItemVariants),
                        // Map stock information
                        Stock = new StockDto
                        {
                            Id = item.Stock.Id,
                            Quantity = item.Stock.StockQuantity,
                            Status = item.Stock.Status.ToString()
                        },
                        // Map image information
                        Image = new ProductImageDto
                        {
                            Id = item.ImageId,
                            ImageUrl = item.ProductImage.Url,
                            IsPrimary = item.ProductImage.IsPrimary
                        }
                    }).ToList()
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while fetching product with ID {id}: {ex.Message}");
                throw new ProductServiceException($"An error occurred while fetching the product with ID {id}.", ex);
            }
        }

        public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
        {
            // Fetch all products from the repository
            var products = await _productRepository.GetAllAsync();

            // Map the products to ProductDto and return
            return products.Select(MapToDto);
        }

        // ---- Helper Method ----
        private static ProductDto MapToDto(Product product)
        {
            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Brand = product.Brand,
                About = product.About,
                Ingredients = product.Ingredients,
                HowToUse = product.HowToUse,
                Gender = product.Gender,  // Gender is nullable in ProductDto as well
                CategoryId = product.CategoryId,
                Status = product.Status
            };
        }
    }

    // Custom exception for the service layer
    public class ProductServiceException : Exception
    {
        public ProductServiceException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
