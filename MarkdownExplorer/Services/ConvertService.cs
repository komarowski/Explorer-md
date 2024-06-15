using Markdig;
using MarkdownExplorer.Entities;
using MarkdownExplorer.MarkdownExtensions;
using System.Text;
using System.Text.Json;

namespace MarkdownExplorer.Services
{
  /// <summary>
  /// Service for converting markdown files to html.
  /// </summary>
  public class ConvertService
  {
    private const string IndexHtml = "index.html";
    private const string TreeDataJS = "tree.js";

    private readonly string treeDataPath;
    private readonly string indexHtmlPath;
    private readonly string sourceFolder;
    private readonly string targetFolder;
    private readonly List<string> ignoreFolders;
    private readonly string template;
    private readonly FileLocationMode locationMode;

    private static readonly MarkdownPipeline Pipeline = new MarkdownPipelineBuilder()
      .UseAdvancedExtensions()
      .Use<DetailsExtension>()
      .Use<SliderExtension>()
      .Build();

    /// <summary>
    /// Service for rendering static content.
    /// </summary>
    /// <param name="appSettings">Application settings.</param>
    public ConvertService(AppSettings appSettings)
    {
      template = GetTemplate(appSettings.Template);
      locationMode = appSettings.LocationMode;
      sourceFolder = appSettings.SourceFolder;
      targetFolder = appSettings.TargetFolder;
      ignoreFolders = appSettings.IngnoreFolders;
      treeDataPath = GetTargetPath(TreeDataJS);
      indexHtmlPath = GetTargetPath(IndexHtml);
    }

    /// <summary>
    /// Get target path.
    /// </summary>
    /// <param name="name">File name.</param>
    /// <returns>Target path.</returns>
    private string GetTargetPath(string name)
    {
      var targetDirectory = locationMode == FileLocationMode.Absolute ? sourceFolder : targetFolder;
      return Path.Combine(targetDirectory, name);
    }

    /// <summary>
    /// Get HTML template.
    /// </summary>
    /// <param name="templatePath">Template file path.</param>
    /// <returns>HTML template content.</returns>
    /// <exception cref="FileNotFoundException">Template file not exist.</exception>
    private static string GetTemplate(string templatePath)
    {
      if (!File.Exists(templatePath))
      {
        throw new FileNotFoundException($"\"{templatePath}\" not exists.");
      }
      return File.ReadAllText(templatePath, Encoding.UTF8);
    }

    /// <summary>
    /// Processes the file.
    /// </summary>
    /// <param name="file">File.</param>
    /// <param name="tree">Folder structure.</param>
    /// <param name="refreshAll">Update all files anyway.</param>
    private void ProcessFile(FileInfo file, TreeStructure tree, bool refreshAll)
    {
      var isAbsoluteLocationMode = locationMode == FileLocationMode.Absolute;
      if (file.Extension == ".md")
      {
        var htmlCode = GetPathCode(file.FullName);
        var htmlFile = isAbsoluteLocationMode
          ? new FileInfo(file.FullName.Replace(".md", ".html")) 
          : new FileInfo(Path.Combine(targetFolder, $"{htmlCode}.html"));
        if (!htmlFile.Exists
          || file.LastWriteTimeUtc > htmlFile.LastWriteTimeUtc
          || refreshAll)
        {
          tree.AddMarkdownToUpdate(file.FullName, htmlCode);
        }
        if (file.Name != "index.md")
        {
          var title = GetFileTitle(file);
          var href = isAbsoluteLocationMode
            ? "file:///" + htmlFile.FullName.Replace('\\', '/')
            : $"{htmlCode}.html";
          tree.AddFileNode(htmlCode, title, href);
        }
        return;
      }

      if (!isAbsoluteLocationMode 
        && file.Extension != ".js" 
        && file.Extension != ".html" 
        && file.Extension != ".css")
      {
        var targetPath = Path.Combine(targetFolder, file.Name);
        var targetFile = new FileInfo(targetPath);
        if (!File.Exists(targetPath) 
          || file.LastWriteTimeUtc > targetFile.LastWriteTimeUtc 
          || refreshAll)
        {
          File.Copy(file.FullName, targetPath, true);
        }
      }
    }

    /// <summary>
    /// Walk a directory tree by using recursion.
    /// </summary>
    /// <param name="root">Root directory.</param>
    /// <param name="tree">Storing information.</param>
    /// <returns>Information about the folder structure.</returns>
    private TreeStructure WalkDirectoryTree(DirectoryInfo root, TreeStructure tree, bool refreshAll)
    {
      FileInfo[]? files = null;
      try
      {
        files = root.GetFiles("*.*");
      }
      catch (UnauthorizedAccessException ex)
      {
        ConsoleService.WriteLog(ex.Message, LogType.Error);
      }
      catch (DirectoryNotFoundException ex)
      {
        ConsoleService.WriteLog(ex.Message, LogType.Error);
      }

      if (files is not null)
      {
        foreach (FileInfo file in files)
        {
          ProcessFile(file, tree, refreshAll);
        }

        DirectoryInfo[] subDirs = root.GetDirectories();
        foreach (DirectoryInfo subDir in subDirs)
        {
          if (ignoreFolders.Contains(subDir.Name))
          {
            continue;
          }
          var folderCode = GetPathCode(subDir.FullName);
          var parentNode = tree.CurrentNode;
          tree.CurrentNode = tree.AddFolderNode(folderCode, subDir.Name);
          tree = WalkDirectoryTree(subDir, tree, refreshAll);
          tree.CurrentNode = parentNode;
        }
      }

      return tree;
    }

