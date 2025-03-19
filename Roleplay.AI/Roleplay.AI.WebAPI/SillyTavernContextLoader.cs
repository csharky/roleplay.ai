using System.Text.Json;

namespace Roleplay.AI.WebAPI;

public static class SillyTavernContextLoader
{
    public static async Task<SillyTavernContextJson> LoadAsync(string path)
    {
        if (!File.Exists(path))
            throw new FileNotFoundException($"File not found: {path}");

        var json = await File.ReadAllTextAsync(path);

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        var contextJson = JsonSerializer.Deserialize<SillyTavernContextJson>(json, options);
        if (contextJson == null)
            throw new Exception("Failed to deserialize SillyTavernContextJson");

        return contextJson;
    }
}