using InterviewAdvanced.AsyncDemo;
using InterviewAdvanced.Linq;
using InterviewAdvanced.Products;
using InterviewAdvanced.Shopping;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddValidation(); // honours the [Required]/[Range] attributes on ProductRequest

// DI lifetimes: Singleton = one instance for the app, Scoped = one per request,
// Transient = new instance every time it is resolved.
builder.Services.AddSingleton<IProductRepository, InMemoryProductRepository>();
builder.Services.AddHttpClient<JsonPlaceholderClient>(client =>
{
    client.BaseAddress = new Uri("https://jsonplaceholder.typicode.com/");
    client.Timeout = TimeSpan.FromSeconds(10);
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// 1. LINQ
var employees = app.MapGroup("/employees").WithTags("LINQ");

employees.MapGet("/", (string? department, decimal? minSalary, bool? sortBySalary) =>
    EmployeeQueries.FilterAndSort(EmployeeData.Seed, department, minSalary, sortBySalary ?? true));

employees.MapGet("/by-department", () =>
    EmployeeQueries.SummarizeByDepartment(EmployeeData.Seed));

// 2. OOP / SOLID: quote a cart with an optional promo code (SAVE10, BIG100)
app.MapPost("/cart/quote", (CartQuoteRequest request) =>
{
    var cart = new ShoppingCart(DiscountFactory.FromCode(request.PromoCode));
    foreach (var line in request.Lines)
    {
        cart.AddItem(new CartProduct(line.Sku, line.Name, line.UnitPrice), line.Quantity);
    }
    return new { cart.Subtotal, cart.Discount, cart.Total, ItemCount = cart.Items.Count };
}).WithTags("OOP");

// 3. async/await: compare sequential vs Task.WhenAll
app.MapGet("/async/users", async (JsonPlaceholderClient client, CancellationToken ct) =>
{
    int[] ids = [1, 2, 3, 4, 5];
    var sequential = await client.GetUsersSequentialAsync(ids, ct);
    var parallel = await client.GetUsersParallelAsync(ids, ct);
    return new[] { sequential, parallel };
}).WithTags("Async");

// 4. CRUD with DI
app.MapProductEndpoints();

app.Run();

public record CartLine(string Sku, string Name, decimal UnitPrice, int Quantity);
public record CartQuoteRequest(List<CartLine> Lines, string? PromoCode);

// Lets WebApplicationFactory<Program> in the test project find the entry point.
public partial class Program;
