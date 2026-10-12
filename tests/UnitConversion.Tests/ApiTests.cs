using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using UnitConversion.Api.Dtos;

namespace UnitConversion.Tests;

public class ApiTests(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task Convert_ReturnsResult()
    {
        var response = await _client.GetAsync("/api/conversions?value=100&from=celsius&to=fahrenheit");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var body = await response.Content.ReadFromJsonAsync<ConversionResponse>();
        Assert.Equal(212m, body!.Result);
        Assert.Equal("temperature", body.Category);
    }

    [Theory]
    [InlineData("/api/conversions?value=1&from=parsec&to=m", HttpStatusCode.NotFound)]
    [InlineData("/api/conversions?value=1&from=kg&to=m", HttpStatusCode.BadRequest)]
    [InlineData("/api/conversions?value=abc&from=m&to=ft", HttpStatusCode.BadRequest)]
    [InlineData("/api/conversions?value=1&from=m", HttpStatusCode.BadRequest)]
    public async Task Convert_InvalidRequest_ReturnsProblem(string url, HttpStatusCode expected)
    {
        var response = await _client.GetAsync(url);

        Assert.Equal(expected, response.StatusCode);
        Assert.Equal("application/problem+json", response.Content.Headers.ContentType?.MediaType);
    }

    [Fact]
    public async Task Units_CanBeFilteredByCategory()
    {
        var units = await _client.GetFromJsonAsync<List<UnitResponse>>("/api/units?category=mass");

        Assert.NotEmpty(units!);
        Assert.All(units!, u => Assert.Equal("mass", u.Category));
    }

    [Fact]
    public async Task Categories_ListsAllThree()
    {
        var categories = await _client.GetFromJsonAsync<List<CategoryResponse>>("/api/categories");

        Assert.Equivalent(new[] { "length", "temperature", "mass" }, categories!.Select(c => c.Id));
    }
}
