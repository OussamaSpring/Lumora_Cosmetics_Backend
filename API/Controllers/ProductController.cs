using Application.Interfaces;
using Application.DTOs.Product;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateProductAsync([FromBody] CreateProductDto dto)
        {
            // Ensure the DTO is valid
            if (dto == null)
            {
                return BadRequest("Product data is required.");
            }

            if (string.IsNullOrWhiteSpace(dto.Name))
            {
                return BadRequest("Product name is required.");
            }

            try
            {
                // Call the service method to create the product
                var product = await _productService.CreateProductAsync(dto);

                // Return a Created response with the created product and its location
                return CreatedAtAction(
                    nameof(GetProductByIdAsync),  // The action to return when querying for the product by id
                    new { id = product.Id },      // The route parameter for the created product's ID
                    product                       // The created product data
                );
            }
            catch (Exception ex)
            {
                // If something goes wrong, return an internal server error response
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProductAsync(int id, [FromBody] UpdateProductDto dto)
        {
            if (dto == null)
            {
                return BadRequest("Product data is required.");
            }

            if (string.IsNullOrWhiteSpace(dto.Name))
            {
                return BadRequest("Product name is required.");
            }

            try
            {
                var product = await _productService.UpdateProductAsync(id, dto);
                if (product == null)
                {
                    return NotFound($"Product with id {id} not found.");
                }

                return Ok(product);
            }
            catch (Exception ex)
            {
                // Log the exception if needed
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductByIdAsync(int id)
        {
            try
            {
                var product = await _productService.GetProductByIdAsync(id);
                if (product == null)
                {
                    return NotFound($"Product with id {id} not found.");
                }

                return Ok(product);
            }
            catch (Exception ex)
            {
                // Log the exception if needed
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProductsAsync()
        {
            try
            {
                var products = await _productService.GetAllProductsAsync();
                return Ok(products);
            }
            catch (Exception ex)
            {
                // Log the exception if needed
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
