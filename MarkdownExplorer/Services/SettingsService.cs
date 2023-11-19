using MarkdownExplorer.Entities;
using Spectre.Console;
using System.Text.Json;

namespace MarkdownExplorer.Services
{
  /// <summary>
  /// Service for reading app settings.
  /// </summary>
  public static class SettingsService
  {
    /// <summary>Default HTML template file.</summary>
    private const string DefaultTemplate = "template.html";

    /// <summary>App settings file.</summary>
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

    /// <summary>
    /// Checks if the directory exists.
    /// </summary>
    /// <param name="directory">Directory.</param>
    /// <returns>true if the directory exists.</returns>
    private static bool IsDirectoryExists(string directory)
    {
      var result = Directory.Exists(directory);
      if (!result)
      {
        ConsoleService.WriteLog($"\"{directory}\" directory doesn't exist.", LogType.Error);
      }
      return result;
    }

    /// <summary>
    /// Checks if the HTML template exists.
    /// </summary>
    /// <param name="templatePath">Template path.</param>
    /// <returns>true if the template exists.</returns>
    private static bool IsTemplateExists(string templatePath)
    {
      var result = File.Exists(templatePath);
      if (!result)
      {
        ConsoleService.WriteLog($"HTML template file not found.", LogType.Error);
      }
      return result;
    }

    /// <summary>
    /// Tries to read app settings.
    /// </summary>
    /// <param name="appSettings">Application settings.</param>
    /// <returns>true if the settings were read successfully.</returns>
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
          && IsDirectoryExists(appSettings.TargetFolder)
          && IsTemplateExists(appSettings.Template);
      }

      appSettings = null;
      var defaultAppSettings = new AppSettings()
      {
        SourceFolder = currentDirectory,
        TargetFolder = currentDirectory,
        Template = DefaultTemplate,
        IngnoreFolders = new List<string>()
      };
      WriteJson(defaultAppSettings, AppSettingsFile);
      File.WriteAllText("template.html", StaticTemplate.DefaultHTML);
      ConsoleService.WriteLog($"\"{AppSettingsFile}\" file doesn't exist.", LogType.Error);
      ConsoleService.WriteLog("The current folder is set as the source and target folder.", LogType.Info);
      ConsoleService.WriteLog($"\"{AppSettingsFile}\" file has been created.", LogType.Info);
      ConsoleService.WriteLog($"Default \"template.html\" has been created.", LogType.Info);     
      return false;
    }
  }
}