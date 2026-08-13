using System;
using System.Collections.Generic;
using System.Linq;

// IRegistrable interface
public interface IRegistrable
{
    void Register();
}

// INotifiable interface
public interface INotifiable
{
    void NotifyUser();
}

// Base Event class
public class Event : IRegistrable, INotifiable
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime Date { get; set; }
    public string Location { get; set; }

    public Event(int id, string name, DateTime date, string location)
    {
        Id = id;
        Name = name;
        Date = date;
        Location = location;
    }

    public virtual void Register()
    {
        Console.WriteLine($"Registered for event: {Name}");
    }

    public virtual void NotifyUser()
    {
        Console.WriteLine($"Notification: {Name} is scheduled on {Date:dd-MM-yyyy}");
    }
}

// Conference class
public class Conference : Event
{
    public string Speaker { get; set; }

    public Conference(
        int id,
        string name,
        DateTime date,
        string location,
        string speaker)
        : base(id, name, date, location)
    {
        Speaker = speaker;
    }

    public override void Register()
    {
        Console.WriteLine($"Registered for Conference: {Name}");
    }
}

// Workshop class
public class Workshop : Event
{
    public string Topic { get; set; }

    public Workshop(
        int id,
        string name,
        DateTime date,
        string location,
        string topic)
        : base(id, name, date, location)
    {
        Topic = topic;
    }

    public override void Register()
    {
        Console.WriteLine($"Registered for Workshop: {Name}");
    }
}

// Webinar class
public class Webinar : Event
{
    public string MeetingLink { get; set; }

    public Webinar(
        int id,
        string name,
        DateTime date,
        string location,
        string meetingLink)
        : base(id, name, date, location)
    {
        MeetingLink = meetingLink;
    }

    public override void Register()
    {
        Console.WriteLine($"Registered for Webinar: {Name}");
    }
}

// Generic Event Manager
public class EventManager<T> where T : Event
{
    private readonly Dictionary<int, T> events =
        new Dictionary<int, T>();

    // Add event
    public void Add(T eventItem)
    {
        events[eventItem.Id] = eventItem;
    }

    // Indexer
    public T this[int id]
    {
        get
        {
            events.TryGetValue(id, out T eventItem);
            return eventItem;
        }
        set
        {
            events[id] = value;
        }
    }

    // Get all events
    public IEnumerable<T> GetAll()
    {
        return events.Values;
    }

    // Remove event
    public bool Remove(int id)
    {
        return events.Remove(id);
    }

    // Count
    public int Count()
    {
        return events.Count;
    }
}

// Extension methods
public static class EventExtensions
{
    // Reminder extension method
    public static void SetReminder(
        this Event eventItem,
        int daysBefore)
    {
        DateTime reminderDate =
            eventItem.Date.AddDays(-daysBefore);

        Console.WriteLine(
            $"Reminder set for '{eventItem.Name}' on {reminderDate:dd-MM-yyyy}"
        );
    }

    // Another reminder method
    public static void SendReminder(
        this Event eventItem)
    {
        Console.WriteLine(
            $"Reminder: '{eventItem.Name}' is on {eventItem.Date:dd-MM-yyyy}"
        );
    }
}

// Main Program
public class Program
{
    public static void Main()
    {
        // Create Conference
        var conference = new Conference(
            101,
            "Tech Conference 2026",
            new DateTime(2026, 9, 10),
            "Delhi",
            "Dr. Sharma"
        );

        // Create Workshop
        var workshop = new Workshop(
            102,
            "C# Workshop",
            new DateTime(2026, 9, 15),
            "Noida",
            "Advanced C#"
        );

        // Create Webinar
        var webinar = new Webinar(
            103,
            "AI Webinar",
            new DateTime(2026, 9, 20),
            "Online",
            "https://example.com/ai"
        );

        // Generic event managers
        var conferenceManager =
            new EventManager<Conference>();

        var workshopManager =
            new EventManager<Workshop>();

        var webinarManager =
            new EventManager<Webinar>();

        // Add events
        conferenceManager.Add(conference);
        workshopManager.Add(workshop);
        webinarManager.Add(webinar);

        // Indexer example
        Console.WriteLine("Event using indexer:");

        var eventItem = conferenceManager[101];

        Console.WriteLine(
            $"ID: {eventItem.Id}"
        );

        Console.WriteLine(
            $"Name: {eventItem.Name}"
        );

        Console.WriteLine();

        // Register events
        conference.Register();
        workshop.Register();
        webinar.Register();

        Console.WriteLine();

        // Notifications
        conference.NotifyUser();
        workshop.NotifyUser();
        webinar.NotifyUser();

        Console.WriteLine();

        // Extension methods for reminders
        conference.SetReminder(7);
        workshop.SetReminder(3);
        webinar.SendReminder();

        Console.WriteLine();

        // Anonymous type event summary
        var eventSummary = new
        {
            TotalEvents =
                conferenceManager.Count()
                + workshopManager.Count()
                + webinarManager.Count(),

            Conferences = conferenceManager.Count(),

            Workshops = workshopManager.Count(),

            Webinars = webinarManager.Count()
        };

        Console.WriteLine("Event Summary");
        Console.WriteLine(
            $"Total Events: {eventSummary.TotalEvents}"
        );

        Console.WriteLine(
            $"Conferences: {eventSummary.Conferences}"
        );

        Console.WriteLine(
            $"Workshops: {eventSummary.Workshops}"
        );

        Console.WriteLine(
            $"Webinars: {eventSummary.Webinars}"
        );
    }
}