
// Sets the Tree View to highlight the currently open file and open all subfolders in that file's path.
if (treeView && treeView != null){
  const file = treeView.dataset.file;
  const currentFile = document.getElementById(file);
  if (currentFile != null) {
    currentFile.classList.add("tree-view-item-current");
    const indexes = [...file.matchAll(new RegExp('__', 'gi'))].map(a => a.index);
    indexes.forEach(index => {
      const folder = file.slice(0, index + 2);
      let folderElement = document.getElementById(folder);
      if (folderElement != null) {
        folderElement.open = true;
      }
    });
  } else {
    const details = Array.from(treeView.querySelectorAll("details"))
    details.forEach(element => {
      element.open = true;
    });
  }
}

// Adds HTML headings (h1, h2) to the content table on the right for quick navigation.
const blog = document.getElementById("blog");
const tableOfContents = document.getElementById("table-of-content");

if (blog != null && tableOfContents != null){
  const headers = blog.querySelectorAll("h1, h2");

  for (let i = 0; i < headers.length; i++) {
    const li = document.createElement("li");
    const a = document.createElement('a');
    a.innerHTML = headers[i].textContent;
    a.href = `#${headers[i].id}`;
    li.appendChild(a);
    tableOfContents.appendChild(li);
  }
}
