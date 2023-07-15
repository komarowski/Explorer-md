using Markdig;
using MarkdownExplorer.Entities;
using System.Text;

namespace MarkdownExplorer.Services
{
  /// <summary>
  /// Service for converting markdown files to html.
  /// </summary>
  public class ConvertService
  {
    private const string HtmlTemplatePath = "template.html";

    private static readonly MarkdownPipeline Pipeline = new MarkdownPipelineBuilder()
      .UseAdvancedExtensions()
      .Build();

    private readonly string treeViewPath;

    private readonly string indexHtmlPath;

    private readonly string folder;

    private readonly List<string> ignoreFolders;

    private readonly string template;

    /// <summary>
    /// Update all files anyway.
    /// </summary>
    public bool RefreshAll { get; set; }

    /// <summary>
    /// Service for rendering static content.
    /// </summary>
    /// <param name="appSettings">Application settings.</param>
    public ConvertService(AppSettings appSettings)
    {
      folder = appSettings.SourceFolder;
      ignoreFolders = appSettings.IngnoreFolders; 
      template = GetTemplate();
      treeViewPath = Path.Combine(folder, "treeview.js");
      indexHtmlPath = Path.Combine(folder, "index.html");
    }

    /// <summary>
    /// Get html template.
    /// </summary>
    /// <returns>Html template.</returns>
    private static string GetTemplate()
    {
      if (File.Exists(HtmlTemplatePath))
      {
        return File.ReadAllText(HtmlTemplatePath, Encoding.UTF8);
      }

      ConsoleService.WriteLog($"\"{HtmlTemplatePath}\" file not found. Default file generated.", LogType.Warning);
      File.WriteAllText(HtmlTemplatePath, StaticTemplate.DefaultHTML);
      return StaticTemplate.DefaultHTML;
    }

    /// <summary>
    /// Walk a directory tree by using recursion.
    /// </summary>
    /// <param name="root">Root directory.</param>
    /// <param name="tree">Storing information.</param>
    /// <returns>Information about the folder structure.</returns>
    private TreeStructure WalkDirectoryTree(DirectoryInfo root, TreeStructure tree)
    {
      FileInfo[]? mdFiles = null;
      try
      {
        mdFiles = root.GetFiles("*.md");
      }
      catch (UnauthorizedAccessException ex)
      {
        ConsoleService.WriteLog(ex.Message, LogType.Error);
      }
      catch (DirectoryNotFoundException ex)
      {
        ConsoleService.WriteLog(ex.Message, LogType.Error);
      }

      if (mdFiles is not null)
      {
        foreach (FileInfo mdFile in mdFiles)
        {
          var htmlCode = GetPathCode(mdFile.FullName);
          var htmlFile = new FileInfo(mdFile.FullName.Replace(".md", ".html"));
          if (!htmlFile.Exists 
            || mdFile.LastWriteTimeUtc > htmlFile.LastWriteTimeUtc 
            || RefreshAll)
          {
            tree.AddMarkdownToUpdate(mdFile.FullName, htmlCode);
          }
          var title = GetFileTitle(mdFile);
          tree.AddFileNode(htmlFile.FullName, htmlCode, title);
        }

        DirectoryInfo[] subDirs = root.GetDirectories();
        foreach (DirectoryInfo subDir in subDirs)
        {
          if (ignoreFolders.Contains(subDir.Name))
          {
            continue;
          }
          var folderCode = GetPathCode(subDir.FullName);
          tree.AddFolderBlockStart(folderCode, subDir.Name);
          tree = WalkDirectoryTree(subDir, tree);
          tree.AddFolderBlockEnd();
        }
      }

      return tree;
    }

    /// <summary>
    /// Converting all markdown files to html.
    /// </summary>
    public void ConvertAllHtml()
    {
      var tree = new TreeStructure();
      tree = WalkDirectoryTree(new DirectoryInfo(folder), tree);
      ConsoleService.WriteLog($"Updated or added {tree.MdFilesToConvert.Count} files.", LogType.Info);
      foreach (var markdownFile in tree.MdFilesToConvert)
      {
        ConvertHtml(markdownFile.SourcePath, markdownFile.Code);
      }

      if (!File.Exists(indexHtmlPath) || RefreshAll)
      {
        var html = InsertIntoTemplate(StaticTemplate.IndexHtmlText, string.Empty);
        File.WriteAllText(indexHtmlPath, html);
      }

      GenerateJS(tree);
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
        var targetPath = markdownPath.Replace(".md", ".html");
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
      template = template.Replace(StaticTemplate.IndexLinkPlace, indexHtmlPath);
      template = template.Replace(StaticTemplate.TreeViewLinkPlace, treeViewPath);
      return template;
    }

    /// <summary>
    /// Generate javascript file with tree view. 
    /// </summary>
    /// <param name="tree">Tree view.</param>
    public void GenerateJS(TreeStructure tree)
    {
      var jsText = StaticTemplate.GetTreeViewJS(tree.Content);
      File.WriteAllText(treeViewPath, jsText, Encoding.UTF8);
    }

    /// <summary>
    /// Get file title from # heading.
    /// </summary>
    /// <param name="file">File information.</param>
    /// <returns>Title.</returns>
    private static string GetFileTitle(FileInfo file)
    {
      using (var logStream = new StreamReader(file.FullName))
      {
        for (int i = 0; i < 3; i++)
        {
          var line = logStream.ReadLine();
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
      var relativePath = Path.GetRelativePath(folder, path);
      return relativePath
        .ToLower()
        .Replace(' ', '-')
        .Replace("\\", "__");
    }
  }
}