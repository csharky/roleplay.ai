using System.Text.Json;

namespace Roleplay.AI.WebAPI;

public static class SillyTavernTemplateLoader
{
    public static async Task<SillyTavernInstructJson> LoadFromFileAsync(string path)
    {
        if (!File.Exists(path))
            throw new FileNotFoundException($"File not found: {path}");

        var json = await File.ReadAllTextAsync(path);
        
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true // чтобы игнорировать кейс
        };

        // Десериализуем
        var template = JsonSerializer.Deserialize<SillyTavernInstructJson>(json, options);
        if (template == null)
            throw new Exception("Failed to deserialize SillyTavernInstructTemplate");

        return template;
    }
}