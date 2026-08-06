using System;

public class Program
{
    public static string EvaluateExpression(string expression)
    {
        if (string.IsNullOrWhiteSpace(expression))
        
            return "Error:InvalidExpression";

        string[] parts = expression.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        if (parts.Length != 3)
            return "Error:InvalidExpression";

        if (!int.TryParse(parts[0], out int a) ||
            !int.TryParse(parts[2], out int b))
        {
            return "Error:InvalidNumber";
        }

        string op = parts[1];

        switch (op)
        {
            case "+":
                return (a + b).ToString();

            case "-":
                return (a - b).ToString();

            case "*":
                return (a * b).ToString();

            case "/":
                if (b == 0)
                    return "Error:DivideByZero";
                return (a / b).ToString();

            default:
                return "Error:UnknownOperator";
        }
    }

    public static void Main(string[] args)
    {
        string? expression = Console.ReadLine();

        if (expression == null)
        {
            Console.WriteLine("Error:InvalidExpression");
            return;
        }

        Console.WriteLine(EvaluateExpression(expression));
    }
}