namespace MarkdownExplorer.Entities
{
  /// <summary>
  /// Markdown file information.
  /// </summary>
  public class MarkdownFile
  {
    /// <summary>
    /// HTML file name.
    /// </summary>
    public string TargetName { get; set; } = string.Empty;

    /// <summary>
    /// Relative markdown file path.
    /// </summary>
    public string SourcePath { get; set; } = string.Empty;

    /// <summary>
    /// Last update time.
    /// </summary>
    public DateTime LastUpdate { get; set; }
  }
}