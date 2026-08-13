using System;
using System.Collections.Generic;
using System.Linq;

// Interface for exporting reports
interface IReportExporter
{
    void Export(List<object> rows);
}

// Abstract base class
abstract class Report
{
    protected List<object> Rows = new List<object>();

    public void AddRow(object row)
    {
        Rows.Add(row);
    }

    public abstract void Generate();
}

// PDF Report
class PdfReport : Report, IReportExporter
{
    public void Export(List<object> rows)
    {
        Console.WriteLine("Exporting report to PDF...");
    }

    public override void Generate()
    {
        Console.WriteLine("\nPDF Report");
        Console.WriteLine("----------------");

        foreach (var row in Rows)
        {
            Console.WriteLine(row.FormatRow());
        }

        Export(Rows);
    }
}

// Excel Report
class ExcelReport : Report, IReportExporter
{
    public void Export(List<object> rows)
    {
        Console.WriteLine("Exporting report to Excel...");
    }

    public override void Generate()
    {
        Console.WriteLine("\nExcel Report");
        Console.WriteLine("----------------");

        foreach (var row in Rows)
        {
            Console.WriteLine(row.FormatRow());
        }

        Export(Rows);
    }
}

// CSV Report
class CsvReport : Report, IReportExporter
{
    public void Export(List<object> rows)
    {
        Console.WriteLine("Exporting report to CSV...");
    }

    public override void Generate()
    {
        Console.WriteLine("\nCSV Report");
        Console.WriteLine("----------------");

        foreach (var row in Rows)
        {
            Console.WriteLine(row.FormatRow());
        }

        Export(Rows);
    }
}

// Factory Pattern
class ReportFactory
{
    public static Report Create(string type)
    {
        switch (type.ToUpper())
        {
            case "PDF":
                return new PdfReport();

            case "EXCEL":
                return new ExcelReport();

            case "CSV":
                return new CsvReport();

            default:
                throw new ArgumentException("Invalid report type.");
        }
    }
}

// Extension methods for formatting
static class ReportExtensions
{
    public static string FormatRow(this object row)
    {
        var properties = row.GetType().GetProperties();

        return string.Join(" | ",
            properties.Select(p => $"{p.Name}: {p.GetValue(row)}"));
    }
}

class Program
{
    static void Main()
    {
        // Anonymous types for report rows
        var row1 = new
        {
            Id = 1,
            Name = "Navneet",
            Amount = 5000
        };

        var row2 = new
        {
            Id = 2,
            Name = "Rahul",
            Amount = 7500
        };

        var row3 = new
        {
            Id = 3,
            Name = "Aman",
            Amount = 6200
        };

        // Create PDF report using Factory Pattern
        var report = ReportFactory.Create("PDF");

        // Add anonymous type rows
        report.AddRow(row1);
        report.AddRow(row2);
        report.AddRow(row3);

        // Generate report
        report.Generate();

        // Create Excel report
        var excelReport = ReportFactory.Create("Excel");

        excelReport.AddRow(row1);
        excelReport.AddRow(row2);
        excelReport.AddRow(row3);

        excelReport.Generate();

        // Create CSV report
        var csvReport = ReportFactory.Create("CSV");

        csvReport.AddRow(row1);
        csvReport.AddRow(row2);
        csvReport.AddRow(row3);

        csvReport.Generate();
    }
}