using InterviewBasics;

Section("FizzBuzz (1-15)");
Console.WriteLine(string.Join(", ", StringExercises.FizzBuzz(15)));

Section("Reverse a string");
Console.WriteLine($"\"interview\" -> \"{StringExercises.Reverse("interview")}\"");
Console.WriteLine($"LINQ version: \"{StringExercises.ReverseWithLinq("interview")}\"");

Section("Palindrome");
foreach (var s in new[] { "racecar", "A man, a plan, a canal: Panama", "dotnet" })
{
    Console.WriteLine($"\"{s}\" -> {StringExercises.IsPalindrome(s)}");
}

Section("Character frequency");
const string text = "Hello World";
Console.WriteLine($"\"{text}\" -> {StringExercises.FormatFrequency(StringExercises.CharFrequency(text))}");

Section("Two Sum");
int[] nums = [2, 7, 11, 15];
var pair = CollectionExercises.TwoSum(nums, 9);
Console.WriteLine(pair is { } p
    ? $"[{string.Join(", ", nums)}], target 9 -> indices ({p.First}, {p.Second})"
    : "No pair found");

Section("Find duplicates");
int[] withDupes = [1, 2, 3, 2, 4, 5, 1, 1];
Console.WriteLine($"[{string.Join(", ", withDupes)}] -> [{string.Join(", ", CollectionExercises.FindDuplicates(withDupes))}]");

Section("Group anagrams");
string[] words = ["eat", "tea", "tan", "ate", "nat", "bat"];
foreach (var group in CollectionExercises.GroupAnagrams(words))
{
    Console.WriteLine($"[{string.Join(", ", group)}]");
}

static void Section(string title) => Console.WriteLine($"{Environment.NewLine}=== {title} ===");
