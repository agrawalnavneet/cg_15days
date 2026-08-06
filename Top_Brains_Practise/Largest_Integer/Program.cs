using System;

class Program
{
    static int Largest(int a, int b, int c)
    {
        if (a >= b && a >= c)
            return a;
        else if (b >= a && b >= c)
            return b;
        else
            return c;
    }

    static void Main(string[] args)
    {
        Console.Write("Enter first integer (a): ");
        int a = Convert.ToInt32(Console.ReadLine());

        Console.Write("Enter second integer (b): ");
        int b = Convert.ToInt32(Console.ReadLine());

        Console.Write("Enter third integer (c): ");
        int c = Convert.ToInt32(Console.ReadLine());

        int result = Largest(a, b, c);

        Console.WriteLine("The largest integer is: " + result);
    }
}