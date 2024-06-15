using Markdig;
using MarkdownExplorer.MarkdownExtensions;

namespace MarkdownExplorerTests
{
  public class MarkdwonToHtmlTests
  {
    private static readonly MarkdownPipeline Pipeline;

    static MarkdwonToHtmlTests()
    {
      Pipeline = new MarkdownPipelineBuilder()
          .UseAdvancedExtensions()
          .Use<DetailsExtension>()
          .Use<SliderExtension>()
          .Build();
    }

    [Fact]
    public void GetSliderWithTwoImagesWithCustomHeight()
    {
      var markdownText = @"
        @@slider 300px
        ![image1.jpg](Text for image 1)
        ![image2.jpg](Text for image 2)
        @@
        ";

      var expectedHtml = @"
<div class=""w4-slider"" style=""height: 300px;"">
<div class=""w4-slide"">
<img src=""image1.jpg"">
<span>Text for image 1</span>
</div>
<div class=""w4-slide"">
<img src=""image2.jpg"">
<span>Text for image 2</span>
</div>
<button class=""w4-button-slider w4-button-slider--prev""> &lt; </button>
<button class=""w4-button-slider w4-button-slider--next""> &gt; </button>
</div>
";

      var actualHtml = Markdown.ToHtml(markdownText, Pipeline);

      Assert.Equal(expectedHtml.NormalizeLineEndings().Trim(), actualHtml.NormalizeLineEndings().Trim());
    }

    [Fact]
    public void GetSliderWithTwoImagesWithDefaultHeight()
    {
      var markdownText = @"
        @@slider
        ![image1.jpg](Text for image 1)
        ![image2.jpg](Text for image 2)
        @@
        ";

      var expectedHtml = @"
<div class=""w4-slider"" style=""height: 350px;"">
<div class=""w4-slide"">
<img src=""image1.jpg"">
<span>Text for image 1</span>
</div>
<div class=""w4-slide"">
<img src=""image2.jpg"">
<span>Text for image 2</span>
</div>
<button class=""w4-button-slider w4-button-slider--prev""> &lt; </button>
<button class=""w4-button-slider w4-button-slider--next""> &gt; </button>
</div>
";

      var actualHtml = Markdown.ToHtml(markdownText, Pipeline);

      Assert.Equal(expectedHtml.NormalizeLineEndings().Trim(), actualHtml.NormalizeLineEndings().Trim());
    }

    [Fact]
    public void GetSliderWithOneImageWithDefaultHeight()
    {
      var markdownText = @"
        @@slider
        ![image.jpg](Text for image 1)
        @@
        ";

      var expectedHtml = @"
<div class=""w4-slider"" style=""height: 350px;"">
<div class=""w4-slide"">
<img src=""image.jpg"">
<span>Text for image 1</span>
</div>
</div>
";

      var actualHtml = Markdown.ToHtml(markdownText, Pipeline);

      Assert.Equal(expectedHtml.NormalizeLineEndings().Trim(), actualHtml.NormalizeLineEndings().Trim());
    }

    [Fact]
    public void GetSliderWithOneImageWithDefaultHeightWithoutText()
    {
      var markdownText = @"
        @@slider
        ![image.jpg]()
        @@
        ";

      var expectedHtml = @"
<div class=""w4-slider"" style=""height: 350px;"">
<div class=""w4-slide"">
<img src=""image.jpg"">
<span></span>
</div>
</div>
";

      var actualHtml = Markdown.ToHtml(markdownText, Pipeline);

      Assert.Equal(expectedHtml.NormalizeLineEndings().Trim(), actualHtml.NormalizeLineEndings().Trim());
    }

    [Fact]
    public void GetDetailsWithSimpleText()
    {
      var markdownText = @"
        @@details How we can do it?
        It's very simple.
        Some text.
        @@
        ";

      var expectedHtml = @"
<details>
<summary>How we can do it?</summary>
<div><p>It's very simple.
Some text.</p>
</div>
</details>
";

      var actualHtml = Markdown.ToHtml(markdownText, Pipeline);

      Assert.Equal(expectedHtml.NormalizeLineEndings().Trim(), actualHtml.NormalizeLineEndings().Trim());
    }

    [Fact]
    public void GetDetailsWithCodeBlock()
    {
      var markdownText = @"
        @@details Sql query to get information about columns
        The `INFORMATION_SCHEMA.COLUMNS` view allows you to get information about all columns for all tables and views within a database.

        ```sql
        SELECT *
        FROM INFORMATION_SCHEMA.COLUMNS
        WHERE TABLE_NAME='TableName'
        ```
        @@
        ";

      var expectedHtml = @"
<details>
<summary>Sql query to get information about columns</summary>
<div><p>The <code>INFORMATION_SCHEMA.COLUMNS</code> view allows you to get information about all columns for all tables and views within a database.</p>
<pre><code class=""language-sql"">SELECT *
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME='TableName'
</code></pre>
</div>
</details>
";

      var actualHtml = Markdown.ToHtml(markdownText, Pipeline);

      Assert.Equal(expectedHtml.NormalizeLineEndings().Trim(), actualHtml.NormalizeLineEndings().Trim());
    }

    [Fact]
    public void GetHtmlWithTextAndSliderAndDetails()
    {
      var markdownText = @"# Header h1

## Header h2

@@slider
![image1.jpg](Text for image 1)
![image2.jpg](Text for image 2)
@@

Some text in a paragraph.

@@details How we can do it?
It's very simple.
Some text.
@@

Some other text in the paragraph with **bold font**.
";

      var expectedHtml = @"
<h1 id=""header-h1"">Header h1</h1>
<h2 id=""header-h2"">Header h2</h2>
<div class=""w4-slider"" style=""height: 350px;"">
<div class=""w4-slide"">
<img src=""image1.jpg"">
<span>Text for image 1</span>
</div>
<div class=""w4-slide"">
<img src=""image2.jpg"">
<span>Text for image 2</span>
</div>
<button class=""w4-button-slider w4-button-slider--prev""> &lt; </button>
<button class=""w4-button-slider w4-button-slider--next""> &gt; </button>
</div>
<p>Some text in a paragraph.</p>
<details>
<summary>How we can do it?</summary>
<div><p>It's very simple.
Some text.</p>
</div>
</details>
<p>Some other text in the paragraph with <strong>bold font</strong>.</p>
";

      var actualHtml = Markdown.ToHtml(markdownText, Pipeline);

      Assert.Equal(expectedHtml.NormalizeLineEndings().Trim(), actualHtml.NormalizeLineEndings().Trim());
    }
  }
}