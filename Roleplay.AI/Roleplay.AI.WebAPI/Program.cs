using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.AI;
using Roleplay.AI.WebAPI;
using Roleplay.AI.WebAPI.Configurations;
using Roleplay.AI.WebAPI.Requests;
using Scalar.AspNetCore;
using ChatResponse = Roleplay.AI.WebAPI.Responses.ChatResponse;

var builder = WebApplication.CreateBuilder(args);

var chatClientOptions = builder.Configuration.GetSection("ChatClient").Get<ChatClientOptions>();
if (chatClientOptions is null)
{
    throw new NullReferenceException("ChatClientOptions is null");
}

builder.Services.AddOpenApi();
builder.Services.AddHealthChecks();
builder.Services.AddChatClient(new OllamaChatClient(chatClientOptions.Url, chatClientOptions.ModelId));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.MapGet("/hi", async () => "hi");

app.MapPost("/chat", async (ChatRequest request, IChatClient chatClient) =>
{
    var context = await SillyTavernContextLoader.LoadAsync("./context.json");
    var instruct = await SillyTavernInstructLoader.LoadAsync("./instruct.json");

    var data = new StoryData
    {
        character = "Алина",
        user = "Максим",
        system = "Ты ведёшь роль Алины в вымышленной беседе между Максимом и Алиной. Пиши только реплики и действия своего персонажа. Не пиши реплики персонажа Максим. Ты говоришь только на русском языке и совершенно не понимаешь английского языка. Используй описание своего персонажа, сценарий, память и контекст для точного воспроизведения характера.\nВАЖНО:\nПиши только реплики и действия своего персонажа (Алина).\nНикогда не пиши реплики за персонажа Максим.\nВ своих ответах ни при каких обстоятельствах не пиши реплики за Максим. Отвечай только от имени Алина.\nНе продолжай диалог от имени Максим ни при каких условиях.",
        wiBefore = "Информация о мире до: Это фэнтезийный мир.",
        description = "Алина — молодая волшебница из древней деревни Сосновка. У неё длинные русые волосы, зелёные глаза, и лёгкая улыбка. Алина любит природу, умеет общаться с животными, и терпеть не может огонь.",
        personality = "Добрая, дружелюбная, но немного вспыльчивая и обидчивая. Старается избегать конфликтов и магии огня.",
        scenario = "Жутковатый лес ночью. {{user}} заблудился и встретил {{character}}",
        persona = "Максим — начинающий волшебник, который заблудился в лесу. Он немного растерян и впервые оказался в такой ситуации."
        // wiAfter не заполняем — проверим, что {{#if wiAfter}} не выводится
    };

    data.scenario = HandlebarsHelper.RenderHandlebars(data.scenario, data);
    
    var storyPrompt = HandlebarsHelper.RenderHandlebars(context.StoryString, data);
    var chatStartPrompt = HandlebarsHelper.RenderHandlebars(context.ChatStart, data);
    var stringBuilder = new StringBuilder();
    stringBuilder.AppendLine($"###{data.user}:");
    stringBuilder.AppendLine($"{request.Text}");
    stringBuilder.AppendLine();
    stringBuilder.AppendLine(chatStartPrompt);
    stringBuilder.AppendLine($"###{data.character}:");
    
    app.Logger.LogInformation(stringBuilder.ToString());
    
    var messages = new List<ChatMessage>()
    {
        new ChatMessage()
        {
            Role  = ChatRole.System,
            Text = instruct.SystemPrompt
        },
        new ChatMessage()
        {
            Role = ChatRole.System,
            Text = storyPrompt,
        },
        new ChatMessage()
        {
            Role = ChatRole.User,
            Text = stringBuilder.ToString(),
        }
    };
    
    messages.AddRange(messages);

    app.Logger.LogInformation(JsonSerializer.Serialize(messages));

    var options = await ChatOptionsLoader.Load(); 
    options.StopSequences ??= new List<string>();
    options.StopSequences.Add($"###{data.user}:");
    
    var responseAsync = await chatClient.GetResponseAsync(messages, options);
    var botResponse = responseAsync.Choices[0].Text;
    var correctedBotResponse = botResponse?.Replace("\n", string.Empty);

    app.Logger.LogInformation(JsonSerializer.Serialize(responseAsync));

    return new ChatResponse()
    {
        Text = correctedBotResponse
    };
});

app.Run();