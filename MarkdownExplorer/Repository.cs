namespace MarkdownExplorer.Entities
{
  /// <summary>
  /// Repository for storing temporary data.
  /// </summary>
  public class Repository
  {
    private readonly List<MarkdownFile> markdownFilesCache;

    /// <summary>
    /// Files to new cache.
    /// </summary>
    public List<MarkdownFile> MarkdownFilesNewCache { get; }

    /// <summary>
    /// Files to update.
    /// </summary>
    public List<MarkdownFile> MarkdownFilesToUpdate { get; }

    /// <summary>
    /// Source folder.
    /// </summary>
    public string FolderFrom { get; }

    /// <summary>
    /// Target folder.
    /// </summary>
    public string FolderTo { get; }

    /// <summary>
    /// Repository for storing temporary data.
    /// </summary>
    /// <param name="folderFrom">Source folder.</param>
    /// <param name="folderTo">Target folder.</param>
    /// <param name="markdownFiles">List of files processed last time.</param>
    public Repository(string folderFrom, string folderTo, List<MarkdownFile> markdownFiles)
    {
      this.FolderFrom = folderFrom;
      this.FolderTo = folderTo;
      this.markdownFilesCache = markdownFiles;
      this.MarkdownFilesToUpdate = new List<MarkdownFile>();
      this.MarkdownFilesNewCache = new List<MarkdownFile>();
    }
    
    /// <summary>
    /// Add a markdown file to the repository.
    /// </summary>
    /// <param name="fileInfo">File information.</param>
    /// <returns>HTML file name.</returns>
    public string AddMarkdown(FileInfo fileInfo)
    {
      var relativePath = Path.GetRelativePath(this.FolderFrom, fileInfo.FullName);
      var markdownFile = markdownFilesCache.FirstOrDefault(file => file.SourcePath == relativePath);
      if (markdownFile is null)
      {
        markdownFile = new MarkdownFile 
        { 
          TargetName = TransformPath(relativePath.Replace(".md", "")), 
          SourcePath = relativePath, 
          LastUpdate = fileInfo.LastWriteTimeUtc 
        };
        this.MarkdownFilesToUpdate.Add(markdownFile);
      }
      else if (markdownFile.LastUpdate != fileInfo.LastWriteTimeUtc)
      {
        markdownFile.LastUpdate = fileInfo.LastWriteTimeUtc;
        this.MarkdownFilesToUpdate.Add(markdownFile);
      }

      this.MarkdownFilesNewCache.Add(markdownFile);
      return markdownFile.TargetName;
    }

    /// <summary>
    /// Determine which markdown files to delete.
    /// </summary>
    /// <returns>List of MarkdownFile.</returns>
    public List<MarkdownFile> GetMarkdownFilesToDelete()
    {
      return this.markdownFilesCache
        .Where(file => !this.MarkdownFilesNewCache.Contains(file))
        .ToList();
    }

    /// <summary>
    /// Transform path.
    /// </summary>
    /// <param name="path">File path.</param>
    /// <returns>New file path form.</returns>
    public static string TransformPath(string path)
    {
      return path
        .ToLower()
        .Replace(' ', '-')
        .Replace("\\", "__");
    }
  }
}