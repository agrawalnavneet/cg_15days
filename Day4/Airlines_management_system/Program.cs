using System;
using System.Collections.Generic;
using System.Linq;

// Abstract Person class
public abstract class Person
{
    public string Name { get; set; }
    public string Email { get; set; }

    protected Person(string name, string email)
    {
        Name = name;
        Email = email;
    }
}

// Interfaces
public interface IBookable
{
    void BookSeat(Flight flight);
}

public interface ICancelable
{
    void CancelTicket(Flight flight);
}

// Passenger class
public class Passenger : Person, IBookable, ICancelable
{
    public string PassengerId { get; set; }
    public string BookedSeat { get; private set; }

    public Passenger(string name, string email, string passengerId)
        : base(name, email)
    {
        PassengerId = passengerId;
    }

    public void BookSeat(Flight flight)
    {
        if (flight.GetAvailableSeats() > 0)
        {
            BookedSeat = flight.BookSeat(this);
            Console.WriteLine(
                $"{Name} booked seat {BookedSeat} on flight {flight.FlightNumber}.");
        }
        else
        {
            flight.AddToWaitlist(this);
            Console.WriteLine(
                $"{Name} added to waitlist for flight {flight.FlightNumber}.");
        }
    }

    public void CancelTicket(Flight flight)
    {
        if (BookedSeat != null)
        {
            flight.CancelSeat(BookedSeat);
            Console.WriteLine(
                $"{Name} cancelled ticket for seat {BookedSeat}.");

            BookedSeat = null;
        }
        else
        {
            Console.WriteLine($"{Name} has no booked seat.");
        }
    }

    public void UpdateSeat(string newSeat)
    {
        BookedSeat = newSeat;
    }
}

// Flight class
public class Flight
{
    private readonly Dictionary<string, Passenger> seats;
    private readonly Queue<Passenger> waitlist;

    public string FlightNumber { get; set; }
    public string Source { get; set; }
    public string Destination { get; set; }
    public int TotalSeats { get; set; }
    public decimal BasePrice { get; set; }

    public Flight(
        string flightNumber,
        string source,
        string destination,
        int totalSeats,
        decimal basePrice)
    {
        FlightNumber = flightNumber;
        Source = source;
        Destination = destination;
        TotalSeats = totalSeats;
        BasePrice = basePrice;

        seats = new Dictionary<string, Passenger>();
        waitlist = new Queue<Passenger>();

        for (int i = 1; i <= totalSeats; i++)
        {
            seats.Add($"S{i}", null);
        }
    }

    // Indexer by flight number is implemented in repository.
    public string BookSeat(Passenger passenger)
    {
        foreach (var seat in seats)
        {
            if (seat.Value == null)
            {
                seats[seat.Key] = passenger;
                return seat.Key;
            }
        }

        return null;
    }

    public void CancelSeat(string seatNumber)
    {
        if (seats.ContainsKey(seatNumber))
        {
            seats[seatNumber] = null;

            // Give the cancelled seat to the first person in waitlist
            if (waitlist.Count > 0)
            {
                Passenger nextPassenger = waitlist.Dequeue();

                seats[seatNumber] = nextPassenger;
                nextPassenger.UpdateSeat(seatNumber);

                Console.WriteLine(
                    $"{nextPassenger.Name} got seat {seatNumber} from waitlist.");
            }
        }
    }

    public int GetAvailableSeats()
    {
        return seats.Count(x => x.Value == null);
    }

    public void AddToWaitlist(Passenger passenger)
    {
        waitlist.Enqueue(passenger);
    }

    public void UpgradeSeat(Passenger passenger, string newSeat)
    {
        if (!seats.ContainsKey(newSeat))
        {
            Console.WriteLine("Invalid seat.");
            return;
        }

        if (seats[newSeat] != null)
        {
            Console.WriteLine("Seat is already occupied.");
            return;
        }

        if (passenger.BookedSeat == null)
        {
            Console.WriteLine("Passenger does not have a booked seat.");
            return;
        }

        string oldSeat = passenger.BookedSeat;

        seats[oldSeat] = null;
        seats[newSeat] = passenger;

        passenger.UpdateSeat(newSeat);

        Console.WriteLine(
            $"{passenger.Name} upgraded from {oldSeat} to {newSeat}.");
    }

    public void DisplaySeats()
    {
        Console.WriteLine("\nSeat Status:");

        foreach (var seat in seats)
        {
            if (seat.Value == null)
            {
                Console.WriteLine($"{seat.Key} - Available");
            }
            else
            {
                Console.WriteLine(
                    $"{seat.Key} - {seat.Value.Name}");
            }
        }
    }
}

// Generic repository
public class Repository<T>
{
    private readonly List<T> items = new List<T>();

    public void Add(T item)
    {
        items.Add(item);
    }

    public IEnumerable<T> GetAll()
    {
        return items;
    }
}

// Flight repository with indexer
public class FlightRepository : Repository<Flight>
{
    public Flight this[string flightNumber]
    {
        get
        {
            return GetAll()
                .FirstOrDefault(f =>
                    f.FlightNumber.Equals(
                        flightNumber,
                        StringComparison.OrdinalIgnoreCase));
        }
    }

