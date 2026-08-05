public class UPIPayment : Payment
{
    public override void ProcessPayment(double amount)
    {
        Console.WriteLine($"Processing UPI payment: {amount}");
        LogTransaction(amount);
    }
}