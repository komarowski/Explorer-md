namespace MarkdownExplorer.Entities
{
  /// <summary>
  /// Application settings.
  /// </summary>
  public class AppSettings
  {
    /// <summary>
    /// Source folder.
    /// </summary>
    public string SourceFolder { get; set; } = string.Empty;

    /// <summary>
    /// List of ignore folders in source folder.
    /// </summary>
    public List<string> IngnoreFolders { get; set; } = new List<string>();
  }
}