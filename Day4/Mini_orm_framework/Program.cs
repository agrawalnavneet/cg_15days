using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MiniORM.Entities
{
    public partial class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Department { get; set; }
        public decimal Salary { get; set; }
    }

    public partial class Order
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public decimal Amount { get; set; }
    }

    public partial class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }
}

namespace MiniORM.Data
{
    using MiniORM.Entities;

    public class Database
    {
        private readonly Dictionary<Type, List<object>> tables;

        public Database()
        {
            tables = new Dictionary<Type, List<object>>();

            tables[typeof(Employee)] = new List<object>();
            tables[typeof(Order)] = new List<object>();
            tables[typeof(Customer)] = new List<object>();
        }

        public void Save<T>(T entity)
        {
            Type type = typeof(T);

            if (!tables.ContainsKey(type))
            {
                tables[type] = new List<object>();
            }

            List<object> table = tables[type];

            PropertyInfo idProperty = type.GetProperty("Id");

            if (idProperty == null)
            {
                throw new Exception("Entity must have an Id property.");
            }

            int id = (int)idProperty.GetValue(entity);

            object existingEntity = table.FirstOrDefault(item =>
            {
                int existingId =
                    (int)idProperty.GetValue(item);

                return existingId == id;
            });

            if (existingEntity != null)
            {
                table.Remove(existingEntity);
            }

            table.Add(entity);

            Console.WriteLine(
                $"{type.Name} with Id {id} saved successfully."
            );
        }

        public T Get<T>(int id)
        {
            Type type = typeof(T);

            if (!tables.ContainsKey(type))
            {
                return default(T);
            }

            PropertyInfo idProperty = type.GetProperty("Id");

            object entity = tables[type].FirstOrDefault(item =>
            {
                int existingId =
                    (int)idProperty.GetValue(item);

                return existingId == id;
            });

            if (entity == null)
            {
                Console.WriteLine(
                    $"{type.Name} with Id {id} not found."
                );

                return default(T);
            }

            return (T)entity;
        }

        public void Delete<T>(int id)
        {
            Type type = typeof(T);

            if (!tables.ContainsKey(type))
            {
                Console.WriteLine("Entity type not found.");
                return;
            }

            PropertyInfo idProperty = type.GetProperty("Id");

            object entity = tables[type].FirstOrDefault(item =>
            {
                int existingId =
                    (int)idProperty.GetValue(item);

                return existingId == id;
            });

            if (entity != null)
            {
                tables[type].Remove(entity);

                Console.WriteLine(
                    $"{type.Name} with Id {id} deleted successfully."
                );
            }
            else
            {
                Console.WriteLine(
                    $"{type.Name} with Id {id} not found."
                );
            }
        }

        public List<T> GetAll<T>()
        {
            Type type = typeof(T);

            if (!tables.ContainsKey(type))
            {
                return new List<T>();
            }

            return tables[type]
                .Cast<T>()
                .ToList();
        }
    }
}

namespace MiniORM
{
    using MiniORM.Data;
    using MiniORM.Entities;

    class Program
    {
        static void Main(string[] args)
        {
            Database db = new Database();

            // Object initializer for Employee
            Employee employee = new Employee
            {
                Id = 1,
                Name = "Navneet",
                Department = "Engineering",
                Salary = 50000
            };

            // Save employee
            db.Save(employee);

            // Object initializer for another Employee
            Employee employee2 = new Employee
            {
                Id = 2,
                Name = "Rahul",
                Department = "HR",
                Salary = 45000
            };

            db.Save(employee2);

            // Get Employee
            Employee result = db.Get<Employee>(1);

            Console.WriteLine("\nEmployee Details:");

            if (result != null)
            {
                Console.WriteLine($"Id: {result.Id}");
                Console.WriteLine($"Name: {result.Name}");
                Console.WriteLine($"Department: {result.Department}");
                Console.WriteLine($"Salary: {result.Salary}");
            }

            // Object initializer for Order
            Order order = new Order
            {
                Id = 5,
                ProductName = "Laptop",
                Amount = 75000
            };

            // Save Order
            db.Save(order);

            // Delete Order
            db.Delete<Order>(5);

            // Add Customers
            db.Save(new Customer
            {
                Id = 1,
                Name = "Amit",
                Email = "amit@example.com"
            });

            db.Save(new Customer
            {
                Id = 2,
                Name = "Priya",
                Email = "priya@example.com"
            });

            // GetAll<Customer>()
            List<Customer> customers =
                db.GetAll<Customer>();

            Console.WriteLine("\n===== ALL CUSTOMERS =====");

            foreach (Customer customer in customers)
            {
                Console.WriteLine(
                    $"Id: {customer.Id}, " +
                    $"Name: {customer.Name}, " +
                    $"Email: {customer.Email}"
                );
            }

            Console.WriteLine("\nProgram completed.");
        }
    }
}