Console.WriteLine("=== Strategy Pattern Example: Discount Calculation ===");

double purchaseAmount = 1000;

Console.WriteLine("\n--- Regular Customer ---");
var regularOrder = new Order(new RegularDiscount());
double regularFinal = regularOrder.GetFinalPrice(purchaseAmount);
Console.WriteLine($"Final price: {regularFinal}");

Console.WriteLine("\n--- Premium Customer ---");
var premiumOrder = new Order(new PremiumDiscount());
double premiumFinal = premiumOrder.GetFinalPrice(purchaseAmount);
Console.WriteLine($"Final price: {premiumFinal}");

Console.WriteLine("\n--- Seasonal Offer ---");
var seasonalOrder = new Order(new SeasonalDiscount());
double seasonalFinal = seasonalOrder.GetFinalPrice(purchaseAmount);
Console.WriteLine($"Final price: {seasonalFinal}");