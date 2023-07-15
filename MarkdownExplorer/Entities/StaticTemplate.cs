namespace MarkdownExplorer.Entities
{
  /// <summary>
  /// Static content templates.
  /// </summary>
  public static class StaticTemplate
  {
    public const string FileCodePlace = "{@FileCode}";

    public const string MainBodyPlace = "{@MainBody}";

    public const string IndexLinkPlace = "{@IndexLink}";

    public const string TreeViewLinkPlace = "{@TreeViewLink}";

    /// <summary>
    /// Text for index.html file.
    /// </summary>
    public const string IndexHtmlText = @"<h1>Welcome to Explorer.md!</h1>
<p>This is the start page for your personal wiki.<p>";

    /// <summary>
    /// Default html template with inline css and js.
    /// </summary>
    public const string DefaultHTML = $@"<!DOCTYPE html>
<html>
  <head>
    <meta charset=""utf-8"" />
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"" />
    <link rel=""stylesheet"" href=""https://fonts.googleapis.com/css?family=Roboto"">
    <style type=""text/css"">
      .logo-container,table th{{background-color:var(--border)}}:root{{--link:#247aa8;--link-hover:#1b5b7e;--icon:#1b657e;--border:#303b44;--border-light:#4e606e;--code:#c2d6e6}}body{{margin:0;font-family:Roboto,Verdana,sans-serif;font-size:16px;line-height:1.5}}::-webkit-scrollbar{{width:5px;height:10px}}::-webkit-scrollbar-track{{background:#f1f1f1}}::-webkit-scrollbar-thumb{{background:#888;border-radius:5px}}::-webkit-scrollbar-thumb:hover{{background:#555}}.main-wrapper{{display:flex;flex:1 0 auto;width:100%}}.main-container{{display:flex;width:100%;flex-grow:1}}.container{{box-sizing:border-box;width:100%;padding:12px 24px;max-width:1320px;margin:0 auto}}.sidebar{{top:0;position:sticky;min-width:300px;width:300px;max-height:100vh;height:100vh;overflow:auto;outline:1px solid var(--border)}}.logo-container{{padding:16px;text-align:center}}.logo-text{{font-size:30px;text-decoration:none;font-weight:700;color:#fff}}.blog-container h1,.tree-view,h2,h3,h4,h5,h6{{font-weight:500}}.blog-container a,.logo-text:hover{{color:var(--link)}}.tree-view details>summary::before,.tree-view details[open]>summary::before,.tree-view-item::before{{font-size:25px;margin-right:5px;content:'aa';color:transparent}}.row{{display:flex;flex-direction:row;flex-wrap:nowrap;margin:0 2px}}.blog-col{{width:100%;flex:1 0;margin-left:0;max-width:75%}}.content-col{{max-width:25%;padding-left:20px}}.blog-container{{line-height:1.6}}.blog-container h1{{border-bottom:1px solid var(--border);margin-top:2rem}}.blog-container h2{{margin-top:2rem}}.blog-container pre{{border-radius:4px}}.blog-container img{{display:block;margin-left:auto;margin-right:auto;max-width:100%}}.blog-container code{{background-color:var(--code);border-radius:4px}}.blog-container details{{background:#f8fbff;border-radius:4px;box-sizing:border-box;padding:16px 36px;margin:5px 0;border:1px solid var(--border)}}.blog-container details>summary{{color:var(--border);font-size:1.1rem;cursor:pointer;text-decoration:underline;-webkit-text-decoration-style:dotted;text-decoration-style:dotted}}.content-container{{padding:12px 0;max-height:calc(100vh - 70px);overflow-y:auto;position:sticky;top:0}}.table-of-contents{{list-style:none;font-size:.9rem;border-left:1px solid var(--border);padding:5px 0 5px 10px}}.table-of-contents li{{margin:.8rem}}.table-of-contents a{{text-decoration:none;color:var(--link)}}.tree-view{{padding:12px;font-size:18px;color:var(--border)}}.tree-view details{{padding:6px 0 6px 12px}}.tree-view details>summary{{list-style-type:none;cursor:pointer}}.tree-view details>summary::-webkit-details-marker{{display:none}}.tree-view details>summary::before{{background:url(""data:image/svg+xml;utf8,<svg xmlns='http://www.w3.org/2000/svg' height='26px' viewBox='0 0 24 24' width='26px' fill='%231b657e'><path d='M0 0h24v24H0z' fill='none'/><path d='M10 4H4c-1.1 0-1.99.9-1.99 2L2 18c0 1.1.9 2 2 2h16c1.1 0 2-.9 2-2V8c0-1.1-.9-2-2-2h-8l-2-2z'/></svg> "") center no-repeat}}.tree-view details[open]>summary::before{{background:url(""data:image/svg+xml;utf8,<svg xmlns='http://www.w3.org/2000/svg' height='26px' viewBox='0 0 24 24' width='26px' fill='%231b657e'><path d='M0 0h24v24H0V0z' fill='none'/><path d='M20 6h-8l-2-2H4c-1.1 0-1.99.9-1.99 2L2 18c0 1.1.9 2 2 2h16c1.1 0 2-.9 2-2V8c0-1.1-.9-2-2-2zm0 12H4V8h16v10z'/></svg> "") center no-repeat}}.tree-view-item::before{{background:url(""data:image/svg+xml;utf8,<svg xmlns='http://www.w3.org/2000/svg' height='24px' viewBox='0 0 24 24' width='24px' fill='%231b657e'><path d='M0 0h24v24H0V0z' fill='none'/><path d='M8 16h8v2H8zm0-4h8v2H8zm6-10H6c-1.1 0-2 .9-2 2v16c0 1.1.89 2 1.99 2H18c1.1 0 2-.9 2-2V8l-6-6zm4 18H6V4h7v5h5v11z'/></svg> "") center no-repeat}}.tree-view-group{{border-left:2.5px dotted var(--border)}}.tree-view-item{{font-size:16px;font-weight:400;text-decoration:none;color:var(--border);display:flex;align-items:center;padding:0 12px;margin:6px 0;cursor:pointer}}.tree-view-item:hover{{color:var(--link-hover)}}.tree-view-item-current{{color:var(--link);border-radius:5px;background-color:var(--code);font-weight:700}}table td,table th{{border:1px solid var(--border-light);margin:0;padding:6px 13px}}tr{{break-inside:avoid;break-after:auto}}thead{{display:table-header-group}}table{{width:100%;overflow:auto;break-inside:auto;text-align:left;padding:0;word-break:initial;background-color:#fff;border-collapse:collapse;border-spacing:0px}}table tr{{margin:0;padding:0}}table tr:nth-child(2n),thead{{background-color:#f8f8f8}}table th{{color:#fff}}table td:first-child,table th:first-child{{margin-top:0}}table td:last-child,table th:last-child{{margin-bottom:0}}@media (max-width:1500px){{.blog-col{{max-width:100%}}.content-col{{opacity:0;width:0;border:none;font-size:0}}}}
    </style>
  </head>

  <body>
    <main class=""main-wrapper"">
      <div class=""sidebar"">
        <div class=""logo-container"">
          <a class=""logo-text"" href=""{IndexLinkPlace}"">Explorer.md</a>
        </div>
        <div id=""tree-view"" data-file=""{FileCodePlace}"" class=""tree-view"">
        </div>
      </div>

      <div class=""main-container"">
        <div class=""container"">
          <div class=""row"">
            <div class=""blog-col"">
              <div id=""blog"" class=""blog-container"">
                {MainBodyPlace}
              </div>
            </div>

            <div class=""content-col"">
              <div class=""content-container"">
                <ul id=""table-of-content"" class=""table-of-contents"">
                </ul>
              </div>
            </div>
          </div>
        </div>
      </div>
    </main>

    <script src=""{TreeViewLinkPlace}""></script>
    <script>
      if(treeView&&null!=treeView){{let e=treeView.dataset.file,l=document.getElementById(e);if(null!=l){{l.classList.add(""tree-view-item-current"");let t=[...e.matchAll(RegExp(""__"",""gi""))].map(e=>e.index);t.forEach(l=>{{let t=e.slice(0,l+2),n=document.getElementById(t);null!=n&&(n.open=!0)}})}}else{{let n=Array.from(treeView.querySelectorAll(""details""));n.forEach(e=>{{e.open=!0}})}}}}const blog=document.getElementById(""blog""),tableOfContents=document.getElementById(""table-of-content"");if(null!=blog&&null!=tableOfContents){{let o=blog.querySelectorAll(""h1, h2"");for(let a=0;a<o.length;a++){{let i=document.createElement(""li""),r=document.createElement(""a"");r.innerHTML=o[a].textContent,r.href=`#${{o[a].id}}`,i.appendChild(r),tableOfContents.appendChild(i)}}}}
    </script>
  </body>
</html>
";

    /// <summary>
    /// Get js code with tree view as string variable which contain html.
    /// Surely this is not the best solution, but I have not yet come up with a better one.
    /// </summary>
    /// <param name="treeViewData">Tree view in the form of html.</param>
    /// <returns>Code for treeview.js.</returns>
    public static string GetTreeViewJS(string treeViewData)
    {
      return $@"
const treeViewData = `{treeViewData}`;
const treeView = document.getElementById(""tree-view"");
treeView.innerHTML = treeViewData;";
    } 
  }
}