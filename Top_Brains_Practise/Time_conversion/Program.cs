using System;

class Program
{
    static string TimeConversion(int totalSeconds)
    {
        int minutes = totalSeconds / 60;
        int seconds = totalSeconds % 60;

        return $"{minutes}:{seconds:D2}";
    }

    static void Main()
    {
        Console.WriteLine(TimeConversion(125)); 
        Console.WriteLine(TimeConversion(60));  
    }
}