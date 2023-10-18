using System.Text.Json;

namespace SEProject.Services;

public static class FileStreamJsonExtensions
{
    public static void Serialize(this FileStream fileStream, object t, JsonSerializerOptions options)
    {
        using var writer = new StreamWriter(fileStream);
        var json = JsonSerializer.Serialize(t, options);
        writer.Write(json);
    }

    public static T? Deserialize<T>(this FileStream fileStream)
    {
        using var reader = new StreamReader(fileStream);
        var json = reader.ReadToEnd();
        return JsonSerializer.Deserialize<T>(json);
    }
}