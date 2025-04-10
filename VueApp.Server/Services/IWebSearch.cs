using Microsoft.AspNetCore.WebUtilities;

namespace VueApp.Server.Services;

public interface IWebSearch
{
    Task<string> Search(string searchTerm);
}

public class GoogleSearch : IWebSearch
{
    private static HttpClient _httpClient = new()
    {
        BaseAddress = new Uri("https://www.google.co.uk/"),
    };

    public async Task<string> Search(string searchTerm)
    {
        // So google search needs javascript enabled to work (see eg https://www.reddit.com/r/google/comments/1fuipwh/googleca_now_requires_java_script_just_to_do_a/)
        // The beginning of the result looks like this:
        // <!DOCTYPE html><html lang="en-GB"><head><title>Google Search</title><style>body{background-color:var(--xhUGwc)}</style></head><body><noscript><style>table,div,span,p{display:none}</style><meta content="0;url=/httpservice/retry/enablejs?sei=Vyr2Z-DpKsXU7M8P1IiTqQQ" http-equiv="refresh"><div style="display:block">Please click <a href="/httpservice/retry/enablejs?sei=Vyr2Z-DpKsXU7M8P1IiTqQQ">here</a> if you are not redirected within a few seconds.</div></noscript><script nonce="gcYgnHoqydl-8MSXOc2gLw">//# sourceMappingURL=data:application/
        //
        // Think that means cannot use HttpClient for this, and would need a headless browser solution like selenium (https://www.selenium.dev/) or puppeteer (https://pptr.dev/)
        // That doesn't seem to be the goal of the test, so use bing instead
        _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/135.0.0.0 Safari/537.36 Edg/135.0.0.0");
        var result = await _httpClient.GetAsync("/search?num=100&q=land+registry+search");
        var content = await result.Content.ReadAsStringAsync();
        return content;
    }
}

public class BingSearch : IWebSearch
{
    private static HttpClient _httpClient = new()
    {
        BaseAddress = new Uri("https://www.bing.com/"),
    };

    public async Task<string> Search(string searchTerm)
    {
        // TODO: How set number of search results?
        // https://stackoverflow.com/questions/41523186/how-to-set-the-number-of-bing-search-result-page-to-50-in-cookie

        // TODO: Handle failure

        // Think need to set the user-agent header to mimic a browser, see https://www.zenrows.com/blog/c-sharp-httpclient-user-agent#set-a-custom-ua-in-http
        _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/135.0.0.0 Safari/537.36 Edg/135.0.0.0");
        var query = new Dictionary<string, string?> { ["q"] = searchTerm };
        var result = await _httpClient.GetAsync(QueryHelpers.AddQueryString("/search/", query));
        var content = await result.Content.ReadAsStringAsync();
        return content;
    }
}