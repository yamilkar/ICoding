# ICoding

C# / .NET 10 interview practice: common coding exercises and the backend topics interviewers tend to ask about, each kept small and commented.

```
ICoding
├── SmartApps.slnx
├── src
│   ├── InterviewBasics       # console app: classic algorithm exercises
│   └── InterviewAdvanced     # ASP.NET Core minimal API: LINQ, OOP/SOLID, async, CRUD + DI
└── tests
    └── InterviewAdvanced.Tests   # xUnit unit + integration tests
```

## Requirements

- [.NET 10 SDK](https://dotnet.microsoft.com/download)

```bash
dotnet build SmartApps.slnx
```

---

## InterviewBasics

A console app that runs each exercise and prints the result.

```bash
dotnet run --project src/InterviewBasics
```

| Exercise | File | Approach |
|---|---|---|
| FizzBuzz | `StringExercises.cs` | `yield return` + tuple pattern matching on `(i % 3, i % 5)` |
| Reverse a string | `StringExercises.cs` | Two-pointer swap (O(n)), plus a LINQ one-liner |
| Palindrome | `StringExercises.cs` | Two pointers, skipping punctuation/spaces, case-insensitive |
| Character frequency | `StringExercises.cs` | `Dictionary<char,int>` counting, plus a `GroupBy` version |
| Two Sum | `CollectionExercises.cs` | One pass with a `Dictionary<value, index>`: O(n) vs O(n²) brute force |
| Find duplicates | `CollectionExercises.cs` | `HashSet.Add` returning `false`, plus a `GroupBy` version |
| Group anagrams | `CollectionExercises.cs` | Sorted letters as the dictionary key |

---

## InterviewAdvanced

An ASP.NET Core minimal API, with one area per topic.

```bash
dotnet run --project src/InterviewAdvanced
```

It listens on `http://localhost:5038` (and `https://localhost:7169` with the `https` profile). In Development the OpenAPI document is at `/openapi/v1.json`. `InterviewAdvanced.http` has a ready-made request for every endpoint (you can run them from Visual Studio, Rider or VS Code REST Client).

### 1. LINQ — `Linq/`

Filtering, sorting, grouping and aggregates over a seeded employee list, written as pure functions so they're easy to test. Also shows deferred execution and query syntax.

| Method | Route | Description |
|---|---|---|
| GET | `/employees?department=&minSalary=&sortBySalary=` | Filter + sort |
| GET | `/employees/by-department` | Headcount, average/max salary and top earner per department |

### 2. OOP / SOLID — `Shopping/`

A shopping cart that uses the **Strategy pattern** for discounts. It covers encapsulation, Open/Closed, Single Responsibility and Dependency Inversion.

| Method | Route | Description |
|---|---|---|
| POST | `/cart/quote` | Subtotal, discount and total for a list of cart lines |

Promo codes: `SAVE10` (10% off) and `BIG100` ($15 off orders of $100 or more). Any other code gives no discount.

### 3. async/await — `AsyncDemo/`

A typed `HttpClient` (registered through `IHttpClientFactory`) calls [JSONPlaceholder](https://jsonplaceholder.typicode.com). It fetches the same users one after another and then with `Task.WhenAll`, and returns the timing for each.

| Method | Route | Description |
|---|---|---|
| GET | `/async/users` | Sequential vs parallel timings (needs internet access) |

### 4. CRUD + Dependency Injection — `Products/`

REST endpoints that depend on an `IProductRepository` interface. The in-memory implementation is a thread-safe singleton (`ConcurrentDictionary` + `Interlocked`). Requests are checked with data-annotation validation and responses use `TypedResults`.

| Method | Route | Description |
|---|---|---|
| GET | `/products` | List all |
| GET | `/products/{id}` | Get one (404 if missing) |
| POST | `/products` | Create (400 if invalid) |
| PUT | `/products/{id}` | Update |
| DELETE | `/products/{id}` | Delete |

---

## Tests

```bash
dotnet test SmartApps.slnx
```

`tests/InterviewAdvanced.Tests` uses xUnit:

- **Unit tests:** `EmployeeQueriesTests`, `ShoppingCartTests` (including `[Theory]` cases for discounts) and `ProductRepositoryTests`.
- **Integration tests:** `ProductApiTests` runs the real app in memory with `WebApplicationFactory<Program>` and calls it over HTTP.
