using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }

    public Product(int id, string name, decimal price)
    {
        Id = id;
        Name = name;
        Price = price;
    }

    public override string ToString()
    {
        return $"{Id} - {Name} - {Price}";
    }
}

public class ShoppingCart<T> : IEnumerable<T>
{
    private readonly List<T> items = new List<T>();

    public int Count
    {
        get { return items.Count; }
    }

    public T this[int index]
    {
        get { return items[index]; }
        set { items[index] = value; }
    }

    public void AddItem(T item)
    {
        items.Add(item);
    }

    public void RemoveItem(T item)
    {
        items.Remove(item);
    }

    public IEnumerator<T> GetEnumerator()
    {
        return items.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}

public static class ShoppingCartExtensions
{
    public static decimal GetTotalPrice(this ShoppingCart<Product> cart)
    {
        return cart.Sum(product => product.Price);
    }

    public static decimal ApplyDiscount(
        this ShoppingCart<Product> cart,
        decimal discount)
    {
        decimal total = cart.GetTotalPrice();

        if (discount < 0)
        {
            discount = 0;
        }

        if (discount > total)
        {
            discount = total;
        }

        return total - discount;
    }
}

class Program
{
    static void Main(string[] args)
    {
        // Create a generic shopping cart
        ShoppingCart<Product> cart = new ShoppingCart<Product>();

        // Add items to cart
        cart.AddItem(new Product(1, "Laptop", 2000));
        cart.AddItem(new Product(2, "Mobile", 1000));
        cart.AddItem(new Product(3, "Headphones", 500));
        cart.AddItem(new Product(4, "Keyboard", 500));
        cart.AddItem(new Product(5, "Mouse", 500));

        // Access item using indexer
        Console.WriteLine("First Item:");
        Console.WriteLine(cart[0]);

        // Calculate total price
        decimal total = cart.GetTotalPrice();

        // Apply discount using extension method
        decimal discount = 500;
        decimal finalPrice = cart.ApplyDiscount(discount);

        // Anonymous type for invoice summary
        var invoice = new
        {
            ItemCount = cart.Count,
            Total = total,
            Discount = discount,
            FinalAmount = finalPrice
        };

        Console.WriteLine();
        Console.WriteLine("===== SHOPPING CART =====");

        foreach (Product product in cart)
        {
            Console.WriteLine(product);
        }

        Console.WriteLine();
        Console.WriteLine("===== INVOICE SUMMARY =====");

        Console.WriteLine("{");
        Console.WriteLine($"    ItemCount = {invoice.ItemCount},");
        Console.WriteLine($"    Total = {invoice.Total},");
        Console.WriteLine($"    Discount = {invoice.Discount},");
        Console.WriteLine($"    FinalAmount = {invoice.FinalAmount}");
        Console.WriteLine("}");

        // Remove an item
        cart.RemoveItem(cart[4]);

        Console.WriteLine();
        Console.WriteLine("After removing last item:");

        foreach (Product product in cart)
        {
            Console.WriteLine(product);
        }
    }
}