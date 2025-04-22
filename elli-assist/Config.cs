using System.IO;
using System.Text.Json;

public class Config
{//
    public string ActivationWord { get; set; } = "элли";
    public bool UseActivationWord { get; set; } = true;

    public static Config Load(string filePath)
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<Config>(json);
        }

       
        var defaultConfig = new Config();
        File.WriteAllText(filePath, JsonSerializer.Serialize(defaultConfig, new JsonSerializerOptions { WriteIndented = true }));
        return defaultConfig;
    }
}