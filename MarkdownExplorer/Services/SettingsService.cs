using MarkdownExplorer.Entities;
using Spectre.Console;
using System.Text.Json;

namespace MarkdownExplorer.Services
{
  /// <summary>
  /// Service for json files management.
  /// </summary>
  public static class SettingsService
  {
    /// <summary>
    /// App settings file.
    /// </summary>
    public const string AppSettingsFile = "appsettings.json";

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
        return default;
      }
      var jsonString = File.ReadAllText(filePath);
      return JsonSerializer.Deserialize<T>(jsonString);
    }

    private static bool IsDirectoryExists(string directory)
    {
      var result = Directory.Exists(directory);
      if (!result)
      {
        ConsoleService.WriteLog($"\"{directory}\" directory doesn't exist.", LogType.Error);
      }
      return result;
    }

    public static bool TryReadAppSettings(out AppSettings? appSettings)
    {
      var currentDirectory = Directory.GetCurrentDirectory();
      var files = new DirectoryInfo(currentDirectory)
        .GetFiles("appsettings*.json");
      if (files.Length > 0)
      {
        string appSettingsFile = files.Length > 1
          ? AnsiConsole.Prompt(
          new SelectionPrompt<string>()
              .Title("Select application settings file.")
              .PageSize(files.Length + 1)
              .AddChoices(files.Select(x => x.Name)))
          : files[0].Name;

        appSettings = ReadJson<AppSettings>(appSettingsFile);
        return appSettings is not null
          && IsDirectoryExists(appSettings.SourceFolder)
          && IsDirectoryExists(appSettings.TargetFolder);
      }

      appSettings = null;
      var defaultAppSettings = new AppSettings()
      {
        SourceFolder = currentDirectory,
        TargetFolder = currentDirectory,
        IngnoreFolders = new List<string>()
      };
      WriteJson(defaultAppSettings, AppSettingsFile);
      ConsoleService.WriteLog($"\"{AppSettingsFile}\" file doesn't exist.", LogType.Error);
      ConsoleService.WriteLog("The current folder is set as the source and target folder.", LogType.Info);
      ConsoleService.WriteLog($"\"{AppSettingsFile}\" file has been created.", LogType.Info);
      return false;
    }

    public static FileLocationMode GetFileLocationMode()
    {
      var fileLocation = AnsiConsole.Prompt(
        new SelectionPrompt<string>()
          .Title("What is the file location mode?")
          .PageSize(3)
          .AddChoices(new[] { "Absolute", "Relative" }));

      return fileLocation == "Absolute" 
        ? FileLocationMode.Absolute 
        : FileLocationMode.Relative;
    }
  }
}