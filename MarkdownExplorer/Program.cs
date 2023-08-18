using MarkdownExplorer;
using MarkdownExplorer.Entities;
using MarkdownExplorer.Services;

ConsoleService.WriteGreeting();

AppSettings? appSettings;
while (!SettingsService.TryReadAppSettings(out appSettings))
{
  ConsoleService.WriteLog("Failed to read application settings.", LogType.Warning);
  ConsoleService.ReadLine("Try to change the settings...");
}

FileLocationMode locationMode = SettingsService.GetFileLocationMode();
var convertService = new ConvertService(appSettings!, locationMode);
convertService.ConvertAllHtml();
var _ = new FileWatcher(convertService, appSettings!.SourceFolder);

string? command;
do
{
  command = ConsoleService.ReadLine("Enter command or press 'enter' to exit:");
  if (command == "refresh")
  {
    convertService.ConvertAllHtml(true);
  }
  else if (!string.IsNullOrEmpty(command))
  {
    ConsoleService.WriteLog($"The \"{command}\" command does not exist.", LogType.Info);
  }
}
while (!string.IsNullOrEmpty(command));