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

    /// <summary>
    /// Converting markdown to html and saving html files.
    /// </summary>
    /// <param name="repository">Repository.</param>
    public static void MarkdownToHtml(Repository repository)
    {
      foreach (var file in repository.MarkdownFilesToUpdate)
      {
        var markdown = File.ReadAllText(Path.Combine(repository.FolderFrom, file.SourcePath), Encoding.UTF8);
        var pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();
        var html = Markdown.ToHtml(markdown, pipeline);
        var htmlWithTreeView = InsertIntoTemplate(html, file.TargetName);
        var targetPath = Path.Combine(repository.FolderTo, $"{file.TargetName}.html");
        File.WriteAllText(targetPath, htmlWithTreeView, Encoding.UTF8);
      }

      var indexPath = Path.Combine(repository.FolderTo, "index.html");
      if (!File.Exists(indexPath))
      {
        var html = InsertIntoTemplate(StaticContent.Index, "None");
        File.WriteAllText(indexPath, html);
      }
    }

    /// <summary>
    /// Insert into a template.
    /// </summary>
    /// <param name="htmlText">Main html text.</param>
    /// <param name="htmlName">HTML file name.</param>
    /// <returns></returns>
    private static string InsertIntoTemplate(string htmlText, string htmlName)
    {
      string template;
      if (File.Exists(TemplateHTML))
      {
        template = File.ReadAllText(TemplateHTML, Encoding.UTF8);
      }
      else
      {
        template = StaticContent.StandardHTMLTemplate;
        File.WriteAllText(TemplateHTML, template);
      }

      template = template.Replace("{@FileName}", htmlName);
      template = template.Replace("{@RenderBody}", htmlText);
      return template;
    }

    /// <summary>
    /// Generate javascript file with tree view. 
    /// </summary>
    /// <param name="tree">Tree view.</param>
    /// <param name="repository">Repository.</param>
    public static void GenerateJS(TreeStructure tree, Repository repository)
    {
      var jsText = StaticContent.JavaScriptTemplate.Replace("{@TreeView}", tree.Content);
      var jsPath = Path.Combine(repository.FolderTo, GeneratedJS);
      File.WriteAllText(jsPath, jsText, Encoding.UTF8);
    }
  }
}