    public IEnumerable<Flight> SearchFlights(
        string source,
        string destination)
    {
        return GetAll().Where(f =>
            f.Source.Equals(source, StringComparison.OrdinalIgnoreCase)
            &&
            f.Destination.Equals(
                destination,
                StringComparison.OrdinalIgnoreCase));
    }
}

// Extension methods
public static class FlightExtensions
{
    public static int GetAvailableSeats(this Flight flight)
    {
        return flight.GetAvailableSeats();
    }

    public static decimal CalculateTicketPrice(
        this Flight flight,
        string passengerType = "Adult")
    {
        decimal price = flight.BasePrice;

        if (passengerType.Equals(
            "Child",
            StringComparison.OrdinalIgnoreCase))
        {
            price *= 0.75m;
        }
        else if (passengerType.Equals(
            "Senior",
            StringComparison.OrdinalIgnoreCase))
        {
            price *= 0.80m;
        }

        return price;
    }
}

// Boarding pass class
public class BoardingPass
{
    public string PassengerName { get; set; }
    public string FlightNumber { get; set; }
    public string SeatNumber { get; set; }
    public string Source { get; set; }
    public string Destination { get; set; }

    public void Display()
    {
        Console.WriteLine("\n========== BOARDING PASS ==========");
        Console.WriteLine($"Passenger : {PassengerName}");
        Console.WriteLine($"Flight    : {FlightNumber}");
        Console.WriteLine($"From      : {Source}");
        Console.WriteLine($"To        : {Destination}");
        Console.WriteLine($"Seat      : {SeatNumber}");
        Console.WriteLine("===================================");
    }
}

public class Program
{
    public static void Main()
    {
        // Create flight repository
        var flights = new FlightRepository();

        // Object initializers
        var flight1 = new Flight(
            "AI101",
            "Delhi",
            "Mumbai",
            5,
            5000);

        var flight2 = new Flight(
            "AI102",
            "Delhi",
            "Bangalore",
            5,
            6000);

        var flight3 = new Flight(
            "AI103",
            "Mumbai",
            "Delhi",
            5,
            4500);

        flights.Add(flight1);
        flights.Add(flight2);
        flights.Add(flight3);

        // Search flights
        Console.WriteLine("SEARCH FLIGHTS");

        var searchResults = flights.SearchFlights(
            "Delhi",
            "Mumbai");

        foreach (var flight in searchResults)
        {
            Console.WriteLine(
                $"{flight.FlightNumber}: " +
                $"{flight.Source} -> {flight.Destination}");
        }

        // Indexer
        Console.WriteLine("\nSEARCH USING INDEXER");

        var selectedFlight = flights["AI101"];

        if (selectedFlight != null)
        {
            Console.WriteLine(
                $"Flight found: {selectedFlight.FlightNumber}");
        }

        // Create passengers
        var passenger1 = new Passenger(
            "Navneet",
            "navneet@gmail.com",
            "P001");

        var passenger2 = new Passenger(
            "Rahul",
            "rahul@gmail.com",
            "P002");

        var passenger3 = new Passenger(
            "Amit",
            "amit@gmail.com",
            "P003");

        var passenger4 = new Passenger(
            "Priya",
            "priya@gmail.com",
            "P004");

        var passenger5 = new Passenger(
            "Ankit",
            "ankit@gmail.com",
            "P005");

        var passenger6 = new Passenger(
            "Rohit",
            "rohit@gmail.com",
            "P006");

        // Book seats
        Console.WriteLine("\nBOOKING SEATS");

        passenger1.BookSeat(selectedFlight);
        passenger2.BookSeat(selectedFlight);
        passenger3.BookSeat(selectedFlight);
        passenger4.BookSeat(selectedFlight);
        passenger5.BookSeat(selectedFlight);

        // Flight is full, passenger6 goes to waitlist
        passenger6.BookSeat(selectedFlight);

        // Display seats
        selectedFlight.DisplaySeats();

        // Extension method
        Console.WriteLine(
            $"\nAvailable Seats: {selectedFlight.GetAvailableSeats()}");

        // Calculate ticket price
        decimal price = selectedFlight.CalculateTicketPrice("Adult");

        Console.WriteLine(
            $"Ticket Price for Adult: ₹{price}");

        decimal childPrice =
            selectedFlight.CalculateTicketPrice("Child");

        Console.WriteLine(
            $"Ticket Price for Child: ₹{childPrice}");

        // Cancel ticket
        Console.WriteLine("\nCANCEL TICKET");

        passenger2.CancelTicket(selectedFlight);

        // Display seats after cancellation
        selectedFlight.DisplaySeats();

        // Seat upgrade
        Console.WriteLine("\nSEAT UPGRADE");

        passenger1.BookSeat(selectedFlight);

        selectedFlight.UpgradeSeat(
            passenger1,
            "S5");

        // Generate boarding pass
        Console.WriteLine("\nGENERATE BOARDING PASS");

        var boardingPass = new BoardingPass
        {
            PassengerName = passenger1.Name,
            FlightNumber = selectedFlight.FlightNumber,
            SeatNumber = passenger1.BookedSeat,
            Source = selectedFlight.Source,
            Destination = selectedFlight.Destination
        };

        boardingPass.Display();

        Console.WriteLine("\nProgram completed.");
    }
}