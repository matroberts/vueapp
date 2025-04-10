namespace VueApp.Server.Services;

public interface IRankedSearchService
{
    Task<List<int>> FindSearchResults(string searchTerm, string rankUrl);
}

public class RankedSearchService(IScraper scraper) : IRankedSearchService
{
    public async Task<List<int>> FindSearchResults(string searchTerm, string rankUrl)
    {
        var links = await scraper.ScrapeLinks(searchTerm);

        var result = new List<int>();
        for (int i=0; i<links.Count; i++)
        {
            if (links[i]== rankUrl)
                result.Add(i+1);   // Making the search results 1-indexed
        }
        
        if (result.Count == 0)
            result.Add(0);

        return result;
    }
}