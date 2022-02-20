namespace MarkdownExplorer.Services
{
  /// <summary>
  /// Type of log records.
  /// </summary>
  public enum LogType
  {
    Info,
    Warning,
    Error
  }

  /// <summary>
  /// Service for logging.
  /// </summary>
  public class LogService
  {
    /// <summary>
    /// Write color text.
    /// </summary>
    /// <param name="text">Text.</param>
    /// <param name="color">Color.</param>
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
      WriteColor("Welcome to Markdown Explorer!\n\n", ConsoleColor.Cyan);
    }

    /// <summary>
    /// Write ending.
    /// </summary>
    public static void WriteEnding()
    {
      WriteColor("The program completed successfully.\n\n", ConsoleColor.Cyan);
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
        default:
          Console.Write(log);
          break;
      }
    }
  }
}