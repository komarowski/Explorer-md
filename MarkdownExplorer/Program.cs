using MarkdownExplorer;
using MarkdownExplorer.Entities;
using MarkdownExplorer.Services;
using Spectre.Console;

ConsoleService.WriteGreeting();

AppSettings? appSettings;
while (!SettingsService.TryReadAppSettings(out appSettings))
{
  ConsoleService.WriteLog("Failed to read application settings. Try to change the settings.", LogType.Warning);
  AnsiConsole.Prompt(new TextPrompt<string>("(enter anything to continue) >").AllowEmpty());
}

var convertService = new ConvertService(appSettings!);
convertService.ConvertAllHtml();
var _ = new FileWatcher(convertService, appSettings!.SourceFolder);

string? command;
do
{
  command = AnsiConsole.Prompt(new TextPrompt<string>(">").AllowEmpty());
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