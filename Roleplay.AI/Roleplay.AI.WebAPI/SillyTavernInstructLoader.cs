using System.Text.Json;

namespace Roleplay.AI.WebAPI;

public static class SillyTavernInstructLoader
{
    public static async Task<SillyTavernInstructJson> LoadAsync(string path)
    {
        if (!File.Exists(path))
            throw new FileNotFoundException($"File not found: {path}");

        var json = await File.ReadAllTextAsync(path);

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        var contextJson = JsonSerializer.Deserialize<SillyTavernInstructJson>(json, options);
        if (contextJson == null)
            throw new Exception("Failed to deserialize SillyTavernInstructJson");

        return contextJson;
    }
}