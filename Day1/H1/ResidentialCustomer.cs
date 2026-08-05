public class ResidentialCustomer : IBillCalculator
{
    public double CalculateBill(double units, double rate, double fixedCharge)
    {
        return (units * rate) + fixedCharge;
    }
}