// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExifItem.cs" company="Hämmer Electronics">
//   Copyright (c) All rights reserved.
// </copyright>
// <summary>
//   The exif item class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ExifRemove.Models;

/// <summary>
/// The exif item class.
/// </summary>
public class ExifItem
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ExifItem"/> class.
    /// </summary>
    /// <param name="extension">The extension.</param>
    /// <param name="name">The name.</param>
    /// <param name="filePath">The file path.</param>
    /// <param name="fullPath">The full path.</param>
    public ExifItem(string extension, string name, string filePath, string fullPath)
    {
        this.Name = name;
        this.FilePath = filePath;
        this.Extension = extension;
        this.FullPath = fullPath;
    }

    /// <summary>
    /// Gets or sets the name.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the file path.
    /// </summary>
    public string FilePath { get; set; }

    /// <summary>
    /// Gets or sets the extension.
    /// </summary>
    public string Extension { get; set; }

    /// <summary>
    /// Gets or sets the full path.
    /// </summary>
    public string FullPath { get; set; }
}
