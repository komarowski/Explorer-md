using MarkdownExplorer.Services;
using MarkdownExplorer.Entities;
using MarkdownExplorer;

LogService.WriteGreeting();

const string AppSettingsPath = "appsettings.json";
AppSettings? appSettings = FileService.GetAppSettings(AppSettingsPath);
if (appSettings is null)
{
  LogService.WriteLog("\"appsetting.json\" file doesn't exist\n", LogType.Error);
  return;
}

if (!Directory.Exists(appSettings.FolderFrom) || !Directory.Exists(appSettings.FolderTo))
{
  LogService.WriteLog("\"appsetting\" directories don't exist\n", LogType.Error);
  return;
}

var renderService = new RenderService(appSettings);
renderService.RenderAllHtml();

var _ = new FileWatcher(renderService, appSettings.FolderFrom);

LogService.WriteColor("Press enter to exit.\n\n", ConsoleColor.Cyan);
Console.ReadLine();