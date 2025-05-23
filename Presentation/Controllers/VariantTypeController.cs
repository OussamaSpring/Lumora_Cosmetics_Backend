using Application.DTOs.VarianteType;
using Application.DTOs.VariantType;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VariantTypeController : ControllerBase
{
    private readonly IVariantTypeService _variantTypeService;
    public VariantTypeController(IVariantTypeService variantTypeService)
    {
        _variantTypeService = variantTypeService;
    }

    [HttpGet("{variantTypeId:int}")]
    public async Task<IActionResult> GetVariantType(int variantTypeId)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _variantTypeService.GetVariantTypeByIdAsync(variantTypeId);
        return result.IsSuccess ? Ok(result.Value) : NotFound();
    }

    [HttpPost("{categoryId:int}")]
    public async Task<IActionResult> AddVariantType(
        [FromRoute] int categoryId,
        CreateVariantTypeRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _variantTypeService.CreateVariantTypeAsync(categoryId, request);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error.description);
    }

    [HttpDelete("{variantTypeId:int}")]
    public async Task<IActionResult> DeleteVariantType([FromRoute] int variantTypeId)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _variantTypeService.DeleteVariantType(variantTypeId);
        return result.IsSuccess ? NoContent() : BadRequest(result.Error.description);
    }

    [HttpGet("category/{categoryId:int}")]
    public async Task<IActionResult> GetVariantTypeForCategory([FromRoute] int categoryId)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _variantTypeService.GetVariantTypesForCategoryAsync(categoryId);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error.description);
    }

    [HttpPut("{variantTypeId:int}")]
    public async Task<IActionResult> UpdateVariantType(
        [FromRoute] int variantTypeId,
        UpdateVariantTypeRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _variantTypeService.UpdateVariantAsync(variantTypeId, request);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error.description);
    }
}
