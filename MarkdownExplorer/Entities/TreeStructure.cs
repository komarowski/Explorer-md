using System.Text;

namespace MarkdownExplorer.Entities
{
  /// <summary>
  /// Storing information about the folder structure.
  /// </summary>
  public class TreeStructure
  {
    private readonly StringBuilder content;

    /// <summary>
    /// HTML tree view of folder structure.
    /// </summary>
    public string Content => this.content.ToString();

    /// <summary>
    /// Files to generate.
    /// </summary>
    public List<MarkdownFile> MarkdownFilesToGenerate { get; }

    /// <summary>
    /// Storing information about the folder structure.
    /// </summary>
    public TreeStructure()
    {
      this.content = new StringBuilder();
      this.MarkdownFilesToGenerate = new List<MarkdownFile>();
    }

    /// <summary>
    /// Add file node to tree view.
    /// </summary>
    /// <param name="fileCode">File code.</param>
    /// <param name="title">File title.</param>
    public void AddFileNode(string fileCode, string title)
    {
      this.content.Append($"<a id=\"{fileCode}\" href=\"{fileCode}.html\" class=\"tree-view-item\">{title}</a>");
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
    /// Add a markdown to files to generate.
    /// </summary>
    /// <param name="markdownPath"></param>
    /// <param name="code"></param>
    public void AddMarkdownToUpdate(string markdownPath, string code)
    {
      var markdownFile = new MarkdownFile
      {
        Code = code,
        SourcePath = markdownPath,
      };
      this.MarkdownFilesToGenerate.Add(markdownFile);
    }
  }
}