public class Employee
{
    private double _salary;

    public double Salary => _salary; // read-only public access

    public void SetInitialSalary(double salary)
    {
        _salary = salary;
    }

    public void GiveRaise(double percentage)
    {
        if (percentage <= 0 || percentage > 50)
            throw new ArgumentException("Invalid raise percentage");

        _salary += _salary * (percentage / 100);
    }

    public void ApplyAnnualIncrement(double incrementAmount)
    {
        if (incrementAmount > 100000)
            throw new ArgumentException("Increment exceeds allowed limit");

        _salary += incrementAmount;
    }
}