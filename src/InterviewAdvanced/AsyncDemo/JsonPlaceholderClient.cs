using System.Diagnostics;

namespace InterviewAdvanced.AsyncDemo;

public record User(int Id, string Name, string Email);

public record FetchResult(string Mode, long ElapsedMs, IReadOnlyList<User> Users);

// Typed HttpClient registered with AddHttpClient<T>, so IHttpClientFactory manages
// the handler lifetime (avoids socket exhaustion from `new HttpClient()` per call).
public class JsonPlaceholderClient(HttpClient http)
{
    public async Task<User?> GetUserAsync(int id, CancellationToken ct = default) =>
        await http.GetFromJsonAsync<User>($"users/{id}", ct);

    // One call after another: total time ~= sum of all calls.
    public async Task<FetchResult> GetUsersSequentialAsync(IEnumerable<int> ids, CancellationToken ct = default)
    {
        var sw = Stopwatch.StartNew();
        var users = new List<User>();
        foreach (var id in ids)
        {
            if (await GetUserAsync(id, ct) is { } user)
                users.Add(user);
        }
        return new FetchResult("sequential", sw.ElapsedMilliseconds, users);
    }

    // Start every task first, then await them together: total time ~= slowest call.
    public async Task<FetchResult> GetUsersParallelAsync(IEnumerable<int> ids, CancellationToken ct = default)
    {
        var sw = Stopwatch.StartNew();
        var tasks = ids.Select(id => GetUserAsync(id, ct));
        var results = await Task.WhenAll(tasks);
        return new FetchResult("parallel (Task.WhenAll)", sw.ElapsedMilliseconds, results.OfType<User>().ToList());
    }
}
