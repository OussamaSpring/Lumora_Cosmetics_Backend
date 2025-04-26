using Application.DTOs;
using Application.DTOs.Product;
using Application.Interfaces.Services;
using Domain.Enums.Product;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        // POST: api/products
        [HttpPost]
        public async Task<ActionResult<ProductDto>> CreateProduct([FromBody] CreateProductDto dto)
        {
            var product = await _productService.CreateProductAsync(dto);
            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
        }

        // GET: api/products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetProduct(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            return product == null ? NotFound() : Ok(product);
        }

        // GET: api/products/shop/5
        [HttpGet("shop/{shopId}")]
        public async Task<ActionResult<IEnumerable<ProductBriefDto>>> GetProductsByShop(int shopId)
        {
            var products = await _productService.GetProductsByShopAsync(shopId);
            return Ok(products);
        }

        // PUT: api/products/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] UpdateProductDto dto)
        {
            if (id != dto.Id)
            {
                return BadRequest("ID mismatch");
            }

            var result = await _productService.UpdateProductAsync(dto);
            return result ? NoContent() : NotFound();
        }

        // DELETE: api/products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var result = await _productService.DeleteProductAsync(id);
            return result ? NoContent() : NotFound();
        }

        // POST: api/products/5/items
        [HttpPost("{productId}/items")]
        public async Task<ActionResult<ProductItemDto>> AddProductItem(
            int productId,
            [FromBody] CreateVariantDto dto) // You may want to rename CreateVariantDto too
        {
            if (productId != dto.ProductId)
            {
                return BadRequest("Product ID mismatch");
            }

            var item = await _productService.CreateProductItemAsync(dto);
            return CreatedAtAction(
                nameof(GetProductItem),
                new { productId, itemId = item.Id },
                item);
        }

        // GET: api/products/5/items/10
        [HttpGet("{productId}/items/{itemId}")]
        public async Task<ActionResult<ProductItemDto>> GetProductItem(int productId, int itemId)
        {
            var item = await _productService.GetProductItemByIdAsync(itemId);

            if (item == null || item.ProductId != productId)
            {
                return NotFound();
            }

            return Ok(item);
        }

        // GET: api/products/5/items
        [HttpGet("{productId}/items")]
        public async Task<ActionResult<IEnumerable<ProductItemDto>>> GetProductItems(int productId)
        {
            var items = await _productService.GetProductItemsByProductIdAsync(productId);
            return Ok(items);
        }

        // PUT: api/products/5/items/10
        [HttpPut("{productId}/items/{itemId}")]
        public async Task<IActionResult> UpdateProductItem(
            int productId,
            int itemId,
            [FromBody] UpdateVariantDto dto)
        {

            var result = await _productService.UpdateProductItemAsync(itemId,dto);
            return result ? NoContent() : NotFound();
        }

        // DELETE: api/products/5/items/10
        [HttpDelete("{productId}/items/{itemId}")]
        public async Task<IActionResult> DeleteProductItem(int productId, int itemId)
        {
            var result = await _productService.DeleteProductItemAsync(itemId);
            return result ? NoContent() : NotFound();
        }

        // POST: api/products/5/images
        [HttpPost("{productId}/images")]
        public async Task<ActionResult<ProductImageDto>> AddProductImage(
            int productId,
            [FromForm] AddImageDto dto)
        {
            dto.ProductId = productId;
            var image = await _productService.AddProductImageAsync(dto);
            return CreatedAtAction(
                nameof(GetProductImage),
                new { productId, imageId = image.Id },
                image);
        }

        // GET: api/products/5/images/10
        [HttpGet("{productId}/images/{imageId}")]
        public async Task<ActionResult<ProductImageDto>> GetProductImage(int productId, int imageId)
        {
            var image = await _productService.GetProductImageByIdAsync(imageId);

            if (image == null || image.ProductId != productId)
            {
                return NotFound();
            }

            return Ok(image);
        }

        // POST: api/products/5/items/10/images
        [HttpPost("{productId}/items/{itemId}/images")]
        public async Task<ActionResult> AddProductItemImage(
            int productId,
            int itemId,
            [FromForm] AddImageDto dto)
        {
            var imageId = await _productService.AddProductItemImageAsync(itemId, dto);
            return CreatedAtAction(
                nameof(GetProductImage),
                new { productId, imageId },
                new { ImageId = imageId });
        }

        // DELETE: api/products/images/10
        [HttpDelete("images/{imageId}")]
        public async Task<IActionResult> DeleteImage(int imageId)
        {
            var result = await _productService.DeleteProductImageAsync(imageId);
            return result ? NoContent() : NotFound();
        }

        // PATCH: api/products/5/status
        [HttpPatch("{productId}/status")]
        public async Task<IActionResult> UpdateProductStatus(
            int productId,
            [FromBody] ProductStatusUpdateRequest request)
        {
            var result = await _productService.UpdateProductStatusAsync(productId, request.Status);
            return result ? NoContent() : NotFound();
        }

        // PATCH: api/products/items/10/stock-status
        [HttpPatch("items/{itemId}/stock-status")]
        public async Task<IActionResult> UpdateStockStatus(
            int itemId,
            [FromBody] StockStatusUpdateRequest request)
        {
            var result = await _productService.UpdateStockStatusAsync(itemId, request.Status);
            return result ? NoContent() : NotFound();
        }
    }

    public record ProductStatusUpdateRequest(ProductStatus Status);
    public record StockStatusUpdateRequest(StockStatus Status);
}
