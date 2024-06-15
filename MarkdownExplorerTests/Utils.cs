namespace MarkdownExplorerTests
{
  public static class Utils
  {
    public static string NormalizeLineEndings(this string text)
    {
      return text.Replace("\r\n", "\n");
    }
  }
}
