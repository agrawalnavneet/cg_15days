using System;
using System.Collections.Generic;
using System.Linq;

// Abstract Vehicle class
public abstract class Vehicle
{
    public string VehicleNumber { get; set; }

    protected Vehicle(string vehicleNumber)
    {
        VehicleNumber = vehicleNumber;
    }

    public abstract void DisplayVehicle();
}

// Car class
public class Car : Vehicle
{
    public string Model { get; set; }

    public Car(string vehicleNumber, string model)
        : base(vehicleNumber)
    {
        Model = model;
    }

    public override void DisplayVehicle()
    {
        Console.WriteLine($"Car: {Model}, Number: {VehicleNumber}");
    }
}

// Bike class
public class Bike : Vehicle
{
    public string Model { get; set; }

    public Bike(string vehicleNumber, string model)
        : base(vehicleNumber)
    {
        Model = model;
    }

    public override void DisplayVehicle()
    {
        Console.WriteLine($"Bike: {Model}, Number: {VehicleNumber}");
    }
}

// Driver class
public class Driver
{
    private string name;
    private int age;

    public string Id { get; set; }

    public string Name
    {
        get { return name; }
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Driver name cannot be empty.");

            name = value;
        }
    }

    public int Age
    {
        get { return age; }
        set
        {
            if (value < 18)
                throw new ArgumentException("Driver must be at least 18 years old.");

            age = value;
        }
    }

    public Vehicle Vehicle { get; set; }

    public Driver(string id, string name, int age, Vehicle vehicle)
    {
        Id = id;
        Name = name;
        Age = age;
        Vehicle = vehicle;
    }

    public void Display()
    {
        Console.WriteLine($"Driver: {Name}, Age: {Age}, ID: {Id}");
        Vehicle.DisplayVehicle();
    }
}

// Rider class
public class Rider
{
    private string name;

    public string Id { get; set; }

    public string Name
    {
        get { return name; }
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Rider name cannot be empty.");

            name = value;
        }
    }

    public Rider(string id, string name)
    {
        Id = id;
        Name = name;
    }

    public void Display()
    {
        Console.WriteLine($"Rider: {Name}, ID: {Id}");
    }
}

// Ride class
public class Ride
{
    private double distance;

    public string RideId { get; set; }
    public Driver Driver { get; set; }
    public Rider Rider { get; set; }

    public double Distance
    {
        get { return distance; }
        set
        {
            if (value <= 0)
                throw new ArgumentException("Distance must be greater than 0.");

            distance = value;
        }
    }

    public double Fare { get; set; }

    public Ride(
        string rideId,
        Driver driver,
        Rider rider,
        double distance)
    {
        RideId = rideId;
        Driver = driver;
        Rider = rider;
        Distance = distance;
    }

    public virtual void Display()
    {
        Console.WriteLine($"Ride ID: {RideId}");
        Console.WriteLine($"Driver: {Driver.Name}");
        Console.WriteLine($"Rider: {Rider.Name}");
        Console.WriteLine($"Distance: {Distance} km");
        Console.WriteLine($"Fare: ₹{Fare}");
    }
}

// Sealed class for completed rides
public sealed class CompletedRide : Ride
{
    public DateTime CompletedAt { get; set; }

    public CompletedRide(
        string rideId,
        Driver driver,
        Rider rider,
        double distance)
        : base(rideId, driver, rider, distance)
    {
        CompletedAt = DateTime.Now;
    }

    public override void Display()
    {
        base.Display();
        Console.WriteLine($"Completed At: {CompletedAt}");
        Console.WriteLine("Status: Completed");
    }
}

// Extension methods
public static class RideExtensions
{
    // Calculate distance
    public static double CalculateDistance(
        this Ride ride,
        double startLatitude,
        double startLongitude,
        double endLatitude,
        double endLongitude)
    {
        double latDifference = endLatitude - startLatitude;
        double lonDifference = endLongitude - startLongitude;

        double distance = Math.Sqrt(
            Math.Pow(latDifference, 2) +
            Math.Pow(lonDifference, 2)
        ) * 111;

        return Math.Round(distance, 2);
    }

    // Calculate fare
    public static double CalculateFare(
        this Ride ride,
        double ratePerKm)
    {
        if (ratePerKm <= 0)
            throw new ArgumentException(
                "Rate per kilometer must be greater than 0."
            );

        return Math.Round(ride.Distance * ratePerKm, 2);
    }
}

// Generic Driver Matcher
public class DriverMatcher<T> where T : Driver
{
    private readonly List<T> drivers;

    public DriverMatcher(List<T> drivers)
    {
        this.drivers = drivers;
    }

    public T MatchDriver()
    {
        if (drivers.Count == 0)
            return null;

        return drivers.First();
    }

    public T MatchDriver(Func<T, bool> condition)
    {
        return drivers.FirstOrDefault(condition);
    }
}

// Main Program
public class Program
{
    public static void Main()
    {
        // Create vehicles
        Vehicle car = new Car(
            "DL01AB1234",
            "Toyota Innova"
        );

        Vehicle bike = new Bike(
            "DL05XY5678",
            "Honda Activa"
        );

        // Create drivers
        var driver1 = new Driver(
            "D001",
            "Rahul",
            30,
            car
        );

        var driver2 = new Driver(
            "D002",
            "Amit",
            28,
            bike
        );

        // Create rider
        var rider = new Rider(
            "R001",
            "Navneet"
        );

        // Driver list
        var drivers = new List<Driver>
        {
            driver1,
            driver2
        };

        // Generic driver matching
        var matcher = new DriverMatcher<Driver>(drivers);

        Driver matchedDriver = matcher.MatchDriver(
            d => d.Vehicle is Car
        );

        Console.WriteLine("Matched Driver:");
        matchedDriver.Display();

        Console.WriteLine();

        // Create ride
        var ride = new Ride(
            "RID001",
            matchedDriver,
            rider,
            15
        );

        // Calculate distance using extension method
        double calculatedDistance = ride.CalculateDistance(
            28.6139,
            77.2090,
            28.7041,
            77.1025
        );

        Console.WriteLine(
            $"Calculated Distance: {calculatedDistance} km"
        );

        // Calculate fare
        ride.Fare = ride.CalculateFare(20);

        Console.WriteLine(
            $"Calculated Fare: ₹{ride.Fare}"
        );

        Console.WriteLine();

        // Display ride
        ride.Display();

        Console.WriteLine();

        // Completed ride
        var completedRide = new CompletedRide(
            "RID002",
            driver2,
            rider,
            10
        );

        completedRide.Fare = completedRide.CalculateFare(15);

        Console.WriteLine("Completed Ride:");
        completedRide.Display();
    }
}