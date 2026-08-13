using System;
using System.Collections.Generic;
using System.Linq;

// Base class for all library items
public abstract class LibraryItem
{
    public int Id { get; set; }
    public string Title { get; set; }
    public bool IsAvailable { get; internal set; }

    protected LibraryItem(int id, string title)
    {
        Id = id;
        Title = title;
        IsAvailable = true;
    }

    public abstract string GetItemType();
}

// Book class
public class Book : LibraryItem
{
    public string Author { get; set; }

    public Book(int id, string title, string author)
        : base(id, title)
    {
        Author = author;
    }

    public override string GetItemType()
    {
        return "Book";
    }
}

// Magazine class
public class Magazine : LibraryItem
{
    public int IssueNumber { get; set; }

    public Magazine(int id, string title, int issueNumber)
        : base(id, title)
    {
        IssueNumber = issueNumber;
    }

    public override string GetItemType()
    {
        return "Magazine";
    }
}

// Journal class
public class Journal : LibraryItem
{
    public string Publisher { get; set; }

    public Journal(int id, string title, string publisher)
        : base(id, title)
    {
        Publisher = publisher;
    }

    public override string GetItemType()
    {
        return "Journal";
    }
}

// Generic Repository
public class Repository<T> where T : LibraryItem
{
    private readonly List<T> items = new List<T>();

    // Add item
    public void Add(T item)
    {
        items.Add(item);
    }

    // Get all items
    public List<T> GetAll()
    {
        return items;
    }

    // Find item by ID
    public T FindById(int id)
    {
        return items.FirstOrDefault(x => x.Id == id);
    }

    // Remove item
    public bool Remove(int id)
    {
        T item = FindById(id);

        if (item == null)
            return false;

        items.Remove(item);
        return true;
    }

    // Generic repository indexer
    public T this[int index]
    {
        get
        {
            return items[index];
        }
        set
        {
            items[index] = value;
        }
    }
}

// First part of Library partial class
public partial class Library
{
    private readonly Repository<Book> books;
    private readonly Repository<Magazine> magazines;
    private readonly Repository<Journal> journals;

    public Library()
    {
        books = new Repository<Book>();
        magazines = new Repository<Magazine>();
        journals = new Repository<Journal>();
    }

    // Library indexer
    public LibraryItem this[string title]
    {
        get
        {
            LibraryItem item = books.GetAll()
                .FirstOrDefault(x =>
                    x.Title.Equals(title, StringComparison.OrdinalIgnoreCase));

            if (item != null)
                return item;

            item = magazines.GetAll()
                .FirstOrDefault(x =>
                    x.Title.Equals(title, StringComparison.OrdinalIgnoreCase));

            if (item != null)
                return item;

            item = journals.GetAll()
                .FirstOrDefault(x =>
                    x.Title.Equals(title, StringComparison.OrdinalIgnoreCase));

            return item;
        }
    }

    // Add book
    public void AddBook(Book book)
    {
        books.Add(book);
    }

    // Add magazine
    public void AddMagazine(Magazine magazine)
    {
        magazines.Add(magazine);
    }

    // Add journal
    public void AddJournal(Journal journal)
    {
        journals.Add(journal);
    }

    // Borrow item
    public bool Borrow(string title)
    {
        LibraryItem item = this[title];

        if (item == null)
        {
            Console.WriteLine("Item not found.");
            return false;
        }

        if (!item.IsAvailable)
        {
            Console.WriteLine("Item is already borrowed.");
            return false;
        }

        item.IsAvailable = false;

        Console.WriteLine($"{item.Title} borrowed successfully.");
        return true;
    }

    // Return item
    public bool Return(string title)
    {
        LibraryItem item = this[title];

        if (item == null)
        {
            Console.WriteLine("Item not found.");
            return false;
        }

        if (item.IsAvailable)
        {
            Console.WriteLine("Item is already available.");
            return false;
        }

        item.IsAvailable = true;

        Console.WriteLine($"{item.Title} returned successfully.");
        return true;
    }
}

