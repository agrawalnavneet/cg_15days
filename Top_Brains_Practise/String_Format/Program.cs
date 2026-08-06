using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;


public record Student(string Name, int Score);

class Program
{
    static string SolveStudents(string[] items, int minScore)
    {
        var students = items
            .Select(ParseStudent)
            .Where(s => s.Score >= minScore)
            .OrderByDescending(s => s.Score)
            .ThenBy(s => s.Name, StringComparer.Ordinal)
            .ToList();

        return JsonSerializer.Serialize(students);
    }

    static Student ParseStudent(string item)
    {
        int colonIndex = item.LastIndexOf(':');
        string name = item.Substring(0, colonIndex);
        int score = int.Parse(item.Substring(colonIndex + 1));
        return new Student(name, score);
    }

    static void Main()
    {
        string[] items = { "Alice:85", "Bob:92", "Charlie:85", "Dave:70" };
        int minScore = 80;

        string json = SolveStudents(items, minScore);
        Console.WriteLine(json);
       
    }
}