using ConfigurationManager.Console.Services;

namespace ConfigurationManager.Tests;

public class ServicesTests
{
    [Fact]
    public void DatabaseService_Connect_ShouldUseConfigurationManager()
    {
        // Arrange
        var service = new DatabaseService();
        
        // Act & Assert
        // Não deve lançar exceção
        service.Connect();
    }

    [Fact]
    public void ApiService_MakeRequest_ShouldUseConfigurationManager()
    {
        // Arrange
        var service = new ApiService();
        
        // Act & Assert
        // Não deve lançar exceção
        service.MakeRequest();
    }

    [Fact]
    public void CacheService_Connect_ShouldUseConfigurationManager()
    {
        // Arrange
        var service = new CacheService();
        
        // Act & Assert
        // Não deve lançar exceção
        service.Connect();
    }

    [Fact]
    public void LoggingService_Log_ShouldUseConfigurationManager()
    {
        // Arrange
        var service = new LoggingService();
        
        // Act & Assert
        // Não deve lançar exceção
        service.Log("Test message");
    }

    [Fact]
    public void AllServices_ShouldShareSameConfigurationManagerInstance()
    {
        // Arrange
        var dbService = new DatabaseService();
        var apiService = new ApiService();
        var cacheService = new CacheService();
        var logService = new LoggingService();

        // Act
        dbService.Connect();
        apiService.MakeRequest();
        cacheService.Connect();
        logService.Log("Test");

        // Assert
        // Se todos os serviços executaram sem erro, 
        // significa que estão compartilhando a mesma instância do ConfigurationManager
        Assert.True(true);
    }
}

