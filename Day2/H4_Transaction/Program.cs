using System;
using System.Collections.Generic;

public class Transaction
{
    public int Id { get; set; }
    public string TransactionCode { get; set; }
    public decimal Amount { get; set; }
    public List<Transaction> DependentTransactions { get; } = new List<Transaction>();

    public Transaction(int id, string code, decimal amount)
    {
        Id = id;
        TransactionCode = code;
        Amount = amount;
    }
}

public static class RiskAssessment
{
    private const int MaxDepth = 1000;
    private const double ErrorScore = -1;

    public static bool TryParseTransactionId(string input, out int transactionId)
    {
        transactionId = -1;

        if (string.IsNullOrWhiteSpace(input))
        {
            return false;
        }

        if (!input.StartsWith("TX", StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        string numericPart = input.Substring(2);

        if (!int.TryParse(numericPart, out int parsedId))
        {
            return false;
        }

        transactionId = parsedId;
        return true;
    }

    public static double CalculateRiskScore(string startTransactionId, Dictionary<int, Transaction> transactionGraph)
    {
        if (!TryParseTransactionId(startTransactionId, out int startId))
        {
            Console.WriteLine($"Warning: '{startTransactionId}' is not a valid transaction ID format.");
            return ErrorScore;
        }

        if (!transactionGraph.TryGetValue(startId, out Transaction startTransaction))
        {
            Console.WriteLine($"Warning: Transaction ID {startId} was not found in the graph.");
            return ErrorScore;
        }

        var visited = new HashSet<int>();

        double Traverse(Transaction current, ref int depth)
        {
            if (depth > MaxDepth)
            {
                Console.WriteLine($"Warning: Maximum recursion depth ({MaxDepth}) exceeded at transaction {current.Id}. Aborting.");
                return ErrorScore;
            }

            if (!visited.Add(current.Id))
            {
                Console.WriteLine($"Warning: Circular reference detected at transaction {current.Id}. Treating as terminal node.");
                return 0;
            }

            double score = (double)current.Amount * 0.01;

            foreach (var dependent in current.DependentTransactions)
            {
                int nextDepth = depth + 1;
                double dependentScore = Traverse(dependent, ref nextDepth);

                if (dependentScore == ErrorScore)
                {
                    return ErrorScore;
                }

                score += dependentScore;
            }

            return score;
        }

        int depth = 0;
        return Traverse(startTransaction, ref depth);
    }
}

class Program
{
    static void Main()
    {
        var graph = new Dictionary<int, Transaction>();

        for (int i = 1; i <= 1500; i++)
        {
            graph[i] = new Transaction(i, $"TX{i:D3}", 100m);
        }

        for (int i = 1; i < 1500; i++)
        {
            graph[i].DependentTransactions.Add(graph[i + 1]);
        }
        graph[1500].DependentTransactions.Add(graph[1]);

        double result = RiskAssessment.CalculateRiskScore("TX001", graph);

        Console.WriteLine($"Final risk score: {result}");
    }
}