using System;

class Program
{
    static void Main()
    {
        double price, discount;
        int quantity;

        Console.Write("Enter Item Price: ");
        while (!double.TryParse(Console.ReadLine(), out price) || price < 0)
        {
            Console.Write("Invalid price. Enter again: ");
        }

        Console.Write("Enter Quantity: ");
        while (!int.TryParse(Console.ReadLine(), out quantity) || quantity < 0)
        {
            Console.Write("Invalid quantity. Enter again: ");
        }

        Console.Write("Enter Discount (%): ");
        while (!double.TryParse(Console.ReadLine(), out discount) || discount < 0)
        {
            Console.Write("Invalid discount. Enter again: ");
        }

        double subtotal = price * quantity;
        double discountAmount = subtotal * discount / 100;
        double finalAmount = subtotal - discountAmount;

        Console.WriteLine("\n----- Bill -----");
        Console.WriteLine($"Subtotal: {Math.Round(subtotal, 2)}");
        Console.WriteLine($"Discount: {Math.Round(discountAmount, 2)}");
        Console.WriteLine($"Final Payable: {Math.Round(finalAmount, 2)}");
    }
}