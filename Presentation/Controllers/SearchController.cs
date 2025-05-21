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

    [HttpPost]
    public async Task<IActionResult> Search([FromBody] ProductSearchCriteria criteria)
    {
        var products = await _searchService.Search(criteria);
        //if (products.Count == 0)
        //    return NotFound();

        return Ok(products);
    }

}
