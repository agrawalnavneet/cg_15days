using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

class Program
{
    static void Main()
    {
        var output = new StringBuilder();

        string? input = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(input))
            return;

        string[] firstLine = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        int numberOfRequests = int.Parse(firstLine[0]);
        double baseFare = double.Parse(firstLine[1], CultureInfo.InvariantCulture);
        double perKmRate = double.Parse(firstLine[2], CultureInfo.InvariantCulture);
        double maxDailyCap = double.Parse(firstLine[3], CultureInfo.InvariantCulture);

        string? stationCountInput = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(stationCountInput))
            return;

        int numberOfStations = int.Parse(stationCountInput.Trim());

        var stations = new List<Station>();

        for (int i = 0; i < numberOfStations; i++)
        {
            string? stationLine = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(stationLine))
                continue;

            var parts = stationLine.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            stations.Add(new Station
            {
                StationId = int.Parse(parts[0]),
                StationName = parts[1],
                Zone = int.Parse(parts[2]),
                Latitude = double.Parse(parts[3], CultureInfo.InvariantCulture),
                Longitude = double.Parse(parts[4], CultureInfo.InvariantCulture)
            });
        }

        var manager = new MetroCardManager(stations, baseFare, perKmRate, maxDailyCap);

        var tokenPattern = new Regex("\"([^\"]*)\"|(\\S+)");

        for (int i = 0; i < numberOfRequests; i++)
        {
            string? line = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(line))
                break;

            var tokens = new List<string>();

            foreach (Match m in tokenPattern.Matches(line))
            {
                tokens.Add(m.Groups[1].Success ? m.Groups[1].Value : m.Groups[2].Value);
            }

            if (tokens.Count == 0)
                continue;

            string cmd = tokens[0];

            switch (cmd)
            {
                case "issueCard":
                {
                    int cardNumber = int.Parse(tokens[1]);
                    string name = tokens[2];
                    string type = tokens[3];

                    manager.IssueCard(cardNumber, name, type);
                    break;
                }

                case "tapIn":
                {
                    int cardNumber = int.Parse(tokens[1]);
                    int stationId = int.Parse(tokens[2]);
                    long time = long.Parse(tokens[3]);

                    output.AppendLine(manager.TapIn(cardNumber, stationId, time) ? "true" : "false");
                    break;
                }

                case "tapOut":
                {
                    int cardNumber = int.Parse(tokens[1]);
                    int stationId = int.Parse(tokens[2]);
                    long time = long.Parse(tokens[3]);

                    output.AppendLine(manager.TapOut(cardNumber, stationId, time) ? "true" : "false");
                    break;
                }

                case "commuterInfo":
                {
                    int cardNumber = int.Parse(tokens[1]);
                    var c = manager.GetCommuterInfo(cardNumber);

                    if (c != null)
                    {
                        output.AppendLine(string.Join(" ",
                            c.CardNumber,
                            c.CommuterName,
                            c.CommuterType,
                            c.TravelSummary.LastEntryStation,
                            c.TravelSummary.LastExitStation,
                            c.TravelSummary.LastEntryTime,
                            c.TravelSummary.LastExitTime,
                            FormatDouble(c.TravelSummary.TotalFarePaid),
                            c.TravelSummary.TotalTrips,
                            FormatDouble(c.TravelSummary.AverageFarePerTrip)));
                    }

                    break;
                }

                case "fareHistory":
                {
                    int cardNumber = int.Parse(tokens[1]);

                    foreach (var fare in manager.FareHistory(cardNumber))
                    {
                        output.AppendLine(FormatDouble(fare));
                    }

                    break;
                }

                case "zoneRevenue":
                {
                    long startTime = long.Parse(tokens[1]);
                    long endTime = long.Parse(tokens[2]);

                    foreach (var item in manager.GetZoneWiseRevenue(startTime, endTime))
                    {
                        output.AppendLine($"{item.Key}:{FormatDouble(item.Value)}");
                    }

                    break;
                }

                case "frequentRoute":
                {
                    int cardNumber = int.Parse(tokens[1]);

                    foreach (var route in manager.GetFrequentRoute(cardNumber))
                    {
                        output.AppendLine(route);
                    }

                    break;
                }

                case "dailySavings":
                {
                    int cardNumber = int.Parse(tokens[1]);
                    long date = long.Parse(tokens[2]);

                    output.AppendLine(FormatDouble(manager.GetDailyPassSavings(cardNumber, date)));
                    break;
                }
            }
        }

        Console.Write(output.ToString());
    }

    private static string FormatDouble(double value)
    {
        double rounded = Math.Round(value, 2);

        if (rounded == Math.Floor(rounded))
        {
            return ((long)rounded).ToString(CultureInfo.InvariantCulture) + ".0";
        }

        return rounded.ToString(CultureInfo.InvariantCulture);
    }
}