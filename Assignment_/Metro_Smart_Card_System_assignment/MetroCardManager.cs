using System;
using System.Collections.Generic;
using System.Linq;

public class MetroCardManager : IMetroOperations
{
    private readonly Dictionary<int, Station> _stationMap = new();
    private readonly double _baseFare;
    private readonly double _perKmRate;
    private readonly double _maxDailyCap;

    private readonly Dictionary<int, Commuter> _commuters = new();
    private readonly Dictionary<int, Journey> _activeJourneys = new();
    private readonly Dictionary<int, LinkedList<double>> _fareHistoryMap = new(); // last 5 fares
    private readonly Dictionary<int, Dictionary<string, int>> _routeFrequency = new();
    private readonly Dictionary<int, Dictionary<long, double>> _dailyTotals = new(); // card -> day -> total
    private readonly List<FareRecord> _allFareRecords = new();

    public MetroCardManager(List<Station> stations, double baseFare, double perKmRate, double maxDailyCap)
    {
        _baseFare = baseFare;
        _perKmRate = perKmRate;
        _maxDailyCap = maxDailyCap;

        foreach (var s in stations)
            _stationMap[s.StationId] = s;
    }

    public void IssueCard(int cardNumber, string commuterName, string commuterType)
    {
        if (_commuters.ContainsKey(cardNumber))
            return; // card numbers must be unique

        var commuter = new Commuter
        {
            CardNumber = cardNumber,
            CommuterName = commuterName,
            CommuterType = commuterType,
            TravelSummary = new TravelSummary
            {
                LastEntryStation = -1,
                LastExitStation = -1,
                LastEntryTime = 0,
                LastExitTime = 0,
                TotalFarePaid = 0.0,
                TotalTrips = 0,
                AverageFarePerTrip = 0.0
            }
        };

        _commuters[cardNumber] = commuter;
    }

    public bool TapIn(int cardNumber, int stationId, long epochTime)
    {
        if (!_commuters.TryGetValue(cardNumber, out var commuter)) return false;
        if (_activeJourneys.ContainsKey(cardNumber)) return false; // already mid-journey
        if (!_stationMap.ContainsKey(stationId)) return false;

        _activeJourneys[cardNumber] = new Journey(stationId, epochTime);
        commuter.TravelSummary.LastEntryStation = stationId;
        commuter.TravelSummary.LastEntryTime = epochTime;
        return true;
    }

    public bool TapOut(int cardNumber, int stationId, long epochTime)
    {
        if (!_commuters.TryGetValue(cardNumber, out var commuter)) return false;
        if (!_activeJourneys.TryGetValue(cardNumber, out var journey)) return false;
        if (!_stationMap.ContainsKey(stationId)) return false;
        if (epochTime <= journey.EntryTime) return false;
        if (journey.EntryStationId == stationId) return false;

        var entryStation = _stationMap[journey.EntryStationId];
        var exitStation = _stationMap[stationId];

        double distance = CalculateDistance(entryStation, exitStation);
        double durationMinutes = (epochTime - journey.EntryTime) / (1000.0 * 60.0);

        double fare;
        if (durationMinutes > 120)
            fare = _baseFare * 3; // long-journey penalty
        else
            fare = _baseFare + (distance * _perKmRate);

        fare *= GetDiscountMultiplier(commuter.CommuterType);

        // ---- Daily cap logic ----
        long day = epochTime / 86400000L;
        if (!_dailyTotals.TryGetValue(cardNumber, out var cardDaily))
        {
            cardDaily = new Dictionary<long, double>();
            _dailyTotals[cardNumber] = cardDaily;
        }
        double totalSoFar = cardDaily.TryGetValue(day, out var t) ? t : 0.0;

        double actualFareCharged = (totalSoFar >= _maxDailyCap) ? 0.0 : fare;
        cardDaily[day] = totalSoFar + actualFareCharged;

        // ---- Update travel summary ----
        commuter.TravelSummary.LastExitStation = stationId;
        commuter.TravelSummary.LastExitTime = epochTime;
        commuter.TravelSummary.TotalFarePaid += actualFareCharged;
        commuter.TravelSummary.TotalTrips += 1;
        commuter.TravelSummary.AverageFarePerTrip =
            commuter.TravelSummary.TotalFarePaid / commuter.TravelSummary.TotalTrips;

        // ---- Fare history (last 5) ----
        if (!_fareHistoryMap.TryGetValue(cardNumber, out var hist))
        {
            hist = new LinkedList<double>();
            _fareHistoryMap[cardNumber] = hist;
        }
        hist.AddLast(actualFareCharged);
        if (hist.Count > 5) hist.RemoveFirst();

        // ---- Route frequency ----
        string route = $"{entryStation.StationName} to {exitStation.StationName}";
        if (!_routeFrequency.TryGetValue(cardNumber, out var routes))
        {
            routes = new Dictionary<string, int>();
            _routeFrequency[cardNumber] = routes;
        }
        routes[route] = routes.TryGetValue(route, out var c) ? c + 1 : 1;

        // ---- Zone revenue record ----
        _allFareRecords.Add(new FareRecord(cardNumber, entryStation.Zone, exitStation.Zone,
            actualFareCharged, epochTime));

        _activeJourneys.Remove(cardNumber);
        return true;
    }

