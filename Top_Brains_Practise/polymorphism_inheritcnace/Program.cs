using System;

abstract class Employee
{
    public abstract decimal CalculatePay();
}

class HourlyEmployee : Employee
{
    private decimal rate;
    private decimal hours;

    public HourlyEmployee(decimal rate, decimal hours)
    {
        this.rate = rate;
        this.hours = hours;
    }

    public override decimal CalculatePay()
    {
        return rate * hours;
    }
}

class SalariedEmployee : Employee
{
    private decimal monthlySalary;

    public SalariedEmployee(decimal monthlySalary)
    {
        this.monthlySalary = monthlySalary;
    }

    public override decimal CalculatePay()
    {
        return monthlySalary;
    }
}

class CommissionEmployee : Employee
{
    private decimal commission;
    private decimal baseSalary;

    public CommissionEmployee(decimal commission, decimal baseSalary)
    {
        this.commission = commission;
        this.baseSalary = baseSalary;
    }

    public override decimal CalculatePay()
    {
        return baseSalary + commission;
    }
}

class Program
{
    public static decimal ComputePayroll(string[] employees)
    {
        decimal totalPay = 0;

        foreach (string employee in employees)
        {
            string[] parts = employee.Split(' ');

            Employee emp;

            if (parts[0] == "H")
            {
                emp = new HourlyEmployee(
                    decimal.Parse(parts[1]),
                    decimal.Parse(parts[2])
                );
            }
            else if (parts[0] == "S")
            {
                emp = new SalariedEmployee(
                    decimal.Parse(parts[1])
                );
            }
            else
            {
                emp = new CommissionEmployee(
                    decimal.Parse(parts[1]),
                    decimal.Parse(parts[2])
                );
            }

            totalPay += emp.CalculatePay();
        }

        return Math.Round(totalPay, 2);
    }

    // 👇 THIS WAS MISSING
    static void Main(string[] args)
    {
        string[] employees =
        {
            "H 20 10",
            "S 30000",
            "C 5000 25000"
        };

        decimal result = ComputePayroll(employees);

        Console.WriteLine(result);
    }
}