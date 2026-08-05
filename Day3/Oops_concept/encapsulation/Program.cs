Console.WriteLine("=== Encapsulation Example: Employee Salary ===");

var emp = new Employee();
emp.SetInitialSalary(50000);
Console.WriteLine($"Initial salary: {emp.Salary}");

emp.GiveRaise(10); // valid 10% raise
Console.WriteLine($"After 10% raise: {emp.Salary}");

emp.ApplyAnnualIncrement(15000); 
Console.WriteLine($"After annual increment: {emp.Salary}");

Console.WriteLine("\n--- Testing invalid operations ---");

try
{
    emp.GiveRaise(60); 
}
catch (ArgumentException e)
{
    Console.WriteLine($"Error caught: {e.Message}");
}

try
{
    emp.ApplyAnnualIncrement(150000); }
catch (ArgumentException e)
{
    Console.WriteLine($"Error caught: {e.Message}");
}

