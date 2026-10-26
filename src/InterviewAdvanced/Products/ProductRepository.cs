using System.Collections.Concurrent;

namespace InterviewAdvanced.Products;

// Endpoints depend on this interface, so the in-memory version can be swapped
// for an EF Core one without changing them.
public interface IProductRepository
{
    IReadOnlyList<Product> GetAll();
    Product? GetById(int id);
    Product Add(ProductRequest request);
    Product? Update(int id, ProductRequest request);
    bool Delete(int id);
}

// Registered as a Singleton so data survives between requests.
// ConcurrentDictionary + Interlocked make it safe when requests run in parallel.
public class InMemoryProductRepository : IProductRepository
{
    private readonly ConcurrentDictionary<int, Product> _products = new();
    private int _nextId;

    public InMemoryProductRepository()
    {
        Add(new ProductRequest("Keyboard", 49.99m, 25));
        Add(new ProductRequest("Mouse", 19.99m, 40));
        Add(new ProductRequest("Monitor", 189.00m, 8));
    }

    public IReadOnlyList<Product> GetAll() => _products.Values.OrderBy(p => p.Id).ToList();

    public Product? GetById(int id) => _products.GetValueOrDefault(id);

    public Product Add(ProductRequest request)
    {
        var id = Interlocked.Increment(ref _nextId);
        var product = new Product(id, request.Name, request.Price, request.Stock);
        _products[id] = product;
        return product;
    }

    public Product? Update(int id, ProductRequest request)
    {
        if (!_products.ContainsKey(id)) return null;

        var updated = new Product(id, request.Name, request.Price, request.Stock);
        _products[id] = updated;
        return updated;
    }

    public bool Delete(int id) => _products.TryRemove(id, out _);
}
