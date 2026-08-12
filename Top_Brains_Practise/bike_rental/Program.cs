using System;
using System.Collections.Generic;
using System.Linq;

public class Bike
{
    public string Model { get; set; }
    public int PricePerDay { get; set; }
    public string Brand { get; set; }

    public Bike(string model, string brand, int pricePerDay)
    {
        Model = model;
        Brand = brand;
        PricePerDay = pricePerDay;
    }
}

public class BikeUtility
{
    public void AddBikeDetails(string model, string brand, int pricePerDay)
    {
        int key = Program.bikeDetails.Count + 1;

        Bike bike = new Bike(model, brand, pricePerDay);

        Program.bikeDetails.Add(key, bike);
    }

    public SortedDictionary<string, List<Bike>> GroupBikesByBrand()
    {
        SortedDictionary<string, List<Bike>> groupedBikes =
            new SortedDictionary<string, List<Bike>>();

        foreach (Bike bike in Program.bikeDetails.Values)
        {
            if (!groupedBikes.ContainsKey(bike.Brand))
            {
                groupedBikes[bike.Brand] = new List<Bike>();
            }

            groupedBikes[bike.Brand].Add(bike);
        }

        return groupedBikes;
    }
}

public class Program
{
    public static SortedDictionary<int, Bike> bikeDetails =
        new SortedDictionary<int, Bike>();

    public static void Main(string[] args)
    {
        BikeUtility utility = new BikeUtility();

        while (true)
        {
            Console.WriteLine("1. Add Bike Details");
            Console.WriteLine("2. Group Bikes By Brand");
            Console.WriteLine("3. Exit");

            Console.Write("Enter your choice: ");
            int choice = int.Parse(Console.ReadLine());

            if (choice == 1)
            {
                Console.Write("Enter the model: ");
                string model = Console.ReadLine();

                Console.Write("Enter the brand: ");
                string brand = Console.ReadLine();

                Console.Write("Enter the price per day: ");
                int pricePerDay = int.Parse(Console.ReadLine());

                utility.AddBikeDetails(model, brand, pricePerDay);

                Console.WriteLine("Bike details added successfully");
                Console.WriteLine();
            }
            else if (choice == 2)
            {
                SortedDictionary<string, List<Bike>> grouped =
                    utility.GroupBikesByBrand();

                foreach (var group in grouped)
                {
                    foreach (Bike bike in group.Value)
                    {
                        Console.WriteLine($"{bike.Brand} {bike.Model}");
                    }
                }

                Console.WriteLine();
            }
            else if (choice == 3)
            {
                break;
            }
        }
    }
}