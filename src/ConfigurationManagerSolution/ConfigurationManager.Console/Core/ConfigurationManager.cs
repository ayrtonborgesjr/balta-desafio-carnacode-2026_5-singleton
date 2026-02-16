namespace ConfigurationManager.Console.Core;

public sealed class ConfigurationManager
{
    private static readonly Lazy<ConfigurationManager> _instance =
        new Lazy<ConfigurationManager>(() => new ConfigurationManager());

    public static ConfigurationManager Instance => _instance.Value;

    private Dictionary<string, string> _settings;
    private bool _isLoaded;
    private readonly object _lock = new object();

    private ConfigurationManager()
    {
        _settings = new Dictionary<string, string>();
        _isLoaded = false;
        System.Console.WriteLine("🟢 Singleton ConfigurationManager criado!");
    }

    public void LoadConfigurations()
    {
        if (_isLoaded) return;

        lock (_lock)
        {
            if (_isLoaded) return;

            System.Console.WriteLine("🔄 Carregando configurações...");
            Thread.Sleep(200);

            _settings["DatabaseConnection"] = "Server=localhost;Database=MyApp;";
            _settings["ApiKey"] = "abc123xyz789";
            _settings["CacheServer"] = "redis://localhost:6379";
            _settings["MaxRetries"] = "3";
            _settings["TimeoutSeconds"] = "30";
            _settings["EnableLogging"] = "true";
            _settings["LogLevel"] = "Information";

            _isLoaded = true;
            System.Console.WriteLine("✅ Configurações carregadas!\n");
        }
    }

    public string? GetSetting(string key)
    {
        if (!_isLoaded)
            LoadConfigurations();

        return _settings.TryGetValue(key, out var value)
            ? value
            : null;
    }

    public void UpdateSetting(string key, string value)
    {
        lock (_lock)
        {
            _settings[key] = value;
        }
    }
}