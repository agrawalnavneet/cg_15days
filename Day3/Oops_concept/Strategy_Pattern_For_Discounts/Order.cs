public class Order
{
    private readonly IDiscountStrategy _discountStrategy;

    public Order(IDiscountStrategy discountStrategy)
    {
        _discountStrategy = discountStrategy;
    }

    public double GetFinalPrice(double amount)
    {
        double discount = _discountStrategy.CalculateDiscount(amount);
        return amount - discount;
    }
}