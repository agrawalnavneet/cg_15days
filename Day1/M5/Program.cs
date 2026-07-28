using System;

namespace StudentMarks
{
    class Program
    {
        static void Main(string[] args)
        {
            double mark1, mark2, mark3, mark4, mark5;

            Console.WriteLine("===== Student Marks Calculator =====");

            // Subject 1
            Console.Write("Enter marks for Subject 1: ");
            while (!double.TryParse(Console.ReadLine(), out mark1) || mark1 < 0 || mark1 > 100)
            {
                Console.Write("Invalid marks! Enter marks between 0 and 100: ");
            }

            // Subject 2
            Console.Write("Enter marks for Subject 2: ");
            while (!double.TryParse(Console.ReadLine(), out mark2) || mark2 < 0 || mark2 > 100)
            {
                Console.Write("Invalid marks! Enter marks between 0 and 100: ");
            }

            // Subject 3
            Console.Write("Enter marks for Subject 3: ");
            while (!double.TryParse(Console.ReadLine(), out mark3) || mark3 < 0 || mark3 > 100)
            {
                Console.Write("Invalid marks! Enter marks between 0 and 100: ");
            }

            // Subject 4
            Console.Write("Enter marks for Subject 4: ");
            while (!double.TryParse(Console.ReadLine(), out mark4) || mark4 < 0 || mark4 > 100)
            {
                Console.Write("Invalid marks! Enter marks between 0 and 100: ");
            }

            // Subject 5
            Console.Write("Enter marks for Subject 5: ");
            while (!double.TryParse(Console.ReadLine(), out mark5) || mark5 < 0 || mark5 > 100)
            {
                Console.Write("Invalid marks! Enter marks between 0 and 100: ");
            }

            // Calculations
            double total = mark1 + mark2 + mark3 + mark4 + mark5;
            double average = total / 5;
            double percentage = (total / 500) * 100;

            // Output
            Console.WriteLine("\n===== Result =====");
            Console.WriteLine("Total Marks : " + total);
            Console.WriteLine("Average     : " + average);
            Console.WriteLine("Percentage  : " + percentage + "%");

            Console.ReadKey();
        }
    }
}