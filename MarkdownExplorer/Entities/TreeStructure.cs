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
    /// Storing information about the folder structure.
    /// </summary>
    public TreeStructure()
    {
      this.content = new StringBuilder();
    }

    /// <summary>
    /// Add file node to tree view.
    /// </summary>
    /// <param name="fileName">File name.</param>
    /// <param name="title">File title.</param>
    public void AddFileNode(string fileName, string title)
    {
      this.content.Append($"<a id=\"{fileName}\" href=\"{fileName}.html\" class=\"tree-view-item\">{title}</a>");
    }

    /// <summary>
    /// Add start of folder block.
    /// </summary>
    /// <param name="folderId">Folder id.</param>
    /// <param name="folderName">Folder name.</param>
    public void AddFolderBlockStart(string folderId, string folderName)
    {
      this.content.Append($"<details id=\"{folderId}__\"><summary>{folderName}</summary><div class=\"tree-view-group\">");
    }

    /// <summary>
    /// Add end of folder block.
    /// </summary>
    public void AddFolderBlockEnd()
    {
      this.content.Append("</div></details>");
    }
  }
}