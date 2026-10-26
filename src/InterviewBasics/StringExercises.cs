using System.Text;

namespace InterviewBasics;

public static class StringExercises
{
    // FizzBuzz: multiples of 3 -> "Fizz", of 5 -> "Buzz", of both -> "FizzBuzz".
    public static IEnumerable<string> FizzBuzz(int n)
    {
        for (var i = 1; i <= n; i++)
        {
            yield return (i % 3, i % 5) switch
            {
                (0, 0) => "FizzBuzz",
                (0, _) => "Fizz",
                (_, 0) => "Buzz",
                _ => i.ToString()
            };
        }
    }

    // Two-pointer swap: O(n) time, one char[] allocation.
    public static string Reverse(string input)
    {
        var chars = input.ToCharArray();
        for (int left = 0, right = chars.Length - 1; left < right; left++, right--)
        {
            (chars[left], chars[right]) = (chars[right], chars[left]);
        }
        return new string(chars);
    }

    // Alternative people often show: built-in helpers.
    public static string ReverseWithLinq(string input) => new(input.Reverse().ToArray());

    // Ignores case, spaces and punctuation: "A man, a plan, a canal: Panama" -> true.
    public static bool IsPalindrome(string input)
    {
        int left = 0, right = input.Length - 1;
        while (left < right)
        {
            if (!char.IsLetterOrDigit(input[left])) { left++; continue; }
            if (!char.IsLetterOrDigit(input[right])) { right--; continue; }

            if (char.ToLowerInvariant(input[left]) != char.ToLowerInvariant(input[right]))
                return false;

            left++;
            right--;
        }
        return true;
    }

    // Counts letters/digits only, case-insensitive.
    public static Dictionary<char, int> CharFrequency(string input)
    {
        var counts = new Dictionary<char, int>();
        foreach (var c in input)
        {
            if (!char.IsLetterOrDigit(c)) continue;

            var key = char.ToLowerInvariant(c);
            counts[key] = counts.GetValueOrDefault(key) + 1;
        }
        return counts;
    }

    // LINQ version of the same thing.
    public static Dictionary<char, int> CharFrequencyWithLinq(string input) =>
        input.Where(char.IsLetterOrDigit)
             .GroupBy(char.ToLowerInvariant)
             .ToDictionary(g => g.Key, g => g.Count());

    public static string FormatFrequency(Dictionary<char, int> counts)
    {
        var sb = new StringBuilder();
        foreach (var (c, count) in counts.OrderByDescending(kv => kv.Value).ThenBy(kv => kv.Key))
        {
            sb.Append($"{c}:{count} ");
        }
        return sb.ToString().TrimEnd();
    }
}
