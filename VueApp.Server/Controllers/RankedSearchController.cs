using Microsoft.AspNetCore.Mvc;
using VueApp.Server.Services;

namespace VueApp.Server.Controllers;

[ApiController]
[Route("[controller]")]
public class RankedSearchController : ControllerBase
{
    private readonly ILogger<RankedSearchController> _logger;
    private readonly IRankedSearchService _rankedSearchService;

    public RankedSearchController(ILogger<RankedSearchController> logger, IRankedSearchService rankedSearchService)
    {
        _logger = logger;
        _rankedSearchService = rankedSearchService;
    }

    [HttpGet(Name = "GetRankedSearch")]
    public async Task<RankedSearchResult> Get(string searchTerm, string rankUrl)
    {
        // TODO handle errors from the service layer, and return 400 and content of the error
        return await _rankedSearchService.FindSearchResults(searchTerm, rankUrl);
    }
}