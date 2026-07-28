using System;

class Program
{
    static void Main()
    {
        double weight;
        double height;

        // Input Weight
        Console.Write("Enter Weight (kg): ");
        while (!double.TryParse(Console.ReadLine(), out weight) || weight <= 0)
        {
            Console.Write("Invalid weight. Please enter again: ");
        }

        // Input Height
        Console.Write("Enter Height (m): ");
        while (!double.TryParse(Console.ReadLine(), out height) || height <= 0)
        {
            Console.Write("Invalid height. Please enter again: ");
        }

        // Calculate BMI
        double bmi = weight / (height * height);
        bmi = Math.Round(bmi, 2);

        // Display BMI
        Console.WriteLine("\n----- BMI Result -----");
        Console.WriteLine("BMI: " + bmi);

        // Display Category
        if (bmi < 18.5)
        {
            Console.WriteLine("Category: Underweight");
        }
        else if (bmi < 25)
        {
            Console.WriteLine("Category: Normal Weight");
        }
        else if (bmi < 30)
        {
            Console.WriteLine("Category: Overweight");
        }
        else
        {
            Console.WriteLine("Category: Obese");
        }
    }
}