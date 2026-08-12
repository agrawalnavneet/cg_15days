using System;
using System.Collections.Generic;

abstract class Shape
{
    public abstract double GetArea();
}

interface IArea
{
    double GetArea();
}

class Circle : Shape, IArea
{
    private double radius;

    public Circle(double radius)
    {
        this.radius = radius;
    }

    public override double GetArea()
    {
        return Math.PI * radius * radius;
    }
}

class Rectangle : Shape, IArea
{
    private double width;
    private double height;

    public Rectangle(double width, double height)
    {
        this.width = width;
        this.height = height;
    }

    public override double GetArea()
    {
        return width * height;
    }
}

class Triangle : Shape, IArea
{
    private double baseValue;
    private double height;

    public Triangle(double baseValue, double height)
    {
        this.baseValue = baseValue;
        this.height = height;
    }

    public override double GetArea()
    {
        return 0.5 * baseValue * height;
    }
}

class Program
{
    public static double TotalArea(string[] shapes)
    {
        double total = 0;

        foreach (string input in shapes)
        {
            string[] parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            switch (parts[0])
            {
                case "C":
                    total += new Circle(double.Parse(parts[1])).GetArea();
                    break;

                case "R":
                    total += new Rectangle(
                        double.Parse(parts[1]),
                        double.Parse(parts[2])
                    ).GetArea();
                    break;

                case "T":
                    total += new Triangle(
                        double.Parse(parts[1]),
                        double.Parse(parts[2])
                    ).GetArea();
                    break;
            }
        }

        return Math.Round(total, 2, MidpointRounding.AwayFromZero);
    }

    static void Main(string[] args)
    {
        string[] shapes =
        {
            "C 5",
            "R 10 20",
            "T 10 5"
        };

        Console.WriteLine(TotalArea(shapes));
    }
}