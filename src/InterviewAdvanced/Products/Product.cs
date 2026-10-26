using System.ComponentModel.DataAnnotations;

namespace InterviewAdvanced.Products;

public record Product(int Id, string Name, decimal Price, int Stock);

// Request DTO: clients never send the Id, the repository assigns it.
public record ProductRequest(
    [property: Required, StringLength(100, MinimumLength = 1)] string Name,
    [property: Range(0.01, 1_000_000)] decimal Price,
    [property: Range(0, int.MaxValue)] int Stock);
