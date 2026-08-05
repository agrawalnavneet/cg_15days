public interface IDiscountStrategy
{
    double CalculateDiscount(double amount);
}

public class RegularDiscount : IDiscountStrategy
{
    public double CalculateDiscount(double amount) => amount * 0.05; // 5%
}

public class PremiumDiscount : IDiscountStrategy
{
    public double CalculateDiscount(double amount) => amount * 0.15; // 15%
}

public class SeasonalDiscount : IDiscountStrategy
{
    public double CalculateDiscount(double amount) => amount * 0.25; // 25%
}