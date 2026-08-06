using System;

public class Solution
{
    public static double ConvertFeetToCm(int feet)
    {
        double cm = feet * 30.48;
        return Math.Round(cm, 2, MidpointRounding.AwayFromZero);
    }

    public static void Main(string[] args)
    {
        
        int feet = Convert.ToInt32(Console.ReadLine().Trim());

    
        double result = ConvertFeetToCm(feet);
        Console.WriteLine(result.ToString("0.00"));
    }
}