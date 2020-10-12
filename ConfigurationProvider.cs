
using Microsoft.Extensions.Configuration;

public static class ConfigurationProvider
{

    private static ConfigEntity _config;
    public static ConfigEntity Get()
    {
        if (_config != null) return _config;

        var c = new ConfigurationBuilder()
             .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
             .Build();

        _config = new ConfigEntity();
        c.Bind(_config);
        return _config;
    }
}

public class ConfigEntity
{
    public ConfigEntityHeaders Headers { get; set; }
    public string TokenSecret { get; set; }
    public string CodeCharacters { get; set; }
    public int CodeLength { get; set; }
    public string FirebaseApiWebKey { get; set; }
}

public class ConfigEntityHeaders
{
    public string Token { get; set; }
    public string TokenId { get; set; }
}