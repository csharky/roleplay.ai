using System.Net;
using HandlebarsDotNet;

namespace Roleplay.AI.WebAPI;

public class HandlebarsHelper
{
    public static string RenderHandlebars(string str, object data)
    {
        return WebUtility.HtmlDecode(Handlebars.Compile(str)(data));
    }
}