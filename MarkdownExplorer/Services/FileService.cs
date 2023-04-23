using MarkdownExplorer.Entities;
using System.Text.Json;

namespace MarkdownExplorer.Services
{
  /// <summary>
  /// Service for working with files.
  /// </summary>
  public static class FileService
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

    /// <summary>
    /// Get application settings.
    /// </summary>
    /// <param name="appSettingsPath">Application settings file path.</param>
    /// <returns>AppSettings or null.</returns>
    public static AppSettings? GetAppSettings(string appSettingsPath)
    {
      var result = ReadJson<AppSettings>(appSettingsPath);
      if (result is null)
      {
        var newAppSettings = new AppSettings()
        { 
          FolderFrom = Path.Combine(Environment.CurrentDirectory, "From"),
          FolderTo = Path.Combine(Environment.CurrentDirectory, "To")
        };
        WriteJson(newAppSettings, appSettingsPath);
      }
      return result;
    }

    /// <summary>
    /// Get file title.
    /// </summary>
    /// <param name="file">File information.</param>
    /// <returns>Title.</returns>
    public static string GetFileTitle(FileInfo file)
    {
      using (var logStream = new StreamReader(file.FullName))
      {
        var line = logStream.ReadLine();
        while (line is not null)
        {
          if (line.TrimStart().StartsWith("# "))
          {
            return line.TrimStart().Substring(2);
          }
          
          line = logStream.ReadLine();
        }
      }

      return "Unknown";
    }

    /// <summary>
    /// Copy assets source file to target folder.
    /// </summary>
    /// <param name="sourceFile">Source file.</param>
    /// <param name="folderTo">Target folder.</param>
    public static void CopyFile(FileInfo sourceFile, string folderTo)
    {
      var targetPath = Path.Combine(folderTo, sourceFile.Name);
      var targetFile = new FileInfo(targetPath);
      if (!File.Exists(targetPath) || sourceFile.LastWriteTimeUtc > targetFile.LastWriteTimeUtc)
      {
        File.Copy(sourceFile.FullName, targetPath, true);
      }
    }

    /// <summary>
    /// Get HTML code from markdown or folder full path.
    /// </summary>
    /// <param name="path">Markdown or folder full path.</param>
    /// <returns>HTML code.</returns>
    public static string GetHtmlCode(string folderFrom, string path)
    {
      if (path.EndsWith(".md"))
      {
        path = path.Replace(".md", String.Empty);
      }
      var relativePath = Path.GetRelativePath(folderFrom, path);
      return relativePath
        .ToLower()
        .Replace(' ', '-')
        .Replace("\\", "__");
    }
  }
}