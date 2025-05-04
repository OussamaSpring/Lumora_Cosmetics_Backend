using Application.DTOs.Product;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Entities.ProductRelated;
using Domain.Enums.Product;


namespace Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<ProductDto> CreateProductAsync(CreateProductDto dto)
        {
            var product = new Product
            {
                ShopId = dto.ShopId,
                Name = dto.Name,
                Brand = dto.Brand,
                About = dto.About,
                Ingredients = dto.Ingredients,
                HowToUse = dto.HowToUse,
                Gender = (Gender)dto.Gender,
                CategoryId = (short)dto.CategoryId,
                Status = ProductStatus.Draft,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow
            };

            product.Id = await _productRepository.CreateProductAsync(product);
            return MapToProductDto(product);
        }

        public async Task<ProductDto?> GetProductByIdAsync(int productId)
        {
            var product = await _productRepository.GetProductByIdAsync(productId);
            return product == null ? null : MapToProductDto(product);
        }

        public async Task<IEnumerable<ProductBriefDto>> GetProductsByShopAsync(int shopId)
        {
            var products = await _productRepository.GetProductsByShopAsync(shopId);
            var result = new List<ProductBriefDto>();

            foreach (var product in products)
            {
                result.Add(new ProductBriefDto
                {
                    Id = product.Id,
                    Name = product.Name,
                    Brand = product.Brand,
                    MainImageUrl = product.ImageUrl,
                    Status = product.Status
                });
            }

            return result;
        }

        public async Task<bool> UpdateProductAsync(UpdateProductDto dto)
        {
            var product = await _productRepository.GetProductByIdAsync(dto.Id);
            if (product == null) return false;

            product.Name = dto.Name ?? product.Name;
            product.Brand = dto.Brand ?? product.Brand;
            product.About = dto.About ?? product.About;
            product.Ingredients = dto.Ingredients ?? product.Ingredients;
            product.HowToUse = dto.HowToUse ?? product.HowToUse;
            product.Gender = Gender.Female;
            product.CategoryId = (short)(dto.CategoryId ?? product.CategoryId);
            product.UpdateDate = DateTime.UtcNow;

            return await _productRepository.UpdateProductAsync(product);
        }

        public async Task<bool> DeleteProductAsync(int productId)
        {
            return await _productRepository.DeleteProductAsync(productId);
        }

        public async Task<ProductItemDto> CreateProductItemAsync(CreateVariantDto dto)
        {
            var item = new ProductItem
            {
                ProductId = dto.ProductId,
                ProductCode = dto.ProductCode,
                OriginalPrice = dto.OriginalPrice,
                SalePrice = dto.SalePrice,
                Variants = dto.VariantValues,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow,
                Stock = new Stock
                {
                    StockQuantity = dto.Stock.StockQuantity,
                    Status = StockStatus.In_Stock,
                    LastRestockDate = DateTime.UtcNow
                }
            };

            item.Id = await _productRepository.AddProductItemAsync(item);
            return MapToProductItemDto(item);
        }

        public async Task<ProductItemDto?> GetProductItemByIdAsync(int productItemId)
        {
            var item = await _productRepository.GetProductItemByIdAsync(productItemId);
            return item == null ? null : MapToProductItemDto(item);
        }

        public async Task<IEnumerable<ProductItemDto>> GetProductItemsByProductIdAsync(int productId)
        {
            var items = await _productRepository.GetProductItemsByProductIdAsync(productId);
            var result = new List<ProductItemDto>();

            foreach (var item in items)
            {
                result.Add(MapToProductItemDto(item));
            }

            return result;
        }

        public async Task<bool> UpdateProductItemAsync(int itemId, UpdateVariantDto dto)
        {
            var item = await _productRepository.GetProductItemByIdAsync(itemId);
            if (item == null) return false;

            item.ProductCode = dto.ProductCode ?? item.ProductCode;
            item.OriginalPrice = dto.OriginalPrice;
            item.SalePrice = dto.SalePrice;
            item.Variants = dto.VariantValues ?? item.Variants;
            item.UpdateDate = DateTime.UtcNow;

            if (item.Stock != null && dto.Stock != null)
            {
                item.Stock.StockQuantity = (short)dto.Stock.StockQuantity;
                item.Stock.Status = dto.Stock.Status ?? item.Stock.Status;
            }

            return await _productRepository.UpdateProductItemAsync(item);
        }

        public async Task<bool> DeleteProductItemAsync(int productItemId)
        {
            return await _productRepository.DeleteProductItemAsync(productItemId);
        }

        public async Task<ProductImageDto> AddProductImageAsync(AddImageDto dto)
        {
            var image = new ProductImage
            {
                ProductId = (int)dto.ProductId,
                Url = dto.Url,
                Title = dto.Title,
                AlternateText = dto.AlternateText,
                IsPrimary = dto.IsPrimary,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow
            };

            image.Id = await _productRepository.AddProductImageAsync(image);
            return MapToProductImageDto(image);
        }

        public async Task<ProductImageDto?> GetProductImageByIdAsync(int imageId)
        {
            var image = await _productRepository.GetProductImageByIdAsync(imageId);
            return image == null ? null : MapToProductImageDto(image);
        }

        public async Task<bool> DeleteProductImageAsync(int imageId)
        {
            return await _productRepository.DeleteProductImageAsync(imageId);
        }

        public async Task<int> AddProductItemImageAsync(int productItemId, AddImageDto dto)
        {
            var image = new ProductImage
            {
                Url = dto.Url,
                Title = dto.Title,
                AlternateText = dto.AlternateText,
                IsPrimary = dto.IsPrimary,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow
            };

            return await _productRepository.AddProductItemImageAsync(productItemId, image);
        }

        public async Task<bool> UpdateProductStatusAsync(int productId, ProductStatus status)
        {
            var product = await _productRepository.GetProductByIdAsync(productId);
            if (product == null) return false;

            product.Status = status;
            return await _productRepository.UpdateProductAsync(product);
        }

        public async Task<bool> UpdateStockStatusAsync(int productItemId, StockStatus status)
        {
            var item = await _productRepository.GetProductItemByIdAsync(productItemId);
            if (item?.Stock == null) return false;

            item.Stock.Status = status;
            return await _productRepository.UpdateProductItemAsync(item);
        }

        private static ProductDto MapToProductDto(Product product) => new()
        {
            Id = product.Id,
            ShopId = product.ShopId,
            Name = product.Name,
            Brand = product.Brand,
            About = product.About,
            Ingredients = product.Ingredients,
            HowToUse = product.HowToUse,
            Gender = (Domain.Enums.Account.Gender)product.Gender,
            CategoryId = product.CategoryId,
            Status = product.Status,
            ImageUrl = product.ImageUrl,
            CreateDate = product.CreateDate,
            UpdateDate = product.UpdateDate
        };

        private static ProductItemDto MapToProductItemDto(ProductItem item) => new()
        {
            Id = item.Id,
            ProductId = item.ProductId,
            ProductCode = item.ProductCode,
            OriginalPrice = item.OriginalPrice,
            SalePrice = item.SalePrice,
            VariantValues = item.Variants ?? new Dictionary<string, string>(),
            ImageId = item.ImageId,
            Stock = item.Stock != null ? new StockDto
            {
                Id = item.Stock.Id,
                StockQuantity = item.Stock.StockQuantity,
                Status = item.Stock.Status,
                LastRestockDate = item.Stock.LastRestockDate
            } : null
        };

        private static ProductImageDto MapToProductImageDto(ProductImage image) => new()
        {
            Id = image.Id,
            ProductId = image.ProductId,
            Url = image.Url,
            Title = image.Title,
            AlternateText = image.AlternateText,
            IsPrimary = image.IsPrimary,
            CreateDate = image.CreateDate,
            UpdateDate = image.UpdateDate
        };
    }
}