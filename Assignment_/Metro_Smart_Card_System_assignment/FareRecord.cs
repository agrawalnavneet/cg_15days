public class FareRecord
{
    public int CardNumber;
    public int EntryZone;
    public int ExitZone;
    public double Fare;
    public long TapOutTime;

    public FareRecord(int cardNumber, int entryZone, int exitZone, double fare, long tapOutTime)
    {
        CardNumber = cardNumber;
        EntryZone = entryZone;
        ExitZone = exitZone;
        Fare = fare;
        TapOutTime = tapOutTime;
    }
}