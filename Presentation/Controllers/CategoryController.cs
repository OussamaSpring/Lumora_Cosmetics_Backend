using Application.DTOs.Category;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryService;
    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetCategoryById([FromRoute] int id)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("model fault");
        }

        var result = await _categoryService.GetCategoryByIdAsync(id);
        Console.WriteLine(result.IsSuccess ? result.Value : result.Error);

        return result.IsSuccess ? Ok(result.Value) : NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> CreateCategory(CreateCategoryRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest("bad request");

        var result = await _categoryService.CreateCategoryAsync(request);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteCategory([FromRoute] int id)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("model fault");
        }

        var result = await _categoryService.DeleteCategoryAsync(id);
        return result.IsSuccess ? Ok(result.Value) : NotFound();
    }


    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateCategory([FromRoute] int id, UpdateCategoryRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("model fault");
        }

        var result = await _categoryService.UpdateCategoryAsync(id, request);
        return result.IsSuccess ? Ok(result.Value) : NotFound();
    }

    [HttpGet]
    public async Task<IActionResult> GetAllCategory()
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("model fault");
        }

        var result = await _categoryService.GetAllCategoryAsync();
        return result.IsSuccess ? Ok(result.Value) : NotFound();
    }
}
