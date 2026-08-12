using System;
using System.Globalization;

class Program
{
    static void Main()
    {
        string input = " llapppptop bag ";
        input = input.Trim();

        string result = "";

        foreach (char c in input)
        {
            if (result.Length == 0 || result[result.Length - 1] != c)
            {
                result += c;
            }
        }

        result = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(result.ToLower());

        Console.WriteLine(result);
    }
}