    /// <summary>
    ///  Converting all markdown files to html.
    /// </summary>
    /// <param name="refreshAll">Update all files anyway.</param>
    /// <returns>Number of updated or added files.</returns>
    public int ConvertAllHtml(bool refreshAll = false)
    {
      var tree = new TreeStructure();
      tree = WalkDirectoryTree(new DirectoryInfo(sourceFolder), tree, refreshAll);
      foreach (var markdownFile in tree.MdFilesToConvert)
      {
        ConvertHtml(markdownFile.SourcePath, markdownFile.Code);
      }

      if (!File.Exists(indexHtmlPath) || refreshAll)
      {
        var html = InsertIntoTemplate(StaticTemplate.IndexHtmlText, string.Empty);
        File.WriteAllText(indexHtmlPath, html);
      }

      GenerateJS(tree);
      return tree.MdFilesToConvert.Count;
    }

    /// <summary>
    /// Converting markdown file to html and save.
    /// </summary>
    /// <param name="markdownPath">Markdown file path.</param>
    /// <param name="code">Path code.</param>
    public void ConvertHtml(string markdownPath, string? code = null)
    {
      if (File.Exists(markdownPath))
      {
        using var fileStream = new FileStream(markdownPath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
        using var streamReader = new StreamReader(fileStream, Encoding.UTF8);
        var markdown = streamReader.ReadToEnd();
        var htmlText = Markdown.ToHtml(markdown, Pipeline);
        code ??= GetPathCode(markdownPath);
        var html = InsertIntoTemplate(htmlText, code);
        var targetPath = locationMode == FileLocationMode.Absolute
          ? markdownPath.Replace(".md", ".html")
          : Path.Combine(targetFolder, $"{code}.html");
        using var streamWriter = new StreamWriter(targetPath);
        streamWriter.WriteLine(html);
      }
    }

    /// <summary>
    /// Insert html text into a template.
    /// </summary>
    /// <param name="htmlText">Main html text.</param>
    /// <param name="htmlCode">HTML file code.</param>
    /// <returns>Final html page.</returns>
    private string InsertIntoTemplate(string htmlText, string htmlCode)
    {
      var template = this.template.Replace(StaticTemplate.FileCodePlace, htmlCode);
      template = template.Replace(StaticTemplate.MainBodyPlace, htmlText);
      if (locationMode == FileLocationMode.Absolute)
      {
        template = template.Replace(StaticTemplate.IndexLinkPlace, indexHtmlPath);
        template = template.Replace(StaticTemplate.TreeDataPlace, treeDataPath);
      } 
      else
      {
        template = template.Replace(StaticTemplate.IndexLinkPlace, IndexHtml);
        template = template.Replace(StaticTemplate.TreeDataPlace, TreeDataJS);
      }
      return template;
    }

    /// <summary>
    /// Generate javascript file with tree view. 
    /// </summary>
    /// <param name="tree">Tree view.</param>
    public void GenerateJS(TreeStructure tree)
    {
      var jsonString = JsonSerializer.Serialize(tree.RootNode.Children);
      var jsText = StaticTemplate.GetTreeDataJS(jsonString);
      File.WriteAllText(treeDataPath, jsText, Encoding.UTF8);
    }

    /// <summary>
    /// Get file title from # heading.
    /// </summary>
    /// <param name="file">File information.</param>
    /// <returns>Title.</returns>
    private static string GetFileTitle(FileInfo file)
    {
      using (var fileStream = new FileStream(file.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
      using (var streamReader = new StreamReader(fileStream))
      {
        for (int i = 0; i < 3; i++)
        {
          var line = streamReader.ReadLine();
          if (line is not null && line.TrimStart().StartsWith("# "))
          {
            return line.Trim()[2..];
          }
        }
      }
      return file.Name;
    }

    /// <summary>
    /// Get code from markdown or folder full path.
    /// </summary>
    /// <param name="path">Markdown or folder full path.</param>
    /// <returns>Code.</returns>
    private string GetPathCode(string path)
    {
      path = path.Replace(".md", "");
      var relativePath = Path.GetRelativePath(sourceFolder, path);
      return relativePath
        .ToLower()
        .Replace(' ', '-')
        .Replace("\\", "__");
    }
  }
}