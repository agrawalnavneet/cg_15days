public class Notification
{
    private readonly IMessageSender _sender; // composed, not inherited

    public Notification(IMessageSender sender)
    {
        _sender = sender;
    }

    public void NotifyUser(string message)
    {
        _sender.Send(message);
    }
}