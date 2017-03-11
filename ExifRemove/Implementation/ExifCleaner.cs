using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using ExifRemove.Events;
using ExifRemove.Interfaces;
using ExifRemove.Models;

namespace ExifRemove.Implementation
{
    public class ExifCleaner : IExifCleaner
    {
        private readonly BackgroundWorker _backgroundWorker = new BackgroundWorker {WorkerReportsProgress = true};

        private readonly ObservableCollection<ExifItem> _exifItems;

        private readonly string _outPath;

        public ExifCleaner(string outPath, ObservableCollection<ExifItem> exifItems)
        {
            _outPath = outPath;
            _exifItems = exifItems;
        }

        public void Start()
        {
            InitializeAndRunBackgroundworker();
        }

        public event EventHandler<ExifCleanCompletedEventArgs> ExifCleanCompleted;

        public event EventHandler<ExifCleanProgressEventArgs> ExifCleanProgress;

        public event EventHandler<ExceptionThrownEventArgs> ExceptionThrown;

        private void InitializeAndRunBackgroundworker()
        {
            _backgroundWorker.DoWork += BackgroundWork;
            _backgroundWorker.ProgressChanged += BackgroundProgressChanged;
            _backgroundWorker.RunWorkerCompleted += BackgroundRunWorkerCompleted;
            _backgroundWorker.RunWorkerAsync();
        }

        private void BackgroundWork(object sender, DoWorkEventArgs e)
        {
            var backgroundWorker = (BackgroundWorker) sender;
            double num = _exifItems.Count;
            double counter = 0;
            foreach (var item in _exifItems)
                try
                {
                    SavePurgedImage(item);
                    var percentage = Convert.ToInt32(num / ++counter * 100);
                    backgroundWorker.ReportProgress(percentage > 100 ? 100 : percentage);
                }
                catch (Exception ex)
                {
                    ExceptionThrown?.Invoke(this, new ExceptionThrownEventArgs(ex.Message + ex.StackTrace));
                }
        }

        private void BackgroundProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            ExifCleanProgress?.Invoke(this, new ExifCleanProgressEventArgs(e.ProgressPercentage + " %"));
        }

        private void BackgroundRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            ExifCleanCompleted?.Invoke(this, new ExifCleanCompletedEventArgs("All exif data removed and saved"));
        }


        private void SavePurgedImage(ExifItem item)
        {
            var source = new BitmapImage(new Uri("file://" + item.Fullpath));
            var path = "";
            Application.Current.Dispatcher.Invoke(
                () => { path = Path.Combine(_outPath, item.Name) + item.Extension; });

            var bitmapEncoder = GetEncoderFromItem(item);
            bitmapEncoder.Frames.Add(BitmapFrame.Create(source));

            using (var fileStream = new FileStream(path, FileMode.Create))
            {
                bitmapEncoder.Save(fileStream);
            }
        }

        private BitmapEncoder GetEncoderFromItem(ExifItem item)
        {
            BitmapEncoder bitmapEncoder = null;
            if (item.Extension.Equals(".jpeg") ||
                item.Extension.Equals(".jpg"))
                bitmapEncoder = new JpegBitmapEncoder();
            else if (item.Extension.Equals(".png"))
                bitmapEncoder = new PngBitmapEncoder {Palette = BitmapPalettes.WebPaletteTransparent};
            else if (item.Extension.Equals(".gif"))
                bitmapEncoder = new GifBitmapEncoder {Palette = BitmapPalettes.WebPaletteTransparent};
            else if (item.Extension.Equals(".bmp"))
                bitmapEncoder = new BmpBitmapEncoder();
            else if (item.Extension.Equals(".tif") ||
                     item.Extension.Equals(".tiff"))
                bitmapEncoder = new TiffBitmapEncoder();
            return bitmapEncoder;
        }
    }
}