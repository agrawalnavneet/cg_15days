using System;

class Program
{
    static long DigitSum(long n)
    {
        long sum = 0;
        n = Math.Abs(n);
        while (n > 0)
        {
            sum += n % 10;
            n /= 10;
        }
        return sum;
    }

    static bool IsPrime(long num)
    {
        if (num < 2) return false;
        if (num == 2) return true;
        if (num % 2 == 0) return false;

        for (long i = 3; i * i <= num; i += 2)
        {
            if (num % i == 0) return false;
        }
        return true;
    }

    static bool IsLuckyNumber(long x)
    {
        if (IsPrime(x)) return false;

        long sx = DigitSum(x);
        long sxSquare = DigitSum(x * x);

        return sxSquare == sx * sx;
    }

    static int CountLuckyNumbers(int m, int n)
    {
        int count = 0;
        for (int x = m; x <= n; x++)
        {
            if (IsLuckyNumber(x))
                count++;
        }
        return count;
    }

    static void Main(string[] args)
    {
        Console.Write("Enter m: ");
        int m = Convert.ToInt32(Console.ReadLine());

        Console.Write("Enter n: ");
        int n = Convert.ToInt32(Console.ReadLine());

        int result = CountLuckyNumbers(m, n);

        Console.WriteLine("Output: " + result);
    }
}