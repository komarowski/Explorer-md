using MarkdownExplorer.Entities;

namespace MarkdownExplorer.Services
{
  /// <summary>
  /// Service for working with files.
  /// </summary>
  public class FileService
  {
    private const string AppSettingsPath = "appsettings.json";

    private const string AppCachePath = "appcache.json";

    /// <summary>
    /// Get repository.
    /// </summary>
    /// <returns>Repository or null.</returns>
    public static Repository? GetAppRepository()
    {
      var appSettings = GetAppSettings();
      if (appSettings is not null && IsAppSettingsDirectoriesExist(appSettings))
      {
        var markdownFiles = GetMarkdownFiles();
        var result = new Repository(appSettings.FolderFrom, appSettings.FolderTo, markdownFiles);
        return result;    
      }
      
      return null;
    }

    /// <summary>
    /// Get application settings.
    /// </summary>
    /// <returns>AppSettings or null.</returns>
    public static AppSettings? GetAppSettings()
    {
      var result = JsonService.ReadJson<AppSettings>(AppSettingsPath);
      if (result is null)
      {
        LogService.WriteLog("\"appsetting.json\" file doesn't exist\n", LogType.Error);
        var appSettings = new AppSettings()
        { 
          FolderFrom = Path.Combine(Environment.CurrentDirectory, "From"),
          FolderTo = Path.Combine(Environment.CurrentDirectory, "To")
        };
        JsonService.WriteJson(appSettings, AppSettingsPath);
      }
      return result;
    }

    /// <summary>
    /// Checking if directories in "appsettings.json" exist.
    /// </summary>
    /// <param name="appSettings">AppSettings.</param>
    /// <returns>Exist?</returns>
    public static bool IsAppSettingsDirectoriesExist(AppSettings appSettings)
    {
      if (!Directory.Exists(appSettings.FolderFrom))
      {
        LogService.WriteLog("Source folder doesn't exist", LogType.Error);
        return false;
      }

      if (!Directory.Exists(appSettings.FolderTo))
      {
        LogService.WriteLog("Destination folder doesn't exist", LogType.Error);
        return false;
      }

      return true;
    }

    /// <summary>
    /// Get list of markdown files processed last time.
    /// </summary>
    /// <returns>List of MarkdownFile.</returns>
    public static List<MarkdownFile> GetMarkdownFiles()
    {
      var result = JsonService.ReadJson<List<MarkdownFile>>(AppCachePath);
      if (result is null)
      {
        LogService.WriteLog("\"appcache.json\" file doesn't exist\n", LogType.Info);
        return new List<MarkdownFile>();
      }
      return result;
    }

    /// <summary>
    /// Walk a directory tree by using recursion.
    /// </summary>
    /// <param name="root">Root directory.</param>
    /// <param name="tree">Storing information.</param>
    /// <param name="repository">Repository.</param>
    /// <returns>Information about the folder structure.</returns>
    public static TreeStructure WalkDirectoryTree(DirectoryInfo root, TreeStructure tree, Repository repository)
    {
      FileInfo[]? files = null;

      try
      {
        files = root.GetFiles("*.*");
      }
      catch (UnauthorizedAccessException ex)
      {
        LogService.WriteLog(ex.Message, LogType.Error);
      }
      catch (DirectoryNotFoundException ex)
      {
        LogService.WriteLog(ex.Message, LogType.Error);
      }

      if (files is not null)
      {
        foreach (FileInfo file in files)
        {
          if (file.Extension == ".md")
          {
            var htmlName = repository.AddMarkdown(file);
            var title = GetFileTitle(file);
            tree.AddFileNode(htmlName, title);
          }
          else
          {
            CopyFile(file, repository.FolderTo);
          }
        }

        DirectoryInfo[] subDirs = root.GetDirectories();
        foreach (DirectoryInfo dirInfo in subDirs)
        {
          var folderName = dirInfo.Name;
          var htmlName = Repository.TransformPath(Path.GetRelativePath(repository.FolderFrom, dirInfo.FullName));
          tree.AddFolderBlockStart(htmlName, folderName);
          tree = WalkDirectoryTree(dirInfo, tree, repository);
          tree.AddFolderBlockEnd();
        }
      }

      return tree;
    }

    /// <summary>
    /// Get file title.
    /// </summary>
    /// <param name="file">File information.</param>
    /// <returns>Title.</returns>
    private static string GetFileTitle(FileInfo file)
    {
      using (var logStream = new StreamReader(file.FullName))
      {
        var line = logStream.ReadLine();
        while (line is not null)
        {
          if (line.TrimStart().StartsWith("# "))
          {
            return line.TrimStart().Substring(2);
          }
          
          line = logStream.ReadLine();
        }
      }

      return "Unknown";
    }

    /// <summary>
    /// Copy assets file to target folder.
    /// </summary>
    /// <param name="file">Assets file.</param>
    /// <param name="folderTo">Target folder.</param>
    private static void CopyFile(FileInfo file, string folderTo)
    {
      var targetPath = Path.Combine(folderTo, file.Name);
      if (!File.Exists(targetPath))
      {
        File.Copy(file.FullName, targetPath, true);
      }
    }

    /// <summary>
    /// Delete html files from target folder.
    /// </summary>
    /// <param name="markdownFiles">Files to delete.</param>
    /// <param name="folderTo">Target folder.</param>
    public static void DeleteFiles(List<MarkdownFile> markdownFiles, string folderTo)
    {
      foreach (var file in markdownFiles)
      {
        var path = Path.Combine(folderTo, $"{file.TargetName}.html");
        if (File.Exists(path))
        {
          File.Delete(path);
        }
      }
    }

    /// <summary>
    /// Write "appcache.json".
    /// </summary>
    /// <param name="markdownFilesNewCache">Files to cache.</param>
    public static void WriteNewCache(List<MarkdownFile> markdownFilesNewCache)
    {
      JsonService.WriteJson(markdownFilesNewCache, AppCachePath);
    }
  }
}