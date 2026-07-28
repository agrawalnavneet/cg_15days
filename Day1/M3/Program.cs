using System;

class Program
{
    static void Main()
    {
        double length, width, height;

        // Input Length
        Console.Write("Enter Length: ");
        while (!double.TryParse(Console.ReadLine(), out length) || length <= 0)
        {
            Console.Write("Invalid Length. Enter again: ");
        }

        // Input Width
        Console.Write("Enter Width: ");
        while (!double.TryParse(Console.ReadLine(), out width) || width <= 0)
        {
            Console.Write("Invalid Width. Enter again: ");
        }

        // Input Height
        Console.Write("Enter Height: ");
        while (!double.TryParse(Console.ReadLine(), out height) || height <= 0)
        {
            Console.Write("Invalid Height. Enter again: ");
        }

        // Calculate Volume
        double volume = length * width * height;

        Console.WriteLine("\n----- Warehouse Volume -----");
        Console.WriteLine("Volume = " + Math.Round(volume, 2));
    }
}