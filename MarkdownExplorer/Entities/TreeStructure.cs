using System.Text;

namespace MarkdownExplorer.Entities
{
  /// <summary>
  /// Storing information about the source folder structure.
  /// </summary>
  public class TreeStructure
  {
    private readonly StringBuilder content;

    /// <summary>
    /// HTML tree view of folder structure.
    /// </summary>
    public string Content => this.content.ToString();

    /// <summary>
    /// List of markdown files to convert to html.
    /// </summary>
    public List<MarkdownFile> MdFilesToConvert { get; }

    /// <summary>
    /// Storing information about the folder structure.
    /// </summary>
    public TreeStructure()
    {
      this.content = new StringBuilder();
      this.MdFilesToConvert = new List<MarkdownFile>();
    }

    /// <summary>
    /// Add file node to tree view.
    /// </summary>
    /// <param name="filePath">Absolute file path.</param>
    /// <param name="fileCode">File code.</param>
    /// <param name="title">File title.</param>
    public void AddFileNode(string fileCode, string title, string href)
    {
      this.content.Append($"<a id=\"{fileCode}\" href=\"{href}\" class=\"tree-view-item\">{title}</a>");
    }

    /// <summary>
    /// Add start of folder block.
    /// </summary>
    /// <param name="folderCode">Folder code.</param>
    /// <param name="folderName">Folder name.</param>
    public void AddFolderBlockStart(string folderCode, string folderName)
    {
      this.content.Append($"<details id=\"{folderCode}__\"><summary>{folderName}</summary><div class=\"tree-view-group\">");
    }

    /// <summary>
    /// Add end of folder block.
    /// </summary>
    public void AddFolderBlockEnd()
    {
      this.content.Append("</div></details>");
    }

    /// <summary>
    /// Add a markdown to files to generate list.
    /// </summary>
    /// <param name="markdownPath">Markdown path.</param>
    /// <param name="code">File code.</param>
    public void AddMarkdownToUpdate(string markdownPath, string code)
    {
      this.MdFilesToConvert.Add(new MarkdownFile { Code = code, SourcePath = markdownPath });
    }
  }
}