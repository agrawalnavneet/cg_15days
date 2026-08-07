using System;

public class Solution
{
    public static int SumParsedIntegers(string[] tokens)
    {
        int sum = 0;

        foreach (string token in tokens)
        {
            if (int.TryParse(token, out int value))
            {
                sum += value;
            }
        }

        return sum;
    }

    public static void Main()
    {
        string[] tokens = { "10", "20", "abc", "30", "2147483648", "-5" };
        Console.WriteLine(SumParsedIntegers(tokens));
    }
}