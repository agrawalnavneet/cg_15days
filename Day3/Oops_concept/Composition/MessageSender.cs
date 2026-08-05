public interface IMessageSender
{
    void Send(string message);
}

public class EmailSender : IMessageSender
{
    public void Send(string message)
    {
        Console.WriteLine($"Email sent: {message}");
    }
}

public class SMSSender : IMessageSender
{
    public void Send(string message)
    {
        Console.WriteLine($"SMS sent: {message}");
    }
}