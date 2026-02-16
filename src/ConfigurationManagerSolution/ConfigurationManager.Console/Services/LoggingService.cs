namespace ConfigurationManager.Console.Services;

public class LoggingService
{
    public void Log(string message)
    {
        var level =
            Core.ConfigurationManager.Instance.GetSetting("LogLevel");

        System.Console.WriteLine($"[{level}] {message}");
    }
}