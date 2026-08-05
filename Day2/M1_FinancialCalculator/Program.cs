using System;

public static class FinancialCalculator
{
   
    public static double CalculateCompoundInterest(
        double principal,
        double rate,
        double time = 1,
        int compoundingFrequency = 1)
    {
        if (principal < 0) throw new ArgumentException("Principal cannot be negative.");
        if (rate < 0) throw new ArgumentException("Rate cannot be negative.");
        if (time < 0) throw new ArgumentException("Time cannot be negative.");
        if (compoundingFrequency <= 0) throw new ArgumentException("Compounding frequency must be positive.");

        double futureValue = principal * Math.Pow(1 + rate / compoundingFrequency,
                                                    compoundingFrequency * time);
        return futureValue;
    }

   
    public static double CalculateCompoundInterest(
        int principal,
        double rate,
        double time = 1,
        int compoundingFrequency = 1)
    {
        return CalculateCompoundInterest((double)principal, rate, time, compoundingFrequency);
    }
}

public class Program
{
    public static void Main()
    {
     
        double result1 = FinancialCalculator.CalculateCompoundInterest(10000, 0.05, 10);
        Console.WriteLine($"Output 1 (annually): {result1:C2}");

       
        double result2 = FinancialCalculator.CalculateCompoundInterest(
            10000, 0.05, 10, compoundingFrequency: 12);
        Console.WriteLine($"Output 2 (monthly): {result2:C2}");


        double result3 = FinancialCalculator.CalculateCompoundInterest(
            principal: 10000,
            rate: 0.05,
            time: 10,
            compoundingFrequency: 12);
        Console.WriteLine($"Output 3 (named args, same as monthly): {result3:C2}");
    }
}