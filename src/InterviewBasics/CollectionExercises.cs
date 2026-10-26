namespace InterviewBasics;

public static class CollectionExercises
{
    // Two Sum: return indices of the two numbers that add up to target.
    // One pass with a Dictionary<value, index>: O(n) time, O(n) space
    // (vs. the O(n^2) nested-loop brute force).
    public static (int First, int Second)? TwoSum(int[] nums, int target)
    {
        var seen = new Dictionary<int, int>();
        for (var i = 0; i < nums.Length; i++)
        {
            var complement = target - nums[i];
            if (seen.TryGetValue(complement, out var j))
                return (j, i);

            seen[nums[i]] = i;
        }
        return null;
    }

    // HashSet.Add returns false when the item is already present.
    public static List<T> FindDuplicates<T>(IEnumerable<T> items)
    {
        var seen = new HashSet<T>();
        var duplicates = new HashSet<T>();
        foreach (var item in items)
        {
            if (!seen.Add(item))
                duplicates.Add(item);
        }
        return duplicates.ToList();
    }

    public static List<T> FindDuplicatesWithLinq<T>(IEnumerable<T> items) =>
        items.GroupBy(x => x)
             .Where(g => g.Count() > 1)
             .Select(g => g.Key)
             .ToList();

    // Group words that are anagrams of each other: the sorted letters are the key.
    // ["eat","tea","tan","ate","nat","bat"] -> [[eat,tea,ate],[tan,nat],[bat]]
    public static List<List<string>> GroupAnagrams(IEnumerable<string> words)
    {
        var groups = new Dictionary<string, List<string>>();
        foreach (var word in words)
        {
            var chars = word.ToLowerInvariant().ToCharArray();
            Array.Sort(chars);
            var key = new string(chars);

            if (!groups.TryGetValue(key, out var group))
            {
                group = [];
                groups[key] = group;
            }
            group.Add(word);
        }
        return groups.Values.ToList();
    }
}