    public Commuter? GetCommuterInfo(int cardNumber)
    {
        return _commuters.TryGetValue(cardNumber, out var commuter)
            ? commuter
            : null;
    }

    public List<double> FareHistory(int cardNumber)
    {
        if (!_fareHistoryMap.TryGetValue(cardNumber, out var hist))
            return new List<double>();

        // Most recent first, in the order they occurred (not sorted by value)
        var list = hist.ToList();
        list.Reverse();
        return list;
    }

    public Dictionary<string, double> GetZoneWiseRevenue(long startTime, long endTime)
    {
        var agg = new Dictionary<string, double>();

        foreach (var fr in _allFareRecords)
        {
            if (fr.TapOutTime >= startTime && fr.TapOutTime <= endTime)
            {
                string key = $"Zone{fr.EntryZone}-Zone{fr.ExitZone}";
                agg[key] = agg.TryGetValue(key, out var v) ? v + fr.Fare : fr.Fare;
            }
        }

        return agg
            .OrderByDescending(e => e.Value)
            .ToDictionary(e => e.Key, e => e.Value);
    }

    public List<string> GetFrequentRoute(int cardNumber)
    {
        if (!_routeFrequency.TryGetValue(cardNumber, out var routes))
            return new List<string>();

        return routes
            .OrderByDescending(e => e.Value)
            .Take(3)
            .Select(e => e.Key)
            .ToList();
    }

    public double GetDailyPassSavings(int cardNumber, long date)
    {
        if (!_dailyTotals.TryGetValue(cardNumber, out var cardDaily))
            return 0.0;

        long day = YyyyMmDdToEpochDay(date);
        if (!cardDaily.TryGetValue(day, out var actual))
            return 0.0;

        double passCost = _maxDailyCap * 0.8;
        double savings = actual - passCost;
        return savings > 0 ? savings : 0.0;
    }

    // ---------------- Helpers ----------------

    private double GetDiscountMultiplier(string type)
    {
        return type switch
        {
            "SENIOR" => 0.50,  // 50% discount
            "STUDENT" => 0.75, // 25% discount
            "CHILD" => 0.25,   // 75% discount
            "ADULT" => 1.00,
            _ => 1.00
        };
    }

    private long YyyyMmDdToEpochDay(long date)
    {
        int year = (int)(date / 10000);
        int month = (int)((date / 100) % 100);
        int day = (int)(date % 100);
        var dt = new DateTime(year, month, day, 0, 0, 0, DateTimeKind.Utc);
        var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        return (long)(dt - epoch).TotalDays;
    }

    private double CalculateDistance(Station s1, Station s2)
    {
        double lat1 = ToRadians(s1.Latitude);
        double lon1 = ToRadians(s1.Longitude);
        double lat2 = ToRadians(s2.Latitude);
        double lon2 = ToRadians(s2.Longitude);

        double dlat = lat2 - lat1;
        double dlon = lon2 - lon1;

        double a = Math.Pow(Math.Sin(dlat / 2), 2) +
                   Math.Cos(lat1) * Math.Cos(lat2) * Math.Pow(Math.Sin(dlon / 2), 2);

        double c = 2 * Math.Asin(Math.Sqrt(a));
        double r = 6371; // Earth radius km
        return r * c;
    }

    private double ToRadians(double deg) => deg * Math.PI / 180.0;
}