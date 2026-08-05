public class CreditCardPayment : Payment
{
    public override void ProcessPayment(double amount)
    {
        Console.WriteLine($"Processing credit card payment: {amount}");
        LogTransaction(amount);
    }
}