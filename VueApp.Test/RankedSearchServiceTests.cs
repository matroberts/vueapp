using NSubstitute;
using VueApp.Server.Services;

namespace VueApp.Test;

[TestFixture]   
public class RankedSearchServiceTests
{
    [Test]
    public async Task FindSearchResults_ReturnsZero_IfNoMatchesAreFound()
    {
        // Arrange
        var scraper = Substitute.For<IScraper>();
        scraper.ScrapeLinks(Arg.Any<string>()).Returns([]);

        // Act
        var service = new RankedSearchService(scraper);
        var result = await service.FindSearchResults("search query", "http://example.com");

        // Assert
        Assert.That(result.Ranks, Is.EqualTo(new []{0}));
    }

    [Test]
    public async Task FindSearchResults_ReturnsOne_IfTheUrlMatchesTheFirstLink()
    {
        // Arrange
        var scraper = Substitute.For<IScraper>();
        scraper.ScrapeLinks(Arg.Any<string>()).Returns(["http://example.com"]);

        // Act
        var service = new RankedSearchService(scraper);
        var result = await service.FindSearchResults("search query", "http://example.com");

        // Assert
        Assert.That(result.Ranks, Is.EqualTo(new[] { 1 }));
    }

    [Test]
    public async Task FindSearchResults_ReturnsListOfMatchPositions_WhenMoreThanOneMatch()
    {
        // Arrange
        var scraper = Substitute.For<IScraper>();
        scraper.ScrapeLinks(Arg.Any<string>()).Returns(["http://test.com", "http://example.com", "http://test.com", "http://example.com"]);

        // Act
        var service = new RankedSearchService(scraper);
        var result = await service.FindSearchResults("search query", "http://example.com");

        // Assert
        Assert.That(result.Ranks, Is.EqualTo(new[] { 2, 4 }));
    }


}