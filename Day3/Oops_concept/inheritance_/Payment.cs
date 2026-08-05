public abstract class Payment
{
    public abstract void ProcessPayment(double amount);

    public void LogTransaction(double amount)
    {
        Console.WriteLine($"Transaction of {amount} logged.");
    }
}