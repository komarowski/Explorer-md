using System.Text.Json;

namespace MarkdownExplorer.Services
{
  /// <summary>
  /// Service for working with json files.
  /// </summary>
  public class JsonService
  {
    /// <summary>
    /// Write object with type T to json file.
    /// </summary>
    /// <typeparam name="T">Object Type.</typeparam>
    /// <param name="_object">Instance of type T.</param>
    /// <param name="filePath">File path.</param>
    public static void WriteJson<T>(T _object, string filePath)
    {
      string jsonString = JsonSerializer.Serialize(_object);
      File.WriteAllText(filePath, jsonString);
    }

    /// <summary>
    /// Read object with type T from json file.
    /// </summary>
    /// <typeparam name="T">Object Type.</typeparam>
    /// <param name="filePath">File path.</param>
    /// <returns>Instance of type T.</returns>
    public static T? ReadJson<T>(string filePath)
    {
      if (!File.Exists(filePath))
      {
        return default(T);
      }
      var jsonString = File.ReadAllText(filePath);
      return JsonSerializer.Deserialize<T>(jsonString);
    }
  }
}