using System;

class Program
{
    static void SwapWithRef(ref int a, ref int b)
    {
        int temp = a;
        a = b;
        b = temp;
    }

    static void Main()
    {
        int x = 10, y = 20;
        Console.WriteLine($"Before: x = {x}, y = {y}");

        SwapWithRef(ref x, ref y);

        Console.WriteLine($"After:  x = {x}, y = {y}");
    }
}