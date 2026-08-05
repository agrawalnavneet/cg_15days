using System.Collections.Generic;

public interface IMetroOperations
{
    void IssueCard(int cardNumber, string commuterName, string commuterType);
    bool TapIn(int cardNumber, int stationId, long epochTime);
    bool TapOut(int cardNumber, int stationId, long epochTime);
   Commuter? GetCommuterInfo(int cardNumber);
    List<double> FareHistory(int cardNumber);
    Dictionary<string, double> GetZoneWiseRevenue(long startTime, long endTime);
    List<string> GetFrequentRoute(int cardNumber);
    double GetDailyPassSavings(int cardNumber, long date);
}