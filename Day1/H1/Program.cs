using System;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("===== Electricity Billing Calculator =====");

        Console.Write("Enter Customer Type (Residential/Commercial): ");
        string customerType = Console.ReadLine();

        double units;
        while (true)
        {
            Console.Write("Enter Units Consumed: ");
            if (double.TryParse(Console.ReadLine(), out units) && units >= 0)
                break;

            Console.WriteLine("Invalid Units! Please enter a valid number.");
        }

        double rate;
        while (true)
        {
            Console.Write("Enter Rate Per Unit: ");
            if (double.TryParse(Console.ReadLine(), out rate) && rate >= 0)
                break;

            Console.WriteLine("Invalid Rate!");
        }

        double fixedCharge;
        while (true)
        {
            Console.Write("Enter Fixed Charges: ");
            if (double.TryParse(Console.ReadLine(), out fixedCharge) && fixedCharge >= 0)
                break;

            Console.WriteLine("Invalid Fixed Charge!");
        }

        IBillCalculator calculator;

        if (customerType.Equals("Residential", StringComparison.OrdinalIgnoreCase))
        {
            calculator = new ResidentialCustomer();
        }
        else if (customerType.Equals("Commercial", StringComparison.OrdinalIgnoreCase))
        {
            calculator = new CommercialCustomer();
        }
        else
        {
            Console.WriteLine("Invalid Customer Type.");
            return;
        }

        double totalBill = calculator.CalculateBill(units, rate, fixedCharge);

        Console.WriteLine("------------------------------");
        Console.WriteLine("Customer Type : " + customerType);
        Console.WriteLine("Total Bill    : ₹" + Math.Round(totalBill, 2));
    }
}