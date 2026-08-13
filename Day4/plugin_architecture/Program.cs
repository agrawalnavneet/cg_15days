using System;
using System.Linq;
using System.Reflection;

namespace PluginSystem
{
    // Interface for all plugins
    public interface IPlugin
    {
        void Execute();
    }

    // Tax Plugin
    namespace TaxPlugins
    {
        public class TaxPlugin : IPlugin
        {
            public void Execute()
            {
                Console.WriteLine("Tax Plugin Executed");
            }

            // Plugin internal data
            private void CalculateTax()
            {
                Console.WriteLine("Calculating Tax...");
            }
        }
    }

    // Payment Plugin
    namespace PaymentPlugins
    {
        public class PaymentPlugin : IPlugin
        {
            public void Execute()
            {
                Console.WriteLine("Payment Plugin Executed");
            }

            // Plugin internal data
            private void ProcessPayment()
            {
                Console.WriteLine("Processing Payment...");
            }
        }
    }

    // Logging Plugin
    namespace LoggingPlugins
    {
        public class LoggingPlugin : IPlugin
        {
            public void Execute()
            {
                Console.WriteLine("Logging Plugin Executed");
            }

            // Plugin internal data
            private void WriteLog()
            {
                Console.WriteLine("Writing Log...");
            }
        }
    }

    // Generic Plugin Loader
    public class PluginLoader<T>
    {
        // Load all plugins from an assembly
        public static void Load()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();

            Console.WriteLine("Loading plugins...\n");

            var pluginTypes = assembly
                .GetTypes()
                .Where(type =>
                    typeof(T).IsAssignableFrom(type) &&
                    type.IsClass &&
                    !type.IsAbstract)
                .ToList();

            foreach (var type in pluginTypes)
            {
                T plugin = (T)Activator.CreateInstance(type)!;

                Console.WriteLine($"Plugin: {type.Name}");
                plugin!.GetType()
                    .GetMethod("Execute")!
                    .Invoke(plugin, null);

                Console.WriteLine();
            }
        }
    }

    // Main Program
    internal class Program
    {
        static void Main(string[] args)
        {
            // Generic plugin loader
            PluginLoader<IPlugin>.Load();

            Console.WriteLine("All plugins loaded successfully.");
        }
    }
}