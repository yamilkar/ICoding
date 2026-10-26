using Microsoft.AspNetCore.Http.HttpResults;

namespace InterviewAdvanced.Products;

public static class ProductEndpoints
{
    public static RouteGroupBuilder MapProductEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/products").WithTags("Products");

        // IProductRepository is injected per request by the DI container.
        group.MapGet("/", (IProductRepository repo) => TypedResults.Ok(repo.GetAll()));

        group.MapGet("/{id:int}", Results<Ok<Product>, NotFound> (int id, IProductRepository repo) =>
            repo.GetById(id) is { } product
                ? TypedResults.Ok(product)
                : TypedResults.NotFound());

        group.MapPost("/", (ProductRequest request, IProductRepository repo) =>
        {
            var created = repo.Add(request);
            return TypedResults.Created($"/products/{created.Id}", created);
        });

        group.MapPut("/{id:int}", Results<Ok<Product>, NotFound> (int id, ProductRequest request, IProductRepository repo) =>
            repo.Update(id, request) is { } updated
                ? TypedResults.Ok(updated)
                : TypedResults.NotFound());

        group.MapDelete("/{id:int}", Results<NoContent, NotFound> (int id, IProductRepository repo) =>
            repo.Delete(id)
                ? TypedResults.NoContent()
                : TypedResults.NotFound());

        return group;
    }
}
