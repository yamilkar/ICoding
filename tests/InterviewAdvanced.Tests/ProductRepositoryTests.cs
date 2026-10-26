using InterviewAdvanced.Products;

namespace InterviewAdvanced.Tests;

public class ProductRepositoryTests
{
    // xUnit creates a new class instance per test, so each test gets a fresh repository.
    private readonly InMemoryProductRepository _repo = new();

    [Fact]
    public void Add_AssignsNewId_AndCanBeRetrieved()
    {
        var created = _repo.Add(new ProductRequest("Webcam", 35m, 5));

        Assert.Equal(created, _repo.GetById(created.Id));
    }

    [Fact]
    public void Update_UnknownId_ReturnsNull()
    {
        Assert.Null(_repo.Update(999, new ProductRequest("X", 1m, 1)));
    }

    [Fact]
    public void Delete_RemovesProduct()
    {
        Assert.True(_repo.Delete(1));
        Assert.Null(_repo.GetById(1));
        Assert.False(_repo.Delete(1));
    }
}
