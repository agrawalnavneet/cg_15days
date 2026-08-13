using System;
using System.Collections.Generic;

abstract class Employee
{
    private int id;
    private string name;

    public int Id
    {
        get { return id; }
        set
        {
            if (value <= 0)
                throw new ArgumentException("Id must be greater than 0.");

            id = value;
        }
    }

    public string Name
    {
        get { return name; }
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Name cannot be empty.");

            name = value;
        }
    }

    public abstract double CalculateSalary();

    public abstract double CalculateBonus();
}


// Permanent Employee
class PermanentEmployee : Employee
{
    public double BasicSalary { get; set; }

    public override double CalculateSalary()
    {
        return BasicSalary;
    }

    public override double CalculateBonus()
    {
        return BasicSalary * 0.10;
    }
}


// Contract Employee
class ContractEmployee : Employee
{
    public double MonthlySalary { get; set; }

    public override double CalculateSalary()
    {
        return MonthlySalary;
    }

    public override double CalculateBonus()
    {
        return MonthlySalary * 0.05;
    }
}


// Intern Employee
class InternEmployee : Employee
{
    public double Stipend { get; set; }

    public override double CalculateSalary()
    {
        return Stipend;
    }

    public override double CalculateBonus()
    {
        return Stipend * 0.02;
    }
}


class Program
{
    static void Main()
    {
        // Create Permanent Employee using object initializer
        var emp1 = new PermanentEmployee
        {
            Id = 1,
            Name = "Pankaj",
            BasicSalary = 50000
        };

        // Create Contract Employee using object initializer
        var emp2 = new ContractEmployee
        {
            Id = 2,
            Name = "Rahul",
            MonthlySalary = 40000
        };

        // Create Intern Employee using object initializer
        var emp3 = new InternEmployee
        {
            Id = 3,
            Name = "Amit",
            Stipend = 15000
        };

        // Store all employees
        List<Employee> employees = new List<Employee>
        {
            emp1,
            emp2,
            emp3
        };

        // Generate payroll report using anonymous types
        var payrollReport = new List<object>();

        foreach (var emp in employees)
        {
            var report = new
            {
                Id = emp.Id,
                Name = emp.Name,
                Salary = emp.CalculateSalary(),
                Bonus = emp.CalculateBonus(),
                Total = emp.CalculateSalary() + emp.CalculateBonus()
            };

            payrollReport.Add(report);
        }

        // Display payroll report
        Console.WriteLine("========== PAYROLL REPORT ==========");

        foreach (dynamic report in payrollReport)
        {
            Console.WriteLine($"Employee ID : {report.Id}");
            Console.WriteLine($"Name        : {report.Name}");
            Console.WriteLine($"Salary      : {report.Salary}");
            Console.WriteLine($"Bonus       : {report.Bonus}");
            Console.WriteLine($"Total       : {report.Total}");
            Console.WriteLine("------------------------------------");
        }
    }
}