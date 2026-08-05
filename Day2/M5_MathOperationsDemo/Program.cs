using System;
using System.Linq;

public static class MathOperations
{
    public static int Add(int a, int b)
    {
        return a + b;
    }

    public static int Add(params int[] numbers)
    {
        int sum = 0;
        foreach (int n in numbers)
        {
            sum += n;
        }
        return sum;
    }

    public static int Multiply(int a, int b)
    {
        return a * b;
    }

    public static int Multiply(params int[] numbers)
    {
        if (numbers.Length == 0)
            return 0;

        int product = 1;
        foreach (int n in numbers)
        {
            product *= n;
        }
        return product;
    }
}

public class Program
{
    public static void Main()
    {
        int output1 = MathOperations.Add(5, 10);
        Console.WriteLine($"Output 1: {output1}");

        int output2 = MathOperations.Add(1, 2, 3, 4, 5);
        Console.WriteLine($"Output 2: {output2}");

        int output3 = MathOperations.Multiply(2, 3);
        Console.WriteLine($"Output 3: {output3}");

        int output4 = MathOperations.Multiply(2, 3, 4, 5);
        Console.WriteLine($"Output 4: {output4}");
    }
}