# ExifCleaner
ExifCleaner is a software/library for C# to clean exif data from images.
The assembly and executable was written in .Net 4.6.2.

[![Build status](https://ci.appveyor.com/api/projects/status/hoy66w2g8vgsx0ou?svg=true)](https://ci.appveyor.com/project/SeppPenner/exifcleaner)

## Basic usage:
```csharp
using System.Collections.ObjectModel;
using System.Windows;
using ExifRemove.Events;
using ExifRemove.Implementation;
using ExifRemove.Interfaces;
using ExifRemove.Models;
using MessageBox = System.Windows.MessageBox;

namespace ExifRemove.Example
{
    public class ExifCleanerExample
    {
        private readonly ObservableCollection<ExifItem> _exifItems =
            new ObservableCollection<ExifItem>();
        private IExifCleaner _cleaner;

        public ExifCleanerExample()
        {
            SetHandlerAndStart("C\\Users\\abc\\Test");
        }

        private void InitializeHandler()
        {
            _cleaner.ExifCleanCompleted += HandleCleanCompleted;
            _cleaner.ExifCleanProgress += HandleCleanProcess;
            _cleaner.ExceptionThrown += HandleExceptionThrown;
        }

        private void SetHandlerAndStart(string outPath)
        {
            _cleaner = new ExifCleaner(outPath, _exifItems);
            InitializeHandler();
            _cleaner.Start();
        }

        private void HandleCleanCompleted(object sender, ExifCleanCompletedEventArgs e)
        {
            MessageBox.Show(e.Text, "Exif remover", MessageBoxButton.OK,
                MessageBoxImage.Information);
        }

        private void HandleCleanProcess(object sender, ExifCleanProgressEventArgs e)
        {
            MessageBox.Show(e.Text, "Exif remover", MessageBoxButton.OK,
                MessageBoxImage.Information);
        }

        private void HandleExceptionThrown(object sender, ExceptionThrownEventArgs e)
        {
            MessageBox.Show(e.Message, "Exif remover", MessageBoxButton.OK,
                MessageBoxImage.Information);
        }
    }
}
```

## Screenshots:

Change history
--------------

* **Version 1.0.0.4 (2017-03-11)** : Switched to .Net 4.6.2. Cleaned up code. Added images to readme.
* **Version 1.0.0.3 (2016-12-03)** : Added basic usage to Readme.
* **Version 1.0.0.3 (2016-09-04)** : Fixed error in "all images" filter in the .exe-file
* **Version 1.0.0.2 (2016-08-27)** : Added "all images" to the filter in the .exe-file
* **Version 1.0.0.1 (2016-07-13)** : 1.0 release.