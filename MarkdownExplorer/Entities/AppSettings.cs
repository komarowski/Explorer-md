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
    public string FolderFrom { get; set; } = string.Empty;

    /// <summary>
    /// Target folder.
    /// </summary>
    public string FolderTo { get; set; } = string.Empty; 
  }
}