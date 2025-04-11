namespace VueApp.Server.Services;

public interface IRankedSearchService
{
    Task<RankedSearchResult> FindSearchResults(string searchTerm, string rankUrl);
}

// TODO this object can be generalised into the result pattern Result<T> 
// which would be an object containing the success return value <T>, or the error information
// see e.g. https://www.milanjovanovic.tech/blog/functional-error-handling-in-dotnet-with-the-result-pattern 
public class RankedSearchResult
{
    public bool IsValid => string.IsNullOrWhiteSpace(Error) == false;
    public string Error { get; } = "";
    public List<int> Ranks { get; } = [];
    public List<string> Links { get; } = [];
}

public class RankedSearchService(IScraper scraper) : IRankedSearchService
{
    public async Task<RankedSearchResult> FindSearchResults(string searchTerm, string rankUrl)
    {
        // TODO validate the input
        // mandatoryness of searchTerm and rankUrl currently handled by the framework
        // rankUrl should be validated that it is a url

        var result = new RankedSearchResult();
        
        var links = await scraper.ScrapeLinks(searchTerm);
        result.Links.AddRange(links);

        // TODO...trim trailing / from rankUrl
        // TODO...contains rather than equals so http://example.com would match on a result of http://example.com/my-page
        for (int i=0; i<links.Count; i++)
        {
            if (links[i]== rankUrl)
                result.Ranks.Add(i+1);   // Making the search results 1-indexed
        }
        
        if (result.Ranks.Count == 0)
            result.Ranks.Add(0);

        return result;
    }
}