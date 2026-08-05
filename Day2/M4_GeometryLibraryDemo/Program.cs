using System;

public static class GeometryCalculator
{
    public static double CalculateArea(double radius, int decimals = 2)
    {
        double area = Math.PI * radius * radius;
        return Math.Round(area, decimals);
    }

    public static double CalculateArea(double width, double height)
    {
        return width * height;
    }

    public static double CalculateArea(double baseLength, double height, bool isTriangle)
    {
        return isTriangle
            ? 0.5 * baseLength * height
            : baseLength * height;
    }
}

public class Program
{
    public static void Main()
    {
        double output1 = GeometryCalculator.CalculateArea(5);
        Console.WriteLine($"Output 1: Circle area = {output1}");

        double output2 = GeometryCalculator.CalculateArea(width: 4, height: 6);
        Console.WriteLine($"Output 2: Rectangle area = {output2}");

        double output3 = GeometryCalculator.CalculateArea(baseLength: 3, height: 7, isTriangle: true);
        Console.WriteLine($"Output 3: Triangle area = {output3}");

        double output4 = GeometryCalculator.CalculateArea(radius: 5, decimals: 4);
        Console.WriteLine($"Output 4: Circle area = {output4}");
    }
}