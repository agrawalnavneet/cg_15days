using System;

public class Solution
{
    public static int Gcd(int a, int b)
    {
        if (b == 0)
            return a;
        return Gcd(b, a % b);
    }

    public static void Main(string[] args)
    {
        int a = Convert.ToInt32(Console.ReadLine().Trim());
        int b = Convert.ToInt32(Console.ReadLine().Trim());

        int result = Gcd(a, b);

        Console.WriteLine(result);
    }
}