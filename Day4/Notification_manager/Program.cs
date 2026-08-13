using System;
using System.Collections.Generic;

// Interface for all notification channels
public interface INotification
{
    string ChannelName { get; }
    bool IsSent { get; }
    void Send(string message);
}

// Email notification
public class Email : INotification
{
    public string ChannelName => "Email";
    public bool IsSent { get; private set; }

    public void Send(string message)
    {
        Console.WriteLine($"Email sent: {message}");
        IsSent = true;
    }
}

// SMS notification
public class SMS : INotification
{
    public string ChannelName => "SMS";
    public bool IsSent { get; private set; }

    public void Send(string message)
    {
        Console.WriteLine($"SMS sent: {message}");
        IsSent = true;
    }
}

// WhatsApp notification
public class WhatsApp : INotification
{
    public string ChannelName => "WhatsApp";
    public bool IsSent { get; private set; }

    public void Send(string message)
    {
        Console.WriteLine($"WhatsApp message sent: {message}");
        IsSent = true;
    }
}

// Push notification
public class PushNotification : INotification
{
    public string ChannelName => "Push Notification";
    public bool IsSent { get; private set; }

    public void Send(string message)
    {
        Console.WriteLine($"Push notification sent: {message}");
        IsSent = true;
    }
}

// Notification Manager
// Uses Dependency Injection
public class NotificationManager
{
    private readonly List<INotification> notifications;

    public NotificationManager(List<INotification> notifications)
    {
        this.notifications = notifications;
    }

    // Sends notification to multiple channels
    public void Send(string message)
    {
        foreach (var notification in notifications)
        {
            notification.Send(message);
        }
    }

    // Displays notification status
    public void ShowStatus()
    {
        Console.WriteLine("\nNotification Status:");

        foreach (var notification in notifications)
        {
            Console.WriteLine(
                $"{notification.ChannelName}: " +
                $"{(notification.IsSent ? "Sent" : "Not Sent")}"
            );
        }
    }
}

// Main Program
public class Program
{
    public static void Main()
    {
        // Create notification objects
        INotification email = new Email();
        INotification whatsapp = new WhatsApp();
        INotification sms = new SMS();

        // Add multiple notification channels
        var notificationChannels = new List<INotification>
        {
            email,
            whatsapp,
            sms
        };

       
        var manager = new NotificationManager(notificationChannels);

        // Send notification
        manager.Send("Your order has been successfully placed.");

        // Display notification status
        manager.ShowStatus();
    }
}