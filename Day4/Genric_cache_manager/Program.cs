using System;
using System.Collections.Generic;
using System.Linq;

//  Custom exception for invalid keys
public class InvalidKeyException : Exception
{
    public InvalidKeyException(string message) : base(message)
    {
    }
}

//  Generic CacheManager class
public class CacheManager<T>
{
    private Dictionary<string, T> cache = new Dictionary<string, T>();

    //  Add item to cache
    public void Add(string key, T value)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            throw new InvalidKeyException("Key cannot be empty.");
        }

        cache[key] = value;
    }

    // Remove item from cache
    public void Remove(string key)
    {
        if (!cache.ContainsKey(key))
        {
            throw new InvalidKeyException("Key does not exist.");
        }

        cache.Remove(key);
    }

    //  Get item by key
    public T GetByKey(string key)
    {
        if (!cache.ContainsKey(key))
        {
            throw new InvalidKeyException("Key does not exist.");
        }

        return cache[key];
    }

    //  Clear all items
    public void Clear()
    {
        cache.Clear();
    }

    // Indexer to retrieve items
    public T this[string key]
    {
        get
        {
            return GetByKey(key);
        }
    }

    // Used by extension method to access cache items
    public Dictionary<string, T> GetCache()
    {
        return cache;
    }
}

//  Extension methods
public static class CacheExtensions
{
    // Get all keys
    public static List<string> GetAllKeys<T>(this CacheManager<T> cacheManager)
    {
        return cacheManager.GetCache().Keys.ToList();
    }

    // Count expired items
    public static int CountExpiredItems<T>(
        this CacheManager<T> cacheManager,
        List<string> expiredKeys)
    {
        int count = 0;

        foreach (string key in expiredKeys)
        {
            if (cacheManager.GetCache().ContainsKey(key))
            {
                count++;
            }
        }

        return count;
    }
}

// Customer class
public class Customer
{
    public int Id { get; set; }
    public string Name { get; set; }

    public Customer(int id, string name)
    {
        Id = id;
        Name = name;
    }

    public override string ToString()
    {
        return $"Customer Id: {Id}, Name: {Name}";
    }
}

// Order class
public class Order
{
    public int Id { get; set; }
    public string Product { get; set; }

    public Order(int id, string product)
    {
        Id = id;
        Product = product;
    }

    public override string ToString()
    {
        return $"Order Id: {Id}, Product: {Product}";
    }
}

// Main program
public class Program
{
    public static void Main()
    {
        // Generic cache for integers
        CacheManager<int> intCache = new CacheManager<int>();

        intCache.Add("one", 100);
        intCache.Add("two", 200);
        intCache.Add("three", 300);

        Console.WriteLine("Integer Cache:");
        Console.WriteLine(intCache["one"]);

        // Get all keys
        List<string> intKeys = intCache.GetAllKeys();

        Console.WriteLine("\nAll Keys:");
        foreach (string key in intKeys)
        {
            Console.WriteLine(key);
        }

        // Remove item
        intCache.Remove("two");

        // Generic cache for customers
        CacheManager<Customer> customerCache =
            new CacheManager<Customer>();

        customerCache.Add(
            "customer1",
            new Customer(1, "Navneet")
        );

        customerCache.Add(
            "customer2",
            new Customer(2, "Rahul")
        );

        Console.WriteLine("\nCustomer Cache:");
        Console.WriteLine(customerCache["customer1"]);

        // Generic cache for orders
        CacheManager<Order> orderCache =
            new CacheManager<Order>();

        orderCache.Add(
            "order1",
            new Order(101, "Laptop")
        );

        orderCache.Add(
            "order2",
            new Order(102, "Mobile")
        );

        Console.WriteLine("\nOrder Cache:");
        Console.WriteLine(orderCache["order1"]);

        // Count expired items
        List<string> expiredKeys = new List<string>
        {
            "order1",
            "order3"
        };

        int expiredCount =
            orderCache.CountExpiredItems(expiredKeys);

        Console.WriteLine(
            $"\nExpired items: {expiredCount}"
        );

        // Invalid key exception
        try
        {
            Console.WriteLine(intCache["invalid"]);
        }
        catch (InvalidKeyException ex)
        {
            Console.WriteLine(
                $"\nException: {ex.Message}"
            );
        }

        // Clear cache
        intCache.Clear();

        Console.WriteLine("\nInteger cache cleared.");
    }
}