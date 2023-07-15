# Explorer.md

**Explorer.md** is a simple console application for generating html docs from markdown files for use on your local machine. It also generates a tree view of these files for quick navigation. 

## Features

- Converts .md files to .html with a pure HTML layout (you can customize CSS or JS)
- Shows a tree view of markdown files
- Shows table of contents (h1, h2 tags)
- Tracks changes in markdown files and modifies html accordingly.

## Demo

![](https://github.com/komarowski/Explorer-md/blob/main/images/demo.gif)

## Conventions

### Absolute paths

Files refer to each other and to static files via absolute paths. If you moved the source folder, run the "refresh" command in the application.

### Custom template

The default HTML is written in code and has basic inline styles. You can change `template.html` in the application root folder.

An example `assets/template.html` is provided in this repository. Copy also the `assets` to the folder with the HTML files. Note that you must provide absolute paths to assets files in the `template.html`.
