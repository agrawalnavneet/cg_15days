public class NetBankingPayment : Payment
{
    public override void ProcessPayment(double amount)
    {
        Console.WriteLine($"Processing net banking payment: {amount}");
        LogTransaction(amount);
    }
}