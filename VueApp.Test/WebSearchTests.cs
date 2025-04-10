using VueApp.Server.Services;

namespace VueApp.Test;

[TestFixture]
public class WebSearchTests
{
    [Test]
    [Ignore("RealTest")]
    public async Task Call_GoogleSearch_ForReal()
    {
        var google = new GoogleSearch();
        var result = await google.Search("");
        Console.WriteLine(result);

    }

    [Test]
    [Ignore("RealTest")]
    public async Task Call_BingSearch_ForReal()
    {
        var bing = new BingSearch();
        var result = await bing.Search("land registry search");
        Console.WriteLine(result);
    }
}