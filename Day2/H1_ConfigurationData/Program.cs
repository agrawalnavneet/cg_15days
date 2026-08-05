using System;
using System.Collections.Generic;

public class ConfigurationData
{
    public Dictionary<string, string> Settings { get; } = new Dictionary<string, string>();
    public string SourceName { get; set; }

    public override string ToString() =>
        $"[{SourceName}] {Settings.Count} setting(s) loaded.";
}

public interface IConfigurationSource
{
    string Name { get; }
    bool TryLoad(out ConfigurationData config);
}

public class EnvironmentVariableSource : IConfigurationSource
{
    public string Name => "EnvironmentVariableSource";

    public bool TryLoad(out ConfigurationData config)
    {
        config = null;

        var value = Environment.GetEnvironmentVariable("APP_CONFIG");
        if (string.IsNullOrEmpty(value))
        {
            return false;
        }

        config = new ConfigurationData { SourceName = Name };
        config.Settings["APP_CONFIG"] = value;
        return true;
    }
}

public class JsonFileSource : IConfigurationSource
{
    private readonly string _filePath;
    public string Name => "JsonFileSource";

    public JsonFileSource(string filePath) => _filePath = filePath;

    public bool TryLoad(out ConfigurationData config)
    {
        config = null;

        if (!System.IO.File.Exists(_filePath))
        {
            return false;
        }

        config = new ConfigurationData { SourceName = Name };
        return true;
    }
}

public class DatabaseSource : IConfigurationSource
{
    public string Name => "DatabaseSource";

    public bool TryLoad(out ConfigurationData config)
    {
        config = new ConfigurationData { SourceName = Name };
        config.Settings["ConnectionTimeout"] = "30";
        config.Settings["FeatureFlagX"] = "true";
        return true;
    }
}

public static class ConfigurationLoader
{
    public static ConfigurationData Load(params IConfigurationSource[] sources)
    {
        if (sources == null || sources.Length == 0)
            throw new ArgumentException("At least one configuration source must be provided.");

        foreach (var source in sources)
        {
            Console.WriteLine($"Attempting to load from {source.Name}...");

            if (source.TryLoad(out ConfigurationData config))
            {
                Console.WriteLine($"Success: loaded configuration from {source.Name}.");
                return config;
            }

            Console.WriteLine($"  -> {source.Name} did not provide configuration. Falling back...");
        }

        throw new InvalidOperationException("No configuration source was able to provide configuration.");
    }
}

class Program
{
    static void Main()
    {
        var result = ConfigurationLoader.Load(
            new EnvironmentVariableSource(),
            new JsonFileSource("config.json"),
            new DatabaseSource()
        );

        Console.WriteLine(result);
    }
}