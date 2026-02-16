namespace ConfigurationManager.Console.Services;

public class DatabaseService
{
    public void Connect()
    {
        var connectionString =
            Core.ConfigurationManager.Instance.GetSetting("DatabaseConnection");

        System.Console.WriteLine($"[DatabaseService] {connectionString}");
    }
}