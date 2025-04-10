using NSubstitute;
using VueApp.Server.Services;

namespace VueApp.Test;

[TestFixture]
public class ScraperTests
{
    [Test]
    public async Task Scrape_ShouldPickOutTheUrl_ForANonSponsoredResult()
    {
        // Arrange
        var searcher = Substitute.For<IWebSearch>();
        searcher.Search(Arg.Any<string>()).Returns("<div class=\"b_attribution\" u=\"10|5471|4906979192306150|SHhf1pNto8K3b90fhx67AhF106IX0WyY\" tabindex=\"0\"><cite>https://www.gov.uk › search-property-information-land-registry</cite><span class=\"c_tlbxTrg\"><span class=\"c_tlbxH\" h=\"BASE:GENERATIVECAPTIONSHINTSIGNAL\" k=\"SERP,5245.1\"></span></span></div>");

        // Act
        var scraper = new BingScraper(searcher);
        var result = await scraper.ScrapeLinks("test query");

        // Assert
        Assert.That(result.Count, Is.EqualTo(1));
        Assert.That(result[0], Is.EqualTo("https://www.gov.uk"));
    }

    [Test]
    public async Task Scrape_ShouldNotMatch_ForASponsoredLink()
    {
        // Arrange
        var searcher = Substitute.For<IWebSearch>();
        searcher.Search(Arg.Any<string>()).Returns("<div class=\"b_attribution\"><div class=\"b_adurl\" style=\"max-width: 575px\"><cite>landregistry.uk › title-deeds</cite></div><a class=\"b_adcaret\" href=\"javascript:void(0)\" title=\"About our ads\" ce=\"adcc\"><div class=\"infobubble_item\" data-id=\"/control/AdChoiceAjax?ns=SERP&amp;key=5716&amp;id=2&amp;ads=landregistry.uk%2clandregistry.uk%2c8FORTY+LIMITED%2cGB%2cLand+Registry+UK%2chttps%3a%2f%2fadlibrary.ads.microsoft.com%2fadvertiser%2f176004665%2cTrue%2c1\"><span class=\"sw_ddgn\"></span></div></a></div>");

        // Act
        var scraper = new BingScraper(searcher);
        var result = await scraper.ScrapeLinks("test query");

        // Assert
        Assert.That(result.Count, Is.EqualTo(0));
    }

    [Test]
    public async Task Scrape_ShouldWordCorrectly_WithRealSearchResults()
    {
        // Arrange
        var file = Path.Combine(Path.Combine(TestContext.CurrentContext.TestDirectory, "SearchResultBing.txt"));
        var searchResult = await File.ReadAllTextAsync(file);
        var searcher = Substitute.For<IWebSearch>();
        searcher.Search(Arg.Any<string>()).Returns(searchResult);

        // Act
        var scraper = new BingScraper(searcher);
        var result = await scraper.ScrapeLinks("test query");

        // Assert
        Assert.That(result.Count, Is.EqualTo(3));
        Assert.That(result[0], Is.EqualTo("https://www.gov.uk"));
        Assert.That(result[1], Is.EqualTo("https://www.gov.uk"));
        Assert.That(result[2], Is.EqualTo("https://www.gov.uk"));
    }

    [Test]
    public async Task Scrape_ShouldReturnZeroMatches_IfInputStringIsEmpty()
    {
        // Arrange
        var searcher = Substitute.For<IWebSearch>();
        searcher.Search(Arg.Any<string>()).Returns("");

        // Act
        var scraper = new BingScraper(searcher);
        var result = await scraper.ScrapeLinks("test query");

        // Assert
        Assert.That(result.Count, Is.EqualTo(0));
    }
}