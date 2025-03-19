namespace Roleplay.AI.WebAPI;

using System;
using System.Collections.Generic;
using System.IO;

public class PromptTemplate
{
    private readonly string _template;

    // Конструктор, принимающий готовый текст шаблона
    public PromptTemplate(string template)
    {
        _template = template ?? throw new ArgumentNullException(nameof(template));
    }

    // Конструктор, принимающий путь к файлу с шаблоном
    public PromptTemplate(string pathToFile, System.Text.Encoding encoding = null)
    {
        if (string.IsNullOrWhiteSpace(pathToFile))
            throw new ArgumentNullException(nameof(pathToFile));

        encoding ??= System.Text.Encoding.UTF8;
        _template = File.ReadAllText(pathToFile, encoding);
    }

    // Подстановка из словаря
    public string Format(Dictionary<string, string> variables)
    {
        if (string.IsNullOrEmpty(_template))
            return string.Empty;

        var result = _template;
        foreach (var kvp in variables)
        {
            var placeholder = "{" + kvp.Key + "}";
            result = result.Replace(placeholder, kvp.Value ?? string.Empty);
        }
        return result;
    }

    // Подстановка из анонимного/обычного объекта 
    public string Format(object obj)
    {
        if (obj == null)
            return _template;

        var dict = new Dictionary<string, string>();
        foreach (var prop in obj.GetType().GetProperties())
        {
            var value = prop.GetValue(obj, null)?.ToString() ?? string.Empty;
            dict[prop.Name] = value;
        }

        return Format(dict);
    }
}
