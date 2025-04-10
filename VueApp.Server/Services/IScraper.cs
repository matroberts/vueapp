using System.Text.RegularExpressions;

namespace VueApp.Server.Services;

public interface IScraper
{
    Task<List<string>> ScrapeLinks(string searchTerm);
}

public class BingScraper(IWebSearch searcher) : IScraper
{
    // 'Parsing' html with regex is madness...the html agility pack would be a much better option
    // However test seems to suggest not using 3rd party tools, so here we are
    public async Task<List<string>> ScrapeLinks(string searchTerm)
    {
        var searchResult = await searcher.Search(searchTerm);
        var result = new List<string>();
        // Find the result <div class="b_attribution"
        foreach (Match match in Regex.Matches(searchResult, "<div class=\"b_attribution\".*?</div>", RegexOptions.None, TimeSpan.FromSeconds(1)))
        {
            // Skip sponsored links
            if (match.Value.Contains("class=\"b_adurl\""))
                continue;

            // Trying to pull out the url from this: <cite>https://www.gov.uk › search-property-information-land-registry</cite>
            // Match on the <cite>...</cite> then pick out the non-whitespace characters which immediately follow the opening tag
            Match url = Regex.Match(match.Value, @"<cite>(\S+).*</cite>", RegexOptions.None, TimeSpan.FromSeconds(1));
            if (url.Success)
            {
                result.Add(url.Groups[1].Value);
            }
        }
        return result;
    }
}