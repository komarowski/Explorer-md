using MarkdownExplorer.Services;

namespace MarkdownExplorer
{
  /// <summary>
  /// Watch for changes of markdown files in the source folder.
  /// </summary>
  public class FileWatcher
  {
    private readonly FileSystemWatcher fileSystemWatcher;
    private readonly RenderService renderService;
    private DateTime lastRead = DateTime.MinValue;

    public FileWatcher(RenderService renderService, string rootDirectory)
    {
      this.renderService = renderService;
      this.fileSystemWatcher = new FileSystemWatcher(rootDirectory);

      this.fileSystemWatcher.Changed += OnChanged;
      this.fileSystemWatcher.Created += OnCreated;
      this.fileSystemWatcher.Deleted += OnDeleted;

      this.fileSystemWatcher.Filter = "*.md";
      this.fileSystemWatcher.IncludeSubdirectories = true;
      this.fileSystemWatcher.EnableRaisingEvents = true;
    }

    private void OnChanged(object sender, FileSystemEventArgs e)
    {
      DateTime lastWriteTime = File.GetLastWriteTime(e.FullPath);
      if (e.ChangeType != WatcherChangeTypes.Changed || lastWriteTime == this.lastRead)
      {
        return;
      }

      this.lastRead = lastWriteTime;
      renderService.RenderHtml(e.FullPath);
      LogService.WriteLog($"changed \"{e.Name}\"\n", LogType.Info);
    }

    private void OnCreated(object sender, FileSystemEventArgs e)
    {
      DateTime lastWriteTime = File.GetLastWriteTime(e.FullPath);
      if (e.ChangeType != WatcherChangeTypes.Created || lastWriteTime == this.lastRead)
      {
        return;
      }

      this.lastRead = lastWriteTime;
      renderService.RenderAllHtml();
      LogService.WriteLog($"added \"{e.Name}\"\n", LogType.Info);
    }

    private void OnDeleted(object sender, FileSystemEventArgs e)
    {
      DateTime lastWriteTime = File.GetLastWriteTime(e.FullPath);
      if (e.ChangeType != WatcherChangeTypes.Deleted || lastWriteTime == this.lastRead)
      {
        return;
      }

      this.lastRead = lastWriteTime;
      renderService.RenderAllHtml();
      LogService.WriteLog($"deleted \"{e.Name}\"\n", LogType.Info);
    }
  }
}
