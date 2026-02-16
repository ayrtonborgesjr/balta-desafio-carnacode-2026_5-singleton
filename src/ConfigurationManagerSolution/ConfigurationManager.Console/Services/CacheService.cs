namespace ConfigurationManager.Console.Services;

public class CacheService
{
    public void Connect()
    {
        var cache =
            Core.ConfigurationManager.Instance.GetSetting("CacheServer");

        System.Console.WriteLine($"[CacheService] {cache}");
    }
}