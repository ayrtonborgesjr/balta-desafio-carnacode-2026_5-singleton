using ConfigManager = ConfigurationManager.Console.Core.ConfigurationManager;

namespace ConfigurationManager.Tests;

public class ConfigurationManagerTests
{
    [Fact]
    public void Instance_ShouldReturnSameInstance_WhenCalledMultipleTimes()
    {
        // Arrange & Act
        var instance1 = ConfigManager.Instance;
        var instance2 = ConfigManager.Instance;

        // Assert
        Assert.NotNull(instance1);
        Assert.NotNull(instance2);
        Assert.Same(instance1, instance2);
    }

    [Fact]
    public void Instance_ShouldBeThreadSafe_WhenAccessedFromMultipleThreads()
    {
        // Arrange
        const int threadCount = 10;
        var instances = new ConfigManager[threadCount];
        var threads = new Thread[threadCount];

        // Act
        for (int i = 0; i < threadCount; i++)
        {
            int index = i;
            threads[i] = new Thread(() =>
            {
                instances[index] = ConfigManager.Instance;
            });
            threads[i].Start();
        }

        foreach (var thread in threads)
        {
            thread.Join();
        }

        // Assert
        var firstInstance = instances[0];
        Assert.NotNull(firstInstance);
        Assert.All(instances, instance => Assert.Same(firstInstance, instance));
    }

    [Fact]
    public void LoadConfigurations_ShouldLoadSettings_WhenCalledFirstTime()
    {
        // Arrange
        var instance = ConfigManager.Instance;

        // Act
        instance.LoadConfigurations();

        // Assert
        var databaseConnection = instance.GetSetting("DatabaseConnection");
        Assert.NotNull(databaseConnection);
        Assert.Equal("Server=localhost;Database=MyApp;", databaseConnection);
    }

    [Fact]
    public void LoadConfigurations_ShouldNotLoadTwice_WhenCalledMultipleTimes()
    {
        // Arrange
        var instance = ConfigManager.Instance;

        // Act
        instance.LoadConfigurations();
        var setting1 = instance.GetSetting("DatabaseConnection");
        
        instance.LoadConfigurations(); // Segunda chamada não deve recarregar
        var setting2 = instance.GetSetting("DatabaseConnection");

        // Assert
        Assert.Equal(setting1, setting2);
    }

    [Fact]
    public void GetSetting_ShouldReturnCorrectValue_WhenKeyExists()
    {
        // Arrange
        var instance = ConfigManager.Instance;
        instance.LoadConfigurations();

        // Act
        var apiKey = instance.GetSetting("ApiKey");
        var cacheServer = instance.GetSetting("CacheServer");
        var maxRetries = instance.GetSetting("MaxRetries");

        // Assert
        Assert.Equal("abc123xyz789", apiKey);
        Assert.Equal("redis://localhost:6379", cacheServer);
        Assert.Equal("3", maxRetries);
    }

    [Fact]
    public void GetSetting_ShouldReturnNull_WhenKeyDoesNotExist()
    {
        // Arrange
        var instance = ConfigManager.Instance;
        instance.LoadConfigurations();

        // Act
        var nonExistentSetting = instance.GetSetting("NonExistentKey");

        // Assert
        Assert.Null(nonExistentSetting);
    }

    [Fact]
    public void GetSetting_ShouldAutoLoadConfigurations_WhenNotLoadedYet()
    {
        // Arrange
        var instance = ConfigManager.Instance;
        // Não chamamos LoadConfigurations() explicitamente

        // Act
        var setting = instance.GetSetting("EnableLogging");

        // Assert
        Assert.NotNull(setting);
        Assert.Equal("true", setting);
    }

    [Fact]
    public void UpdateSetting_ShouldUpdateExistingValue_WhenKeyExists()
    {
        // Arrange
        var instance = ConfigManager.Instance;
        instance.LoadConfigurations();
        var originalValue = instance.GetSetting("LogLevel");

        // Act
        instance.UpdateSetting("LogLevel", "Debug");
        var updatedValue = instance.GetSetting("LogLevel");

        // Assert
        Assert.Equal("Information", originalValue);
        Assert.Equal("Debug", updatedValue);
    }

    [Fact]
    public void UpdateSetting_ShouldAddNewSetting_WhenKeyDoesNotExist()
    {
        // Arrange
        var instance = ConfigManager.Instance;
        instance.LoadConfigurations();

        // Act
        instance.UpdateSetting("NewSetting", "NewValue");
        var newValue = instance.GetSetting("NewSetting");

        // Assert
        Assert.NotNull(newValue);
        Assert.Equal("NewValue", newValue);
    }

    [Fact]
    public void UpdateSetting_ShouldBeThreadSafe_WhenCalledFromMultipleThreads()
    {
        // Arrange
        var instance = ConfigManager.Instance;
        instance.LoadConfigurations();
        const int threadCount = 20;
        var threads = new Thread[threadCount];

        // Act
        for (int i = 0; i < threadCount; i++)
        {
            int index = i;
            threads[i] = new Thread(() =>
            {
                instance.UpdateSetting($"ThreadSetting{index}", $"Value{index}");
            });
            threads[i].Start();
        }

        foreach (var thread in threads)
        {
            thread.Join();
        }

        // Assert
        for (int i = 0; i < threadCount; i++)
        {
            var value = instance.GetSetting($"ThreadSetting{i}");
            Assert.NotNull(value);
            Assert.Equal($"Value{i}", value);
        }
    }

    [Fact]
    public void ConfigurationManager_ShouldLoadAllExpectedSettings()
    {
        // Arrange
        var instance = ConfigManager.Instance;
        instance.LoadConfigurations();

        // Act & Assert
        Assert.Equal("Server=localhost;Database=MyApp;", instance.GetSetting("DatabaseConnection"));
        Assert.Equal("abc123xyz789", instance.GetSetting("ApiKey"));
        Assert.Equal("redis://localhost:6379", instance.GetSetting("CacheServer"));
        Assert.Equal("3", instance.GetSetting("MaxRetries"));
        Assert.Equal("30", instance.GetSetting("TimeoutSeconds"));
        Assert.Equal("true", instance.GetSetting("EnableLogging"));
        Assert.Equal("Information", instance.GetSetting("LogLevel"));
    }
}

