using System.Net;
using System.Net.Http.Json;
using InterviewAdvanced.Products;
using Microsoft.AspNetCore.Mvc.Testing;

namespace InterviewAdvanced.Tests;

// Integration tests: spins up the real app in memory and calls it over HTTP.
public class ProductApiTests(WebApplicationFactory<Program> factory)
    : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task Post_ThenGet_ReturnsCreatedProduct()
    {
        var response = await _client.PostAsJsonAsync("/products", new ProductRequest("Desk Lamp", 25m, 3));

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var created = await response.Content.ReadFromJsonAsync<Product>();
        Assert.NotNull(created);

        var fetched = await _client.GetFromJsonAsync<Product>($"/products/{created.Id}");
        Assert.Equal(created, fetched);
    }

    [Fact]
    public async Task Get_UnknownId_Returns404()
    {
        var response = await _client.GetAsync("/products/99999");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Post_InvalidProduct_Returns400()
    {
        var response = await _client.PostAsJsonAsync("/products", new ProductRequest("", -5m, 1));

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}
