using System.Text.Json.Serialization;

namespace Roleplay.AI.WebAPI;

public class SillyTavernInstructJson
{
    [JsonPropertyName("system_prompt")]
    public string SystemPrompt { get; set; }

    [JsonPropertyName("input_sequence")]
    public string InputSequence { get; set; }

    [JsonPropertyName("output_sequence")]
    public string OutputSequence { get; set; }

    [JsonPropertyName("first_output_sequence")]
    public string FirstOutputSequence { get; set; }

    [JsonPropertyName("last_output_sequence")]
    public string LastOutputSequence { get; set; }

    [JsonPropertyName("system_sequence_prefix")]
    public string SystemSequencePrefix { get; set; }

    [JsonPropertyName("system_sequence_suffix")]
    public string SystemSequenceSuffix { get; set; }

    [JsonPropertyName("stop_sequence")]
    public string StopSequence { get; set; }

    [JsonPropertyName("separator_sequence")]
    public string SeparatorSequence { get; set; }

    [JsonPropertyName("wrap")]
    public bool Wrap { get; set; }

    [JsonPropertyName("macro")]
    public bool Macro { get; set; }

    [JsonPropertyName("names")]
    public bool Names { get; set; }

    [JsonPropertyName("names_force_groups")]
    public bool NamesForceGroups { get; set; }

    [JsonPropertyName("activation_regex")]
    public string ActivationRegex { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }
}