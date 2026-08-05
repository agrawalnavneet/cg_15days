using System;

class Program
{
    static void Main()
    {
        double openingBalance, deposits, withdrawals;

        // Opening Balance
        Console.Write("Enter Opening Balance: ");
        while (!double.TryParse(Console.ReadLine(), out openingBalance) || openingBalance < 0)
        {
            Console.Write("Invalid Opening Balance. Enter again: ");
        }

        // Deposits
        Console.Write("Enter Total Deposits: ");
        while (!double.TryParse(Console.ReadLine(), out deposits) || deposits < 0)
        {
            Console.Write("Invalid Deposit Amount. Enter again: ");
        }

        // Withdrawals
        Console.Write("Enter Total Withdrawals: ");
        while (!double.TryParse(Console.ReadLine(), out withdrawals) || withdrawals < 0)
        {
            Console.Write("Invalid Withdrawal Amount. Enter again: ");
        }

        // Check withdrawal limit
        if (withdrawals > openingBalance + deposits)
        {
            Console.WriteLine("\nError: Insufficient Balance.");
        }
        else
        {
            double finalBalance = openingBalance + deposits - withdrawals;

            Console.WriteLine("\n----- Account Summary -----");
            Console.WriteLine("Updated Balance = " + Math.Round(finalBalance, 2));
        }
    }
}