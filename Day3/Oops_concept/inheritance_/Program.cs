List<Payment> payments = new List<Payment>
{
    new CreditCardPayment(),
    new UPIPayment(),
    new NetBankingPayment()
};

foreach (var payment in payments)
{
    payment.ProcessPayment(500);
}