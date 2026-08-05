public class CommercialCustomer : IBillCalculator
{
    public double CalculateBill(double units, double rate, double fixedCharge)
    {
        double bill = (units * rate) + fixedCharge;
        return bill + (bill * 0.10);
    }
}