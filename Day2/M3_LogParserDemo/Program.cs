using System;
using System.Globalization;

public enum LogLevel
{
    Info,
    Warning,
    Error,
    Critical
}

public static class LogParser
{
    public static bool ParseLogLine(in string logLine, out DateTime timestamp, out LogLevel level, ref int counter)
    {
        timestamp = default;
        level = default;

        if (string.IsNullOrWhiteSpace(logLine))
            return false;

        var parts = logLine.Split(new[] { ' ' }, 3);

        if (parts.Length < 3)
            return false;

        string datePart = parts[0];
        string timePart = parts[1];
        string rest = parts[2];

        string timestampString = $"{datePart} {timePart}";

        if (!DateTime.TryParseExact(
                timestampString,
                "yyyy-MM-dd HH:mm:ss",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out timestamp))
        {
            return false;
        }

        int colonIndex = rest.IndexOf(':');
        string levelString = colonIndex >= 0 ? rest.Substring(0, colonIndex) : rest;

        if (!Enum.TryParse(levelString, ignoreCase: true, out level))
            return false;

        counter++;

        return true;
    }
}

public class Program
{
    public static void Main()
    {
        string logLine = "2023-10-27 14:30:00 ERROR: Disk full";
        int counter = 0;

        bool success = LogParser.ParseLogLine(in logLine, out DateTime timestamp, out LogLevel level, ref counter);

        Console.WriteLine($"Success: {success}");
        Console.WriteLine($"Timestamp: {timestamp:yyyy-MM-dd HH:mm:ss}");
        Console.WriteLine($"LogLevel: {level}");
        Console.WriteLine($"Counter after call: {counter}");
    }
}