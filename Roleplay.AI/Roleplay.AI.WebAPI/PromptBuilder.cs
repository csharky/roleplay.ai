namespace Roleplay.AI.WebAPI;

public static class PromptBuilder
{
    /// <summary>
    /// Собирает промпт для "первого ответа" ассистента (или если вы хотите всегда вызывать "first")
    /// </summary>
    public static string BuildFirstPrompt(SillyTavernInstructJson instruct, string userInput)
    {
        var systemPrefix = instruct.SystemSequencePrefix ?? "";
        var systemSuffix = instruct.SystemSequenceSuffix ?? "";
        var inputSeq = instruct.InputSequence ?? "";
        var firstOutSeq = instruct.FirstOutputSequence ?? "";

        // 1. "system_prompt" + обёртки (если заданы)
        // 2. userInput
        // 3. input_sequence
        // 4. first_output_sequence
        return $"{systemPrefix}{instruct.SystemPrompt}{systemSuffix}" +
               $"{userInput}{inputSeq}" +
               $"{firstOutSeq}";
    }

    /// <summary>
    /// Собирает промпт для "последующих" ответов ассистента
    /// </summary>
    public static string BuildNextPrompt(SillyTavernInstructJson instruct, string userInput)
    {
        var systemPrefix = instruct.SystemSequencePrefix ?? "";
        var systemSuffix = instruct.SystemSequenceSuffix ?? "";
        var inputSeq = instruct.InputSequence ?? "";
        var outSeq = instruct.OutputSequence ?? "";

        return $"{systemPrefix}{instruct.SystemPrompt}{systemSuffix}" +
               $"{userInput}{inputSeq}" +
               $"{outSeq}";
    }
}