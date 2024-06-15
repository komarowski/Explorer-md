namespace MarkdownExplorer.Entities
{
  /// <summary>
  /// Organization mode of generated html files.
  /// </summary>
  public enum FileLocationMode
  {
    /// <summary>
    /// The HTML files are located next to the markdown. 
    /// All paths are absolute.
    /// </summary>
    Absolute,
    /// <summary>
    /// HTML files and supporting files are located in the target folder. 
    /// All paths are relative.
    /// </summary>
    Relative
  }

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
    /// Target folder.
    /// </summary>
    public string TargetFolder { get; set; } = string.Empty;

    /// <summary>
    /// HTML template file.
    /// </summary>
    public string Template { get; set; } = string.Empty;

    /// <summary>
    /// Organization mode of generated html files.
    /// </summary>
    public FileLocationMode LocationMode { get; set; } = FileLocationMode.Absolute;

    /// <summary>
    /// List of ignore folders in source folder.
    /// </summary>
    public List<string> IngnoreFolders { get; set; } = [];
  }
}