using MarkdownExplorer.Services;
using MarkdownExplorer.Entities;
using MarkdownExplorer;


ConsoleService.WriteGreeting();

AppSettings? appSettings = JsonService.ReadJson<AppSettings>(JsonService.AppSettingsFile);
if (appSettings is null)
{
  ConsoleService.WriteLog($"\"{JsonService.AppSettingsFile}\" file doesn't exist.", LogType.Warning);
  string? sourceFolder = null;
  while (string.IsNullOrEmpty(sourceFolder))
  {
    sourceFolder = ConsoleService.ReadLine("Enter source folder:");
  }

  if (!Directory.Exists(sourceFolder))
  {
    ConsoleService.WriteLog($"Entered folder \"{sourceFolder}\" doesn't exist.", LogType.Warning);
    sourceFolder = Environment.CurrentDirectory;
    ConsoleService.WriteLog($"The current folder \"{sourceFolder}\" is set as the source folder.", LogType.Info);
  }

  appSettings = new AppSettings() { SourceFolder = sourceFolder };
  JsonService.WriteJson(appSettings, JsonService.AppSettingsFile);
  ConsoleService.WriteLog($"\"{JsonService.AppSettingsFile}\" file has been created.", LogType.Info);
}

if (!Directory.Exists(appSettings.SourceFolder))
{
  ConsoleService.WriteLog($"Source folder \"{appSettings.SourceFolder}\" doesn't exist.", LogType.Error);
  ConsoleService.ReadLine("Press 'enter' to exit.");
  return;
}

var convertService = new ConvertService(appSettings);
convertService.ConvertAllHtml();
var _ = new FileWatcher(convertService, appSettings.SourceFolder);

string? command;
do
{
  command = ConsoleService.ReadLine("Enter command or press 'enter' to exit:");
  if (command == "refresh")
  {
    convertService.RefreshAll = true;
    convertService.ConvertAllHtml();
    convertService.RefreshAll = false;
  }
  else if (!string.IsNullOrEmpty(command))
  {
    ConsoleService.WriteLog($"The \"{command}\" command does not exist.", LogType.Info);
  }
}
while (!string.IsNullOrEmpty(command));
