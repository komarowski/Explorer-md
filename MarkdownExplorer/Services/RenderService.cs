using Markdig;
using MarkdownExplorer.Entities;
using System.Text;

namespace MarkdownExplorer.Services
{
  /// <summary>
  /// Service for rendering static content.
  /// </summary>
  public class RenderService
  {
    private const string TemplateHTML = "template.html";
    private const string GeneratedJS = "treeview.js";
    private string FolderFrom { get; }
    private string FolderTo { get; }
    private string Template { get; }

    /// <summary>
    /// Service for rendering static content.
    /// </summary>
    /// <param name="appSettings">Application settings.</param>
    public RenderService(AppSettings appSettings)
    {
      FolderFrom = appSettings.FolderFrom;
      FolderTo = appSettings.FolderTo;

      if (File.Exists(TemplateHTML))
      {
        Template = File.ReadAllText(TemplateHTML, Encoding.UTF8);
      }
      else
      {
        Template = StaticContent.StandardHTMLTemplate;
        File.WriteAllText(TemplateHTML, Template);
      }
    }

    /// <summary>
    /// Walk a directory tree by using recursion.
    /// </summary>
    /// <param name="root">Root directory.</param>
    /// <param name="tree">Storing information.</param>
    /// <returns>Information about the folder structure.</returns>
    private TreeStructure WalkDirectoryTree(DirectoryInfo root, TreeStructure tree)
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
        foreach (FileInfo sourceFile in files)
        {
          if (sourceFile.Extension == ".md")
          {
            var fileCode = FileService.GetHtmlCode(FolderFrom, sourceFile.FullName);
            var targetFile = new FileInfo(Path.Combine(FolderTo, $"{fileCode}.html"));
            if (!targetFile.Exists || sourceFile.LastWriteTimeUtc > targetFile.LastWriteTimeUtc)
            {
              tree.AddMarkdownToUpdate(sourceFile.FullName, fileCode);
            }
            var title = FileService.GetFileTitle(sourceFile);
            tree.AddFileNode(fileCode, title);
          }
          else
          {
            FileService.CopyFile(sourceFile, FolderTo);
          }
        }

        DirectoryInfo[] subDirs = root.GetDirectories();
        foreach (DirectoryInfo dirInfo in subDirs)
        {
          var folderName = dirInfo.Name;
          var folderCode = FileService.GetHtmlCode(FolderFrom, dirInfo.FullName);
          tree.AddFolderBlockStart(folderCode, folderName);
          tree = WalkDirectoryTree(dirInfo, tree);
          tree.AddFolderBlockEnd();
        }
      }

      return tree;
    }

    /// <summary>
    /// Converting all markdown files to html.
    /// </summary>
    public void RenderAllHtml()
    {
      var tree = new TreeStructure();
      tree = WalkDirectoryTree(new DirectoryInfo(FolderFrom), tree);
      foreach (var markdownFile in tree.MarkdownFilesToGenerate)
      {
        RenderHtml(markdownFile.SourcePath, markdownFile.Code);
      }

      var indexPath = Path.Combine(FolderTo, "index.html");
      if (!File.Exists(indexPath))
      {
        var html = InsertIntoTemplate(StaticContent.IndexHtmlText, "None");
        File.WriteAllText(indexPath, html);
      }

      GenerateJS(tree);
    }

    /// <summary>
    /// Converting markdown file to html and save.
    /// </summary>
    /// <param name="markdownPath">Markdown file path.</param>
    public void RenderHtml(string markdownPath, string? htmlCode = null)
    {
      if (File.Exists(markdownPath))
      {
        if (htmlCode is null)
        {
          htmlCode = FileService.GetHtmlCode(FolderFrom, markdownPath);
        }

        using var fileStream = new FileStream(markdownPath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
        using var streamReader = new StreamReader(fileStream, Encoding.UTF8);
        string markdown = streamReader.ReadToEnd();
        var pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();
        var html = Markdown.ToHtml(markdown, pipeline);
        var htmlWithTreeView = InsertIntoTemplate(html, htmlCode);
        var targetPath = Path.Combine(FolderTo, $"{htmlCode}.html");
        using var streamWriter = new StreamWriter(targetPath);
        streamWriter.WriteLine(htmlWithTreeView);
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
      var template = Template.Replace(StaticContent.TemplateFileCode, htmlCode);
      template = template.Replace(StaticContent.TemplateRenderBody, htmlText);
      return template;
    }

    /// <summary>
    /// Generate javascript file with tree view. 
    /// </summary>
    /// <param name="tree">Tree view.</param>
    public void GenerateJS(TreeStructure tree)
    {
      var jsText = StaticContent.JavaScriptTemplate.Replace(StaticContent.TemplateTreeView, tree.Content);
      var jsPath = Path.Combine(FolderTo, GeneratedJS);
      File.WriteAllText(jsPath, jsText, Encoding.UTF8);
    }
  }
}