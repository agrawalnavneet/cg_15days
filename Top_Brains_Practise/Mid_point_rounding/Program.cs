using System;

class Mid
{
    static double CA(double r)
    {
        double area = Math.PI * r * r;
        return Math.Round(area, 2, MidpointRounding.AwayFromZero);
    }

    public static void mid()
    {
        double r = Convert.ToDouble(Console.ReadLine());
        double result = CA(r);
        Console.WriteLine(result);
    }
}

class Program
{
    static void Main(string[] args)
    {
        Mid.mid();
    }
}