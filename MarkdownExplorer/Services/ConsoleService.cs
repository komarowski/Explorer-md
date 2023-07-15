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
      WriteColor("Welcome to Markdown Explorer!\n", ConsoleColor.Cyan);
      Console.WriteLine("--------------------------------------------");
      Console.WriteLine("Commands:");
      Console.WriteLine("\trefresh - force refresh all html files");
      Console.WriteLine("--------------------------------------------\n");
    }

    /// <summary>
    /// Write log.
    /// </summary>
    /// <param name="log">Log text.</param>
    /// <param name="logType">Log type.</param>
    public static void WriteLog(string log, LogType logType)
    {
      WriteColor("LOG: ", ConsoleColor.Gray);
      switch (logType)
      {
        case LogType.Info:
          WriteColor(log, ConsoleColor.DarkCyan);
          break;
        case LogType.Warning:
          WriteColor(log, ConsoleColor.DarkYellow);
          break;
        case LogType.Error:
          WriteColor(log, ConsoleColor.DarkRed);
          break;
        case LogType.Normal:
        default:
          Console.Write(log);
          break;
      }
      Console.WriteLine();
    }

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