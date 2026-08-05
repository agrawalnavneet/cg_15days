using System;
using System.Collections.Generic;
using System.Linq;

public static class OrderProcessor
{
    public static bool TryProcessOrder(string commaSeparatedIsbns, out List<string> validIsbns)
    {
        var rawIsbns = commaSeparatedIsbns
            .Split(',')
            .Select(s => s.Trim())
            .ToArray();

        return TryProcessOrder(out validIsbns, rawIsbns);
    }

    public static bool TryProcessOrder(out List<string> validIsbns, params string[] isbns)
    {
        validIsbns = new List<string>();

        foreach (string isbn in isbns)
        {
            if (TryParseISBN(isbn, out string cleanedIsbn))
            {
                validIsbns.Add(cleanedIsbn);
            }
        }

        return validIsbns.Count > 0;
    }

    public static bool TryParseISBN(string input, out string cleanedIsbn)
    {
        cleanedIsbn = null;

        if (string.IsNullOrWhiteSpace(input))
            return false;

        static string Clean(string s) => s.Replace("-", "").Replace(" ", "");

        string candidate = Clean(input);

        if (candidate.Length == 13 && candidate.All(char.IsDigit)
            && (candidate.StartsWith("978") || candidate.StartsWith("979")))
        {
            cleanedIsbn = candidate;
            return true;
        }

        return false;
    }
}

public class Program
{
    public static void Main()
    {
        bool success = OrderProcessor.TryProcessOrder(
            "978-3-16-148410-0, 1234567890123, invalid-isbn, 978-1-4028-9462-6",
            out List<string> validIsbns);

        Console.WriteLine($"Returns: {success}");
        Console.WriteLine("Valid ISBNs: [" + string.Join(", ", validIsbns.Select(x => $"\"{x}\"")) + "]");
    }
}