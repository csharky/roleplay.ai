using System.Text.Json;
using Microsoft.Extensions.AI;

namespace Roleplay.AI.WebAPI;

public static class ChatOptionsLoader
{
    public static async Task<ChatOptions?> Load()
    {
        var stream = new FileStream(Path.Combine(AppContext.BaseDirectory, "ChatOptions.json"), FileMode.Open);
        return await JsonSerializer.DeserializeAsync<ChatOptions>(stream);
    }
}