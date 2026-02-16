namespace ConfigurationManager.Console.Services;

public class ApiService
{
    public void MakeRequest()
    {
        var apiKey =
            Core.ConfigurationManager.Instance.GetSetting("ApiKey");

        System.Console.WriteLine($"[ApiService] {apiKey}");
    }
}