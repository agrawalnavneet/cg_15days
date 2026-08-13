using System;

class Program
{
    static int SumIntegers(object[] values)
    {
        int sum = 0;

        foreach (object value in values)
        {
            if (value is int x)
            {
                sum += x;
            }
        }

        return sum;
    }

    static void Main()
    {
        object[] values = { 10, "Hello", true, 20, null, 5, "C#" };

        int result = SumIntegers(values);

        Console.WriteLine(result);
    }
}