// Second part of Library partial class
public partial class Library
{
    // Search items
    public List<LibraryItem> Search(string keyword)
    {
        List<LibraryItem> result = new List<LibraryItem>();

        result.AddRange(
            books.GetAll().Where(x =>
                x.Title.Contains(keyword, StringComparison.OrdinalIgnoreCase))
        );

        result.AddRange(
            magazines.GetAll().Where(x =>
                x.Title.Contains(keyword, StringComparison.OrdinalIgnoreCase))
        );

        result.AddRange(
            journals.GetAll().Where(x =>
                x.Title.Contains(keyword, StringComparison.OrdinalIgnoreCase))
        );

        return result;
    }

    // Display all items
    public void DisplayAll()
    {
        Console.WriteLine("\nBOOKS");

        foreach (Book book in books.GetAll())
        {
            Console.WriteLine(
                $"{book.Id} | {book.Title} | {book.Author} | Available: {book.IsAvailable}"
            );
        }

        Console.WriteLine("\nMAGAZINES");

        foreach (Magazine magazine in magazines.GetAll())
        {
            Console.WriteLine(
                $"{magazine.Id} | {magazine.Title} | Issue: {magazine.IssueNumber} | Available: {magazine.IsAvailable}"
            );
        }

        Console.WriteLine("\nJOURNALS");

        foreach (Journal journal in journals.GetAll())
        {
            Console.WriteLine(
                $"{journal.Id} | {journal.Title} | Publisher: {journal.Publisher} | Available: {journal.IsAvailable}"
            );
        }
    }
}

// Extension methods
public static class LibraryExtensions
{
    // Get available books
    public static IEnumerable<Book> GetAvailableBooks(
        this IEnumerable<Book> books)
    {
        return books.Where(book => book.IsAvailable);
    }
}

// Main program
public class Program
{
    public static void Main()
    {
        Library library = new Library();

        // Add books
        library.AddBook(
            new Book(1, "Clean Code", "Robert C. Martin")
        );

        library.AddBook(
            new Book(2, "C# in Depth", "Jon Skeet")
        );

        library.AddBook(
            new Book(3, "The Pragmatic Programmer", "Andrew Hunt")
        );

        // Add magazines
        library.AddMagazine(
            new Magazine(4, "Tech Today", 101)
        );

        library.AddMagazine(
            new Magazine(5, "Science Weekly", 202)
        );

        // Add journals
        library.AddJournal(
            new Journal(6, "AI Research Journal", "Springer")
        );

        library.AddJournal(
            new Journal(7, "Computer Science Journal", "IEEE")
        );

        // Display all items
        library.DisplayAll();

        // Indexer example
        Console.WriteLine("\nINDEXER");

        LibraryItem item = library["Clean Code"];

        if (item != null)
        {
            Console.WriteLine(
                $"Found: {item.Title} ({item.GetItemType()})"
            );
        }

        // Borrow
        Console.WriteLine("\nBORROW");

        library.Borrow("Clean Code");

        // Return
        Console.WriteLine("\nRETURN");

        library.Return("Clean Code");

        // Search
        Console.WriteLine("\nSEARCH");

        List<LibraryItem> searchResults =
            library.Search("Code");

        foreach (LibraryItem result in searchResults)
        {
            Console.WriteLine(
                $"{result.Title} - {result.GetItemType()}"
            );
        }

        // Extension method
        Console.WriteLine("\nEXTENSION METHOD");

        Repository<Book> bookRepository =
            new Repository<Book>();

        bookRepository.Add(
            new Book(
                10,
                "Clean Architecture",
                "Robert C. Martin"
            )
        );

        bookRepository.Add(
            new Book(
                11,
                "Design Patterns",
                "Gang of Four"
            )
        );

        IEnumerable<Book> availableBooks =
            bookRepository.GetAll().GetAvailableBooks();

        foreach (Book book in availableBooks)
        {
            Console.WriteLine(
                $"{book.Title} - Available"
            );
        }

        // Generic repository indexer
        Console.WriteLine("\nGENERIC REPOSITORY INDEXER");

        Console.WriteLine(
            $"First Book: {bookRepository[0].Title}"
        );

        // Access modifier
        Console.WriteLine("\nACCESS MODIFIER");

        Console.WriteLine(
            "IsAvailable can be read publicly."
        );

        Console.WriteLine(
            "IsAvailable can only be changed internally."
        );
    }
}