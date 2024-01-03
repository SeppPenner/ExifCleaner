# ExifCleaner
ExifCleaner is a software/library for C# to clean exif data from images.

[![Build status](https://ci.appveyor.com/api/projects/status/hoy66w2g8vgsx0ou?svg=true)](https://ci.appveyor.com/project/SeppPenner/exifcleaner)
[![GitHub issues](https://img.shields.io/github/issues/SeppPenner/ExifCleaner.svg)](https://github.com/SeppPenner/ExifCleaner/issues)
[![GitHub forks](https://img.shields.io/github/forks/SeppPenner/ExifCleaner.svg)](https://github.com/SeppPenner/ExifCleaner/network)
[![GitHub stars](https://img.shields.io/github/stars/SeppPenner/ExifCleaner.svg)](https://github.com/SeppPenner/ExifCleaner/stargazers)
[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](https://raw.githubusercontent.com/SeppPenner/ExifCleaner/master/License.txt)
[![Known Vulnerabilities](https://snyk.io/test/github/SeppPenner/ExifCleaner/badge.svg)](https://snyk.io/test/github/SeppPenner/ExifCleaner)
[![Blogger](https://img.shields.io/badge/Follow_me_on-blogger-orange)](https://franzhuber23.blogspot.de/)
[![Patreon](https://img.shields.io/badge/Patreon-F96854?logo=patreon&logoColor=white)](https://patreon.com/SeppPennerOpenSourceDevelopment)
[![PayPal](https://img.shields.io/badge/PayPal-00457C?logo=paypal&logoColor=white)](https://paypal.me/th070795)

## Basic usage
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

## Screenshot from the executable
![Screenshot from the executable](https://github.com/SeppPenner/ExifCleaner/blob/master/Screenshot_2.JPG "Screenshot from the executable")

Change history
--------------

See the [Changelog](https://github.com/SeppPenner/ExifCleaner/blob/master/Changelog.md).