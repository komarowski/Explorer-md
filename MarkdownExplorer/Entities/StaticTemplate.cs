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

    public const string TreeDataPlace = "{@TreeData}";

    /// <summary>
    /// Text for index.html file.
    /// </summary>
    public const string IndexHtmlText = @"<h1 id=""welcome-to-explorer.md"">Welcome to Explorer.md!</h1>
<h2 id=""basic-syntax"">Basic Syntax</h2>
<pre><code class=""language-markdown"">- I just love **bold text**.
- Italicized text is the *cat's meow*.

1. This text is ***really important***.
2. Text with `code text`
3. Text with [link to wikipedia](https://www.wikipedia.org/) 
</code></pre>
<ul>
<li>I just love <strong>bold text</strong>.</li>
<li>Italicized text is the <em>cat's meow</em>.</li>
</ul>
<ol>
<li>This text is <em><strong>really important</strong></em>.</li>
<li>Text with <code>code text</code></li>
<li>Text with <a href=""https://www.wikipedia.org/"">link to wikipedia</a></li>
</ol>
<h2 id=""details-with-code-block"">Details with code block</h2>
<div class=""code-toolbar""><pre class=""language-markdown"" tabindex=""0""><code class=""language-markdown"">@@details Sql query to get information about columns
<br/>
The `INFORMATION_SCHEMA.COLUMNS` view allows you to get information about all columns.
<br/>
<p>&#96;&#96;&#96;sql<p/>
<p>SELECT *<p/>
<p>FROM INFORMATION_SCHEMA.COLUMNS<p/>
<p>WHERE TABLE_NAME='TableName'<p/>
<p>&#96;&#96;&#96; <p/>
@@
</code></pre></div>
<details>
<summary>Sql query to get information about columns</summary>
<div><p>The <code>INFORMATION_SCHEMA.COLUMNS</code> view allows you to get information about all columns.</p>
<pre><code class=""language-sql"">SELECT *
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME='TableName'
</code></pre>
</div>
</details>
<h2 id=""image-slider"">Image slider</h2>
<pre><code class=""language-markdown"">@@slider
![image.jpg](You can use local images)
![image_link](Or you can use image from Internet)
@@
</code></pre>
<div class=""slider"" style=""height: 350px;"">
<div class=""slide"">
<img src=""https://upload.wikimedia.org/wikipedia/commons/thumb/d/d2/C_Sharp_Logo_2023.svg/1200px-C_Sharp_Logo_2023.svg.png"">
<span>Conversion to HTML is done using C#</span>
</div>
<div class=""slide"">
<img src=""https://upload.wikimedia.org/wikipedia/commons/thumb/9/99/Unofficial_JavaScript_logo_2.svg/1200px-Unofficial_JavaScript_logo_2.svg.png"">
<span>HTML uses JS for interactivity</span>
</div>
<button class=""button-slider button-slider--prev""> &lt; </button>
<button class=""button-slider button-slider--next""> &gt; </button>
</div>
<h2 id=""blockquotes"">Blockquotes</h2>
<pre><code class=""language-markdown"">&gt; Dorothy followed her through many of the beautiful rooms in her castle.
&gt;
&gt; The Witch bade her clean the pots and kettles and sweep the floor and keep the fire fed with wood.
</code></pre>
<blockquote>
<p>Dorothy followed her through many of the beautiful rooms in her castle.</p>
<p>The Witch bade her clean the pots and kettles and sweep the floor and keep the fire fed with wood.</p>
</blockquote>";

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
      .blog a,.content-table a,.header-link:hover{{color:var(--link)}}.blog code,.blog details,.blog pre{{border-radius:4px}}:root{{--link:#247aa8;--link-hover:#1b5b7e;--icon:#1b657e;--border:#303b44;--border-light:#4e606e;--code:#c2d6e6;--white:#fff;--background:#f2f2f2}}body{{margin:0;font-family:Roboto,Verdana,sans-serif;font-size:16px;line-height:1.5}}::-webkit-scrollbar{{width:5px;height:10px}}::-webkit-scrollbar-track{{background:#f1f1f1}}::-webkit-scrollbar-thumb{{background:#888;border-radius:5px}}.blog table th,.header{{background-color:var(--border)}}::-webkit-scrollbar-thumb:hover{{background:#555}}.flex{{display:flex;width:100%}}.header{{position:sticky;top:0;width:100%;height:50px;box-shadow:0 1px 2px rgb(60 64 67 / 30%);z-index:3;align-items:center}}.header-btn{{display:none;align-items:center;justify-content:center;width:30px;height:30px;cursor:pointer;margin-left:10px}}.header-btn:hover{{background-color:var(--border-light)}}.header-link{{margin-left:20px;font-size:28px;text-decoration:none;font-weight:700;color:var(--white)}}.blog h1,.tree-view,h2,h3,h4,h5,h6{{font-weight:500}}.tree-view details>summary::before,.tree-view details[open]>summary::before,.tree-view-item::before{{font-size:25px;margin-right:5px;content:'aa';color:transparent}}.main{{position:absolute;top:50px;overflow:hidden;height:calc(100% - 50px)}}.container-scroll{{width:100%;overflow-y:scroll;box-sizing:border-box}}.container-text{{padding:0 24px;max-width:1320px;margin:0 auto}}.container-content{{padding:12px 0;max-height:calc(100vh - 70px);overflow-y:auto;position:sticky;top:0}}.sidebar{{max-width:300px;outline:1px solid var(--border);background-color:var(--white);height:100%}}.col-blog{{width:100%;flex:1 0;margin-left:0;max-width:75%}}.col-content{{max-width:25%;padding-left:20px}}.content-table{{list-style:none;font-size:.9rem;border-left:1px solid var(--border);padding:5px 0 5px 10px}}.content-table li{{margin:.8rem}}.content-table a{{text-decoration:none}}.tree-view{{padding:12px;font-size:18px;color:var(--border)}}.tree-view details{{padding:6px 0 6px 12px}}.tree-view details>summary{{list-style-type:none;cursor:pointer}}.tree-view details>summary::-webkit-details-marker{{display:none}}.tree-view details>summary::before{{background:url(""data:image/svg+xml;utf8,<svg xmlns='http://www.w3.org/2000/svg' height='26px' viewBox='0 0 24 24' width='26px' fill='%231b657e'><path d='M0 0h24v24H0z' fill='none'/><path d='M10 4H4c-1.1 0-1.99.9-1.99 2L2 18c0 1.1.9 2 2 2h16c1.1 0 2-.9 2-2V8c0-1.1-.9-2-2-2h-8l-2-2z'/></svg>"") center no-repeat}}.tree-view details[open]>summary::before{{background:url(""data:image/svg+xml;utf8,<svg xmlns='http://www.w3.org/2000/svg' height='26px' viewBox='0 0 24 24' width='26px' fill='%231b657e'><path d='M0 0h24v24H0V0z' fill='none'/><path d='M20 6h-8l-2-2H4c-1.1 0-1.99.9-1.99 2L2 18c0 1.1.9 2 2 2h16c1.1 0 2-.9 2-2V8c0-1.1-.9-2-2-2zm0 12H4V8h16v10z'/></svg>"") center no-repeat}}.tree-view-item::before{{background:url(""data:image/svg+xml;utf8,<svg xmlns='http://www.w3.org/2000/svg' height='24px' viewBox='0 0 24 24' width='24px' fill='%231b657e'><path d='M0 0h24v24H0V0z' fill='none'/><path d='M8 16h8v2H8zm0-4h8v2H8zm6-10H6c-1.1 0-2 .9-2 2v16c0 1.1.89 2 1.99 2H18c1.1 0 2-.9 2-2V8l-6-6zm4 18H6V4h7v5h5v11z'/></svg>"") center no-repeat}}.tree-view-group{{border-left:2.5px dotted var(--border)}}.tree-view-item{{font-size:16px;font-weight:400;text-decoration:none;color:var(--border);display:flex;align-items:center;padding:0 12px;margin:6px 0;cursor:pointer}}.blog a:hover,.tree-view-item:hover{{color:var(--link-hover)}}.tree-view-item-current{{color:var(--link);border-radius:5px;background-color:#c3ccd5;font-weight:700}}.blog{{max-width:950px;line-height:1.6}}.blog h1{{border-bottom:1px solid var(--border);margin-top:2rem}}.blog img{{display:block;margin-left:auto;margin-right:auto;max-width:100%}}.blog code{{background-color:var(--code);padding:1px 2px;font-weight:bolder}}.blog pre code{{padding:0;font-weight:400}}.blog details{{background:#f8fbff;box-sizing:border-box;padding:16px 36px;margin:5px 0;border:1px solid var(--border)}}.blog details>summary{{color:var(--border);font-size:1.1rem;cursor:pointer;font-weight:bolder;-webkit-text-decoration-style:dotted;text-decoration-style:dotted}}.blog blockquote{{background:#d4ecf7;border-left:8px solid var(--icon);padding:.5rem 1rem;margin:1rem .5rem}}.blog table{{width:100%;overflow:auto;break-inside:auto;text-align:left;padding:0;word-break:initial;background-color:#fff;border-collapse:collapse;border-spacing:0px}}.blog table tr{{margin:0;padding:0}}.blog table td,.blog table th{{border:1px solid var(--border-light);margin:0;padding:6px 13px}}.blog table tr:nth-child(2n),thead{{background-color:#f8f8f8}}.blog table th{{color:#fff}}.blog table th:first-child,table td:first-child{{margin-top:0}}.blog table th:last-child,table td:last-child{{margin-bottom:0}}@media (max-width:650px){{.header-btn{{display:flex}}.sidebar{{display:none;position:absolute}}}}@media (max-width:1500px){{.col-blog{{max-width:100%}}.col-content{{opacity:0;width:0;border:none;font-size:0}}}}
    </style>
  </head>

  <body>
    <header class=""header flex"">
      <div id=""sidebar-btn"" class=""header-btn"">
        <svg xmlns=""http://www.w3.org/2000/svg"" height=""24"" viewBox=""0 -960 960 960"" width=""24"" fill=""#FFFFFF"">
          <path d=""M120-240v-80h720v80H120Zm0-200v-80h720v80H120Zm0-200v-80h720v80H120Z"" />
        </svg>
      </div>
      <a class=""header-link"" href=""{IndexLinkPlace}"">Explorer.md</a>
    </header>

    <main class=""main flex"">
      <div id=""sidebar"" class=""sidebar container-scroll"">
        <div id=""tree-view"" data-file=""{FileCodePlace}"" class=""tree-view""></div>
      </div>

      <div class=""container-scroll"">
        <div class=""container-text"">
          <div class=""flex"">
            <div class=""col-blog"">
              <div id=""blog"" class=""blog"">
                {MainBodyPlace}
              </div>
            </div>

            <div class=""col-content"">
              <div class=""container-content"">
                <ul id=""content-table"" class=""content-table""></ul>
              </div>
            </div>
          </div>
        </div>
      </div>
    </main>

    <script src=""{TreeDataPlace}""></script>
    <script>
      const generateHtmlTree=e=>{{if(!e||0===e.length)return"""";let t="""";for(let l of e)""Folder""===l.Type&&l.Children&&0!==l.Children.length?(t+=`<details id=""${{l.Id}}__""><summary>${{l.Name}}</summary><div class=""tree-view-group"">`,t+=generateHtmlTree(l.Children),t+=""</div></details>""):""File""===l.Type&&(t+=`<a id=""${{l.Id}}"" href=""${{l.Link}}"" class=""tree-view-item"">${{l.Name}}</a>`);return t}},setUpTree=e=>{{let t=document.getElementById(""tree-view"");if(!t){{console.error(""'tree-view' element not found!"");return}}if(0===e.length){{console.log(""nodeList is empty"");return}}t.innerHTML=generateHtmlTree(e);let l=t.dataset.file,n=document.getElementById(l);if(n){{n.classList.add(""tree-view-item-current"");let r=[...l.matchAll(RegExp(""__"",""gi""))].map(e=>e.index);r.forEach(e=>{{let t=l.slice(0,e+2),n=document.getElementById(t);n&&(n.open=!0)}})}}else{{let i=Array.from(t.querySelectorAll(""details""));i.forEach(e=>{{e.open=!0}})}}}},setUpContentTable=()=>{{let e=document.getElementById(""blog""),t=document.getElementById(""content-table"");if(e&&t){{let l=e.querySelectorAll(""h1, h2"");for(let n=0;n<l.length;n++){{let r=document.createElement(""li""),i=document.createElement(""a"");i.innerHTML=l[n].textContent,i.href=`#${{l[n].id}}`,r.appendChild(i),t.appendChild(r)}}}}}},setUpSidebar=()=>{{let e=document.getElementById(""sidebar-btn""),t=document.getElementById(""sidebar"");e&&t&&(e.onclick=()=>{{""block""===t.style.display?t.style.display=""none"":t.style.display=""block""}})}},main=e=>{{setUpTree(e),setUpContentTable(),setUpSidebar()}};main(nodeList);
    </script>
  </body>
</html>
";

    /// <summary>
    /// Get js code with folder structure as json object <see cref="Node"/>.
    /// </summary>
    /// <param name="treeData">Json string with folder structure.</param>
    /// <returns>Code for tree.js.</returns>
    public static string GetTreeDataJS(string treeData)
    {
      return $"const nodeList = {treeData};";
    } 
  }
}