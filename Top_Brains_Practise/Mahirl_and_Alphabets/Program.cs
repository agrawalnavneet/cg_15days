using System;
using System.Collections.Generic;
using System.Text;

class Alpha
{
    static bool IsVowel(char ch)
    {
        ch = char.ToLower(ch);
        return ch == 'a' || ch == 'e' || ch == 'i' || ch == 'o' || ch == 'u';
    }

    public static void alpha()
    {
        string w1 = Console.ReadLine();
        string w2 = Console.ReadLine();

        HashSet<char> consonantsInWord2 = new HashSet<char>();

        foreach (char c in w2)
        {
            char lower = char.ToLower(c);
            if (!IsVowel(lower))
            {
                consonantsInWord2.Add(lower);
            }
        }

        StringBuilder filtered = new StringBuilder();

        foreach (char c in w1)
        {
            char lower = char.ToLower(c);
            if (IsVowel(lower) || !consonantsInWord2.Contains(lower))
            {
                filtered.Append(c);
            }
        }

        StringBuilder result = new StringBuilder();

        for (int i = 0; i < filtered.Length; i++)
        {
            if (i == 0 || filtered[i] != filtered[i - 1])
            {
                result.Append(filtered[i]);
            }
        }

        Console.WriteLine(result.ToString());
    }
}

class Program
{
    static void Main(string[] args)
    {
        Alpha.alpha();
    }
}