using Spectre.Console;

namespace MarkdownExplorer.Services
{
  /// <summary>
  /// Type of log records.
  /// </summary>
  public enum LogType
  {
    Normal,
    Info,
    Warning,
    Error
  }

  /// <summary>
  /// Service for console methods.
  /// </summary>
  public class ConsoleService
  {
    /// <summary>
    /// Write color text.
    /// </summary>
    /// <param name="text">Text.</param>
    /// <param name="color">Text color.</param>
    public static void WriteColor(string text, ConsoleColor color)
    {
      Console.ForegroundColor = color;
      Console.Write(text);
      Console.ForegroundColor = ConsoleColor.White;
    }

    /// <summary>
    /// Write greeting.
    /// </summary>
    public static void WriteGreeting()
    {
      AnsiConsole.Markup("[cyan3 bold underline]  Welcome to Markdown Explorer![/]\n\n");
      AnsiConsole.Write(new Rule("Commands:").LeftJustified());


      //WriteColor("Welcome to Markdown Explorer!\n", ConsoleColor.Cyan);
      //Console.WriteLine("--------------------------------------------");
      //Console.WriteLine("Commands:");
      //Console.WriteLine("\trefresh - force refresh all html files");
      //Console.WriteLine("--------------------------------------------\n");
    }

    /// <summary>
    /// Write log.
    /// </summary>
    /// <param name="text">Log text.</param>
    /// <param name="logType">Log type.</param>
    public static void WriteLog(string text, LogType logType)
    {
      AnsiConsole.Markup("[silver]LOG: [/]");
      var log = GetLogString(text, logType);
      AnsiConsole.Markup(log);
      AnsiConsole.WriteLine();
    }

    private static string GetLogString(string text, LogType logType) => (logType) switch
    {
      (LogType.Info) => $"[darkcyan]{text}[/]",
      (LogType.Warning) => $"[gold3_1]{text}[/]",
      (LogType.Error) => $"[darkcyan]{text}[/]",
      (LogType.Normal) or _ => text
    };

    /// <summary>
    /// Console.ReadLine() with enter text.
    /// </summary>
    /// <param name="enterText">Enter text.</param>
    /// <returns>Input string.</returns>
    public static string? ReadLine(string enterText)
    {
      Console.WriteLine(enterText);
      return Console.ReadLine();
    }
  }
}