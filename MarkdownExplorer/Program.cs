using MarkdownExplorer.Services;
using MarkdownExplorer.Entities;

LogService.WriteGreeting();
var repository = FileService.GetAppRepository();
if (repository is not null)
{
  LogService.WriteLog("Walking Directory Tree...\n", LogType.Info);
  var tree = new TreeStructure();
  tree = FileService.WalkDirectoryTree(new DirectoryInfo(repository.FolderFrom), tree, repository);
  LogService.WriteLog($"Found {repository.MarkdownFilesNewCache.Count()} markdown files\n", LogType.Info);
  FileService.WriteNewCache(repository.MarkdownFilesNewCache);
  LogService.WriteLog($"Found {repository.MarkdownFilesToUpdate.Count()} markdown files to update\n", LogType.Info);

  var deleteFiles = repository.GetMarkdownFilesToDelete();
  FileService.DeleteFiles(deleteFiles, repository.FolderTo);
  LogService.WriteLog($"Found {deleteFiles.Count()} markdown files to delete\n", LogType.Info);

  LogService.WriteLog("Rendering markdown to html...\n", LogType.Info);
  RenderService.MarkdownToHtml(repository);
  RenderService.GenerateJS(tree, repository);
  LogService.WriteEnding();
}

Console.Read();