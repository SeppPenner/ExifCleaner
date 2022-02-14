// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExifCleaner.cs" company="Hämmer Electronics">
//   Copyright (c) All rights reserved.
// </copyright>
// <summary>
//   The exif cleaner class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ExifRemove.Implementation;

/// <inheritdoc cref="IExifCleaner"/>
/// <summary>
/// The exif cleaner class.
/// </summary>
/// <seealso cref="IExifCleaner"/>
public class ExifCleaner : IExifCleaner
{
    /// <summary>
    /// The background worker.
    /// </summary>
    private readonly BackgroundWorker backgroundWorker = new() { WorkerReportsProgress = true };

    /// <summary>
    /// The exif items.
    /// </summary>
    private readonly ObservableCollection<ExifItem> exifItems = new();

    /// <summary>
    /// The output path.
    /// </summary>
    private readonly string outputPath = string.Empty;

    /// <summary>
    /// Initializes a new instance of the <see cref="ExifCleaner"/> class.
    /// </summary>
    /// <param name="outputPath">The output path.</param>
    /// <param name="exifItems">The exif items.</param>
    public ExifCleaner(string outputPath, ObservableCollection<ExifItem> exifItems)
    {
        this.outputPath = outputPath;
        this.exifItems = exifItems;
    }

    /// <inheritdoc cref="IExifCleaner"/>
    /// <summary>
    /// The exif clean completed event handler.
    /// </summary>
    /// <seealso cref="IExifCleaner"/>
    public event EventHandler<ExifCleanCompletedEventArgs>? ExifCleanCompleted;

    /// <inheritdoc cref="IExifCleaner"/>
    /// <summary>
    /// The exif clean progress event handler.
    /// </summary>
    /// <seealso cref="IExifCleaner"/>
    public event EventHandler<ExifCleanProgressEventArgs>? ExifCleanProgress;

    /// <inheritdoc cref="IExifCleaner"/>
    /// <summary>
    /// The exception thrown event handler.
    /// </summary>
    /// <seealso cref="IExifCleaner"/>
    public event EventHandler<ExceptionThrownEventArgs>? ExceptionThrown;

    /// <inheritdoc cref="IExifCleaner"/>
    /// <summary>
    /// The start method.
    /// </summary>
    /// <seealso cref="IExifCleaner"/>
    public void Start()
    {
        this.InitializeAndRunBackgroundWorker();
    }

    /// <summary>
    /// Gets the <see cref="BitmapEncoder"/> from the <see cref="ExifItem"/>.
    /// </summary>
    /// <param name="item">The exif item.</param>
    /// <returns>A new <see cref="BitmapEncoder"/>.</returns>
    private static BitmapEncoder? GetEncoderFromItem(ExifItem item)
    {
        var extension = item.Extension.ToLowerInvariant();
        BitmapEncoder? bitmapEncoder = null;

        if (extension.Equals(".jpeg") || extension.Equals(".jpg"))
        {
            bitmapEncoder = new JpegBitmapEncoder();
        }

        if (extension.Equals(".png"))
        {
            bitmapEncoder = new PngBitmapEncoder { Palette = BitmapPalettes.WebPaletteTransparent };
        }

        if (extension.Equals(".gif"))
        {
            bitmapEncoder = new GifBitmapEncoder { Palette = BitmapPalettes.WebPaletteTransparent };
        }

        if (extension.Equals(".bmp"))
        {
            bitmapEncoder = new BmpBitmapEncoder();
        }

        if (extension.Equals(".tif") || extension.Equals(".tiff"))
        {
            bitmapEncoder = new TiffBitmapEncoder();
        }

        return bitmapEncoder;
    }

    /// <summary>
    /// Initializes and runs the background worker.
    /// </summary>
    private void InitializeAndRunBackgroundWorker()
    {
        this.backgroundWorker.DoWork += this.BackgroundWork;
        this.backgroundWorker.ProgressChanged += this.BackgroundProgressChanged;
        this.backgroundWorker.RunWorkerCompleted += this.BackgroundRunWorkerCompleted;
        this.backgroundWorker.RunWorkerAsync();
    }

    /// <summary>
    /// Handles the background work.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event args.</param>
    private void BackgroundWork(object sender, DoWorkEventArgs e)
    {
        var worker = (BackgroundWorker)sender;
        double num = this.exifItems.Count;
        double counter = 0;

        foreach (var item in this.exifItems)
        {
            try
            {
                this.SavePurgedImage(item);
                var percentage = Convert.ToInt32(num / ++counter * 100);
                worker.ReportProgress(percentage > 100 ? 100 : percentage);
            }
            catch (Exception ex)
            {
                this.ExceptionThrown?.Invoke(this, new ExceptionThrownEventArgs(ex.Message + ex.StackTrace));
            }
        }
    }

    /// <summary>
    /// Handles the progress changed event.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event args.</param>
    private void BackgroundProgressChanged(object sender, ProgressChangedEventArgs e)
    {
        this.ExifCleanProgress?.Invoke(this, new ExifCleanProgressEventArgs(e.ProgressPercentage + " %"));
    }

    /// <summary>
    /// Handles the background worker completed event.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event args.</param>
    private void BackgroundRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
        this.ExifCleanCompleted?.Invoke(this, new ExifCleanCompletedEventArgs("All exif data removed and saved"));
    }

    /// <summary>
    /// Saves the purged image.
    /// </summary>
    /// <param name="item">The exif item.</param>
    private void SavePurgedImage(ExifItem item)
    {
        var source = new BitmapImage(new Uri("file://" + item.FullPath));
        var path = string.Empty;

        Application.Current.Dispatcher.Invoke(
            () => { path = Path.Combine(this.outputPath, item.Name) + item.Extension; });

        var bitmapEncoder = GetEncoderFromItem(item);

        if (bitmapEncoder is null)
        {
            throw new InvalidOperationException("The bitmap encode is null.");
        }

        bitmapEncoder.Frames.Add(BitmapFrame.Create(source));
        using var fileStream = new FileStream(path, FileMode.Create);
        bitmapEncoder.Save(fileStream);
    }
}
