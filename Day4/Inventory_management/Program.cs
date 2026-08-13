using System;
using System.Collections.Generic;
using System.Linq;



// Partial class for Product entity
public partial class Product
{
    public string SKU { get; set; }
    public string Name { get; set; }
    public int Quantity { get; set; }
    public DateTime ExpiryDate { get; set; }
    public string Warehouse { get; set; }
}

// Partial class can contain additional generated/entity logic
public partial class Product
{
    public bool IsExpired()
    {
        return ExpiryDate < DateTime.Today;
    }
}

// Generic Inventory Repository
public class InventoryRepository<T> where T : Product
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

    // Indexer by SKU
    public T this[string sku]
    {
        get
        {
            return items.FirstOrDefault(x =>
                x.SKU.Equals(sku, StringComparison.OrdinalIgnoreCase));
        }
    }
}

// Extension methods
public static class InventoryExtensions
{
    public static IEnumerable<T> GetLowStockItems<T>(
        this IEnumerable<T> items,
        int threshold = 10) where T : Product
    {
        return items.Where(x => x.Quantity < threshold);
    }

    public static IEnumerable<T> GetExpiredItems<T>(
        this IEnumerable<T> items) where T : Product
    {
        return items.Where(x => x.IsExpired());
    }
}

public class Program
{
    public static void Main()
    {
        // Object initializers
        var product1 = new Product
        {
            SKU = "P101",
            Name = "Laptop",
            Quantity = 50,
            ExpiryDate = DateTime.Now.AddYears(2),
            Warehouse = "Warehouse A"
        };

        var product2 = new Product
        {
            SKU = "P102",
            Name = "Keyboard",
            Quantity = 5,
            ExpiryDate = DateTime.Now.AddYears(1),
            Warehouse = "Warehouse B"
        };

        var product3 = new Product
        {
            SKU = "P103",
            Name = "Mouse",
            Quantity = 8,
            ExpiryDate = DateTime.Now.AddDays(-5),
            Warehouse = "Warehouse A"
        };

        var product4 = new Product
        {
            SKU = "P104",
            Name = "Monitor",
            Quantity = 25,
            ExpiryDate = DateTime.Now.AddYears(1),
            Warehouse = "Warehouse C"
        };

        // Generic repository
        var repository = new InventoryRepository<Product>();

        repository.Add(product1);
        repository.Add(product2);
        repository.Add(product3);
        repository.Add(product4);

        // Display all products
        Console.WriteLine("All Products:");
        foreach (var product in repository.GetAll())
        {
            Console.WriteLine(
                $"{product.SKU} - {product.Name} - Quantity: {product.Quantity} - Warehouse: {product.Warehouse}");
        }

        // Indexer by SKU
        Console.WriteLine("\nProduct with SKU P102:");

        var productBySku = repository["P102"];

        if (productBySku != null)
        {
            Console.WriteLine(
                $"{productBySku.SKU} - {productBySku.Name} - Quantity: {productBySku.Quantity}");
        }

        // Get low stock items
        Console.WriteLine("\nLow Stock Items:");

        var lowStockItems = repository
            .GetAll()
            .GetLowStockItems(10);

        foreach (var product in lowStockItems)
        {
            Console.WriteLine(
                $"{product.SKU} - {product.Name} - Quantity: {product.Quantity}");
        }

        // Get expired items
        Console.WriteLine("\nExpired Items:");

        var expiredItems = repository
            .GetAll()
            .GetExpiredItems();

        foreach (var product in expiredItems)
        {
            Console.WriteLine(
                $"{product.SKU} - {product.Name} - Expiry Date: {product.ExpiryDate:dd-MM-yyyy}");
        }
    }
}