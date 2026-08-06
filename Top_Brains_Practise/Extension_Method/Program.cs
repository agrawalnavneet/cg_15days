using System;
using System.Collections.Generic;

public static class EnumerableExtensions
{
    public static IEnumerable<TSource> DistinctBy<TSource, TKey>(
        this IEnumerable<TSource> source,
        Func<TSource, TKey> keySelector)
    {
        var seenKeys = new HashSet<TKey>();
        foreach (var item in source)
        {
            TKey key = keySelector(item);
            if (seenKeys.Add(key))
            {
                yield return item;
            }
        }
    }
}

public class Solution
{
    public static string[] GetDistinctNames(string[] items)
    {
        if (items == null || items.Length == 0)
            return Array.Empty<string>();

        var parsed = new List<(string id, string name)>(items.Length);

        foreach (var item in items)
        {
            int colonIndex = item.IndexOf(':');
            if (colonIndex < 0)
            {
                parsed.Add((item, string.Empty));
                continue;
            }

            string id = item.Substring(0, colonIndex);
            string name = item.Substring(colonIndex + 1);
            parsed.Add((id, name));
        }

        var distinct = parsed.DistinctBy(p => p.id);

        var result = new List<string>();
        foreach (var p in distinct)
        {
            result.Add(p.name);
        }

        return result.ToArray();
    }

    public static void Main(string[] args)
    {
        int n = Convert.ToInt32(Console.ReadLine().Trim());
        string[] items = new string[n];

        for (int i = 0; i < n; i++)
        {
            items[i] = Console.ReadLine();
        }

        string[] result = GetDistinctNames(items);

        Console.WriteLine(string.Join(",", result));
    }
}