using Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SearchController : ControllerBase
{
    private readonly ISearchService _searchService;
    public SearchController(ISearchService searchService)
    {
        _searchService = searchService;
    }

    [HttpGet]
    public async Task<IActionResult> Search([FromQuery] ProductSearchCriteria criteria)
    {
        if (!ModelState.IsValid)
            return BadRequest("Invalid Arguments");

        var result = await _searchService.Search(criteria);
        return result.IsSuccess ? Ok(result.Value) : NotFound();
    }

}
