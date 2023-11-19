using MarkdownExplorer.Services;

namespace MarkdownExplorer
{
  /// <summary>
  /// Watch for changes of markdown files in the source folder.
  /// </summary>
  public class FileWatcher
  {
    private readonly string rootDirectory;
    private FileSystemWatcher fileSystemWatcher;
    private readonly ConvertService renderService;
    private DateTime lastRead = DateTime.MinValue;

    public FileWatcher(ConvertService renderService, string rootDirectory)
    {
      this.rootDirectory = rootDirectory;
      this.renderService = renderService;
      InitializeFileWatcher();
    }

    private void InitializeFileWatcher()
    {
      this.fileSystemWatcher = new FileSystemWatcher(this.rootDirectory);
      this.fileSystemWatcher.Changed += OnChanged;
      this.fileSystemWatcher.Created += OnCreated;
      this.fileSystemWatcher.Deleted += OnDeleted;
      this.fileSystemWatcher.Error += OnError;
      this.fileSystemWatcher.Filter = "*.md";
      this.fileSystemWatcher.IncludeSubdirectories = true;
      this.fileSystemWatcher.EnableRaisingEvents = true;
    }

    private void OnError(object sender, ErrorEventArgs e)
    {
      var exception = e.GetException();
      ConsoleService.WriteLogBeforeReadLine($"An error occurred: {exception.Message}", LogType.Error, "> ");
    }

    private void OnChanged(object sender, FileSystemEventArgs e)
    {
      if (CheckLastReadTime(WatcherChangeTypes.Changed, e))
      {
        renderService.ConvertHtml(e.FullPath);
        ConsoleService.WriteLogBeforeReadLine($"changed \"{e.Name}\"", LogType.Info, "> ");
      }
    }

    private void OnCreated(object sender, FileSystemEventArgs e)
    {
      if (CheckLastReadTime(WatcherChangeTypes.Created, e))
      {
        ConsoleService.WriteLogBeforeReadLine($"added \"{e.Name}\"", LogType.Info, "> ");
        renderService.ConvertAllHtml();
      }
    }

    private void OnDeleted(object sender, FileSystemEventArgs e)
    {
      if (CheckLastReadTime(WatcherChangeTypes.Deleted, e))
      {
        ConsoleService.WriteLogBeforeReadLine($"deleted \"{e.Name}\"", LogType.Info, "> ");
        renderService.ConvertAllHtml();
      }
    }

    /// <summary>
    /// Duplicate Event Check.
    /// </summary>
    private bool CheckLastReadTime(WatcherChangeTypes watcherChangeType, FileSystemEventArgs e)
    {
      DateTime lastWriteTime = File.GetLastWriteTime(e.FullPath);
      var isValid = e.ChangeType == watcherChangeType && lastWriteTime != this.lastRead;
      if (isValid)
      {
        this.lastRead = lastWriteTime;
      }
      return isValid;
    }

    /// <summary>
    /// Restart FileSystemWatcher.
    /// </summary>
    public void RestartFileSystemWatcher()
    {
      this.fileSystemWatcher.Dispose();
      InitializeFileWatcher();
    }
  }
}
