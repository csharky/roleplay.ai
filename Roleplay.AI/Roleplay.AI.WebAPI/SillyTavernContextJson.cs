using System.Text.Json.Serialization;

namespace Roleplay.AI.WebAPI;

public class SillyTavernContextJson
{
    [JsonPropertyName("story_string")]
    public string StoryString { get; set; }
    
    [JsonPropertyName("example_separator")]
    public string ExampleSeparator { get; set; }
    
    [JsonPropertyName("chat_start")]
    public string ChatStart { get; set; }

    [JsonPropertyName("always_force_name2")]
    public bool AlwaysForceName2 { get; set; }

    [JsonPropertyName("trim_sentences")]
    public bool TrimSentences { get; set; }

    [JsonPropertyName("include_newline")]
    public bool IncludeNewline { get; set; }

    [JsonPropertyName("single_line")]
    public bool SingleLine { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